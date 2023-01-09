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

            //Check for changes, I don't remember why I do this twice, but it works so I'm not touching it.
            EditorGUI.BeginChangeCheck();
            if (EditorGUI.EndChangeCheck())
            {
                _soTarget.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }
            EditorGUI.BeginChangeCheck();

            //If we have InspectorTabs that aren't just the defulat InspectorTab.
            if (_InspectorTabs.Count > 1)
            {


                //Draw the tool bar
                _currentInspectorTab = GUILayout.Toolbar(_currentInspectorTab, _InspectorTabNames.ToArray());


                //Draw the m_Script property on the default InspectorTab, disable it
                if (_currentInspectorTab == 0)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(_soTarget.FindProperty("m_Script"));

                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.Space(10f);

                //Draw each property field
                InspectorTab current = _InspectorTabs[_currentInspectorTab];
                foreach (SerializedProperty field in current.Fields)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(field);
                    if (EditorGUI.EndChangeCheck())
                    {
                        //Check to see if the field has OnEditorValueChanged attribute
                        OnEditorValueChangedAttribute onEditorValueChangedAttribute = Attribute.GetCustomAttribute(field.serializedObject.targetObject.GetType().GetField(field.name), typeof(OnEditorValueChangedAttribute)) as OnEditorValueChangedAttribute;

                        //If it does, call the method
                        if (onEditorValueChangedAttribute != null)
                        {

                            MethodInfo method = field.serializedObject.targetObject.GetType().GetMethod(onEditorValueChangedAttribute.CallbackName);
                            method.Invoke(field.serializedObject.targetObject, null);
                        }
                    }

                }
            }
            else
            {
                // This mimics the behavior of DrawDefaultInspector so you don't see the default tab if there are no tabs
                // Get the serialized properties for the target object
                SerializedProperty property = _soTarget.GetIterator();

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

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(property, true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        //Check to see if the field has OnEditorValueChanged attribute
                        OnEditorValueChangedAttribute onEditorValueChangedAttribute = Attribute.GetCustomAttribute(property.serializedObject.targetObject.GetType().GetField(property.name), typeof(OnEditorValueChangedAttribute)) as OnEditorValueChangedAttribute;

                        //If it does, call the method
                        if (onEditorValueChangedAttribute != null)
                        {

                            MethodInfo method = property.serializedObject.targetObject.GetType().GetMethod(onEditorValueChangedAttribute.CallbackName);
                            if (method != null)
                            {
                                method.Invoke(property.serializedObject.targetObject, null);
                            }
                        }
                    }


                    if (property.name == "m_Script")
                    {
                        EditorGUI.EndDisabledGroup();
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