using Perception.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
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

        public static PerceptionEditor Instance;


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

        private Dictionary<string, List<SerializedProperty>> _boxGroups = new Dictionary<string, List<SerializedProperty>>();


        private void OnEnable()
        {
            if (targets[0] == null)
            {
                return;
            }
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
            InitializeBoxGroups(Fields);

            _buttonsDrawer = new ButtonsDrawer(target);

            Instance = this;

        }



        /// <summary>Initializes the InspectorTabs and gives them their relevant attributes
        /// TODO: This should be moved to its own class eventually
        /// </summary>
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

        public void InitializeBoxGroups(FieldInfo[] f)
        {
            //Go over each field
            for (int i = 0; i < f.Length; i++)
            {
                //Get the InspectorTab attribute
                BoxGroupAttribute boxGroup = Attribute.GetCustomAttribute(f[i], typeof(BoxGroupAttribute)) as BoxGroupAttribute;

                //If it has the attribute
                if (boxGroup != null)
                {
                    //If we don't have this InspectorTab yet
                    if (!_boxGroups.ContainsKey(boxGroup.Name))
                    {
                        //Create a new InspectorTab
                        List<SerializedProperty> newBoxGroup = new List<SerializedProperty>();
                        //Set its name
                        newBoxGroup.Add(_soTarget.FindProperty(f[i].Name));

                        //Add this InspectorTab to our InspectorTabs
                        _boxGroups.Add(boxGroup.Name, newBoxGroup);
                    }
                    else
                    {

                        //Otherwise find this InspectorTab if it exists
                        List<SerializedProperty> existingBoxGroup = _boxGroups[boxGroup.Name];
                        //And if it does, add this field to its fields
                        if (existingBoxGroup != null)
                        {
                            existingBoxGroup.Add(_soTarget.FindProperty(f[i].Name));
                        }
                    }
                }
            }
        }

        private bool FieldIsInBoxGroup(SerializedProperty field)
        {
            if (field == null || field.serializedObject.targetObject == null)
            {
                return false;
            }


            if (field.serializedObject.targetObject.GetType().GetField(field.name) == null)
            {
                return false;
            }
            //Return true if field has boxgroup attribute
            return Attribute.GetCustomAttribute(field.serializedObject.targetObject.GetType().GetField(field.name), typeof(BoxGroupAttribute)) != null;
        }

        public Attribute GetFieldAttribute<T>(SerializedProperty field)
        {
            return Attribute.GetCustomAttribute(field.serializedObject.targetObject.GetType().GetField(field.name), typeof(T));
        }

        private static T Load<T>(string path) where T : Object
        {
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(Assembly.GetExecutingAssembly());
            var packagePath = $"{packageInfo.assetPath}/{path}".Replace('\\', '/');
            var result = AssetDatabase.LoadAssetAtPath<T>(packagePath);
            if (result)
            {
                return result;
            }
            Debug.LogError($"Failed to load [{typeof(T)}] from [{path}]");
            return default;
        }

        private static T LoadFromEditorDefaultResources<T>(string path) where T : Object =>
            Load<T>($"Editor Default Resources/{path}");

        private static T LoadFromGizmos<T>(string path) where T : Object =>
            Load<T>($"Gizmos/{path}");



        public void DrawBoxGroups()
        {
            InspectorTab current = _InspectorTabs[_currentInspectorTab];
            foreach (var boxGroup in _boxGroups)
            {
                List<string> tabs = new List<string>();

                //Check if each field has a tab attribute
                for (int i = 0; i < boxGroup.Value.Count; i++)
                {
                    TabAttribute tab = GetFieldAttribute<TabAttribute>(boxGroup.Value[i]) as TabAttribute;
                    if (tab != null)
                    {
                        tabs.Add(tab.Name);
                    }
                    else
                    {
                        tabs.Add("Default");
                    }
                }

                //If the current tabs name exists in the list of tabs, draw the boxgroup
                //This essentially draws it in a tab if one of the fields has a tab attribute
                //TODO: Make it only draw the fields that have the same tab attribute
                if (tabs.Count > 0)
                {
                    if (!tabs.Contains(current.Name))
                    {

                        continue;
                    }
                }

                Color c = GUI.backgroundColor;
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.BeginVertical(PerceptionStyles.BoxGroupHeaderStyle);
                GUI.backgroundColor = c;
                boxGroup.Value[0].isExpanded = EditorGUILayout.Foldout(boxGroup.Value[0].isExpanded, boxGroup.Key, true, PerceptionStyles.BoxGroupFoldoutStyle);
                EditorGUILayout.EndVertical();

                if (boxGroup.Value[0].isExpanded)
                {
                    EditorGUILayout.BeginVertical(PerceptionStyles.BoxGroupStyle);
                    for (int i = 0; i < boxGroup.Value.Count; i++)
                    {
                        EditorGUILayout.PropertyField(boxGroup.Value[i], true);
                    }
                    EditorGUILayout.EndVertical();
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

                //Add a little space
                EditorGUILayout.Space();

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
                    if (!FieldIsInBoxGroup(field))
                    {
                        EditorGUILayout.PropertyField(field);
                    }
                }

                DrawBoxGroups();

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

                    if (!FieldIsInBoxGroup(property))
                    {
                        EditorGUILayout.PropertyField(property, true);
                    }

                    if (property.name == "m_Script")
                    {
                        EditorGUI.EndDisabledGroup();
                    }
                }

                DrawBoxGroups();


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

        /// <summary>
        /// This method effectively refreshes the SO target. Needed if another object modifies a serialized object like in the LDToolbox
        /// </summary>
        /// <param name="soTarget"></param>
        public void SetSoTarget(SerializedObject soTarget)
        {
            _soTarget = soTarget;
            //Get the fields from the target
            Type t = _soTarget.targetObject.GetType();

            //Extract the fields
            Fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);

            if (_boxGroups != null)
            {
                _boxGroups.Clear();
            }

            if (_InspectorTabNames != null)
            {
                _InspectorTabNames.Clear();
                _InspectorTabNames.Add("Default");

            }

            if (_InspectorTabs != null)
            {
                _InspectorTabs.Clear();
                _InspectorTabs.Add(new InspectorTab("Default"));
            }

            _currentInspectorTab = 0;

            //Re-initialize the inspector tabs
            InitializeInspectorTabs(Fields);
            InitializeBoxGroups(Fields);


            Repaint();
        }

    }


}

