using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Perception.Engine;
using System.Reflection;
using System.Linq;
using System;

using Object = UnityEngine.Object;

namespace Perception.Editor
{
    /// <summary>
    /// The perception engine is meant to work out of the box and offer developers a lot of nice tools in a single place for free. I deliberated over whether or not
    /// to create a custom inspector as I originalyl did not want to cause collision with custom editor libraries like odin. However, I decided to go ahead with it as it is a very
    /// useful tool and Odin costs money. If you do not want to use it, simply delete the editor folder. You will have a lot of messy attributes to clean up,
    /// but I'm assuming if you don't need this stuff then you're not going to be using the attributes anyway.
    /// </summary>
    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    public class PerceptionEditor : UnityEditor.Editor
    {
        /// <summary>Serialized Object</summary>
        private SerializedObject _soTarget;

        /// <summary>The Fields for our given objects</summary>
        FieldInfo[] Fields;

        /// <summary>The Current InspectorTab</summary>
        private int _currentInspectorTab;

        /// <summary>List of our InspectorTabs
        /// TODO: Most of this should be moved to its own class eventually
        /// </summary>
        private List<InspectorTab> _InspectorTabs;

        /// <summary>List of InspectorTab names, used for tool bar</summary>
        private List<string> _InspectorTabNames;


        private ButtonsDrawer _buttonsDrawer;


        private void OnEnable()
        {
            //Get te type
            Type t = target.GetType();
            //Extract the fields
            Fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);

            //Set the serializedo bject
            _soTarget = new SerializedObject(targets);

            //Create the list of InspectorTabs and InspectorTab names
            _InspectorTabNames = new List<string>();
            _InspectorTabs = new List<InspectorTab>();

            //Create the default InspectorTab
            _InspectorTabNames.Add("Default");
            _InspectorTabs.Add(new InspectorTab("Default"));

            //Initialze the InspectorTabs
            InitializeInspectorTabs(Fields);

            _buttonsDrawer = new ButtonsDrawer(target);
        }

        /// <summary>Initializes the InspectorTabs and gives them their relevant attributes</summary>
        public void InitializeInspectorTabs(FieldInfo[] f)
        {
            //Initialize a list of strings
            List<string> l = new List<string>();

            //Go over each field
            for (int i = 0; i < f.Length; i++)
            {
                //Get the InspectorTab attribute
                TabAttribute InspectorTab = Attribute.GetCustomAttribute(f[i], typeof(TabAttribute)) as TabAttribute;

                //If it has the attribute
                if (InspectorTab != null)
                {
                    //If we don't have this InspectorTab yet
                    if (!_InspectorTabNames.Contains(InspectorTab.Name))
                    {
                        //Create a new InspectorTab
                        InspectorTab newInspectorTab = new InspectorTab();
                        //Set its name
                        newInspectorTab.Name = InspectorTab.Name;
                        //Add the field to the InspectorTabs fields
                        newInspectorTab.Fields.Add(_soTarget.FindProperty(f[i].Name));

                        //Add this InspectorTab to our InspectorTabs
                        _InspectorTabNames.Add(InspectorTab.Name);
                        _InspectorTabs.Add(newInspectorTab);
                    }
                    else
                    {

                        //Otherwise find this InspectorTab if it exists
                        InspectorTab existingInspectorTab = _InspectorTabs.Where(x => x.Name == InspectorTab.Name).FirstOrDefault();
                        //And if it does, add this field to its fields
                        if (existingInspectorTab != null)
                        {
                            existingInspectorTab.Fields.Add(_soTarget.FindProperty(f[i].Name));
                        }
                    }
                }
                else
                {
                    //If a field doesn't have a InspectorTab, chuck it in the default InspectorTabs fields
                    InspectorTab existingInspectorTab = _InspectorTabs.Where(x => x.Name == "Default").FirstOrDefault();
                    if (existingInspectorTab != null)
                    {
                        existingInspectorTab.Fields.Add(_soTarget.FindProperty(f[i].Name));
                    }
                }
            }
        }





        public override void OnInspectorGUI()
        {

            EditorGUI.BeginChangeCheck();
            if (EditorGUI.EndChangeCheck())
            {
                _soTarget.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }


            EditorGUI.BeginChangeCheck();
            //Draw the tool bar

            if (EditorGUI.EndChangeCheck())
            {
                _soTarget.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }

            //If we have InspectorTabs that aren't just the defulat InspectorTab.
            if (_InspectorTabs.Count > 1)
            {
                _currentInspectorTab = GUILayout.Toolbar(_currentInspectorTab, _InspectorTabNames.ToArray());
                //Draw each property field
                InspectorTab current = _InspectorTabs[_currentInspectorTab];

                if (_currentInspectorTab == 0)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(_soTarget.FindProperty("m_Script"));

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUI.BeginChangeCheck();

                foreach (SerializedProperty field in current.Fields)
                {
                    EditorGUILayout.PropertyField(field);
                }
                //TODO: This really sucks and has no place here, but is the only way I could get oneditorvalue changed to work
                if (EditorGUI.EndChangeCheck())
                {
                    // Loop over each field in _soTarget and check to see if it has the OnEditorValueChanged attribute
                    for (int i = 0; i < _soTarget.targetObject.GetType().GetFields().Length; i++)
                    {
                        //Get the field
                        FieldInfo field = _soTarget.targetObject.GetType().GetFields()[i];

                        //Get the OnEditorValueChanged attribute
                        OnEditorValueChangedAttribute onEditorValueChangedAttribute = Attribute.GetCustomAttribute(field, typeof(OnEditorValueChangedAttribute)) as OnEditorValueChangedAttribute;

                        //If it does, call the method
                        if (onEditorValueChangedAttribute != null)
                        {
                            MethodInfo method = field.DeclaringType.GetMethod(onEditorValueChangedAttribute.CallbackName);
                            method.Invoke(_soTarget.targetObject, null);
                            _soTarget.ApplyModifiedProperties();
                            _soTarget.UpdateIfRequiredOrScript();
                        }
                    }

                }
            }
            else
            {
                // This mimics the behavior of DrawDefaultInspector so you don't see the default tab if there are no tabs
                // Get the serialized properties for the target object
                SerializedProperty property = _soTarget.GetIterator();
                EditorGUI.BeginChangeCheck();

                // Iterate through all properties and draw them
                bool enterChildren = true;
                while (property.NextVisible(enterChildren))
                {
                    //Disable the m_Script property
                    if (property.name == "m_Script")
                    {
                        EditorGUI.BeginDisabledGroup(true);
                    }
                    enterChildren = false;
                    EditorGUILayout.PropertyField(property, true);
                    if (property.name == "m_Script")
                    {
                        EditorGUI.EndDisabledGroup();
                    }
                }
                //TODO: This really sucks and has no place here, but is the only way I could get oneditorvalue changed to work, this whole module is going to need
                //a proper re-write at some point
                if (EditorGUI.EndChangeCheck())
                {
                    //Loop over each field in _soTarget and check to see if it has the OnEditorValueChanged attribute
                    for (int i = 0; i < _soTarget.targetObject.GetType().GetFields().Length; i++)
                    {
                        //Get the field
                        FieldInfo field = _soTarget.targetObject.GetType().GetFields()[i];

                        //Get the OnEditorValueChanged attribute
                        OnEditorValueChangedAttribute onEditorValueChangedAttribute = Attribute.GetCustomAttribute(field, typeof(OnEditorValueChangedAttribute)) as OnEditorValueChangedAttribute;

                        //If it does, call the method
                        if (onEditorValueChangedAttribute != null)
                        {
                            MethodInfo method = field.DeclaringType.GetMethod(onEditorValueChangedAttribute.CallbackName);
                            method.Invoke(_soTarget.targetObject, null);
                            _soTarget.ApplyModifiedProperties();
                            _soTarget.UpdateIfRequiredOrScript();
                        }
                    }

                }

            }

            //Draw the buttons
            _buttonsDrawer.DrawButtons(targets);



            if (EditorGUI.EndChangeCheck())
            {
                _soTarget.ApplyModifiedProperties();
            }

        }
    }


}