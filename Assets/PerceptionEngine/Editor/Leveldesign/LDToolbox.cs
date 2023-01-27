using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using System.Reflection;
using Perception.Engine;

namespace Perception.Editor
{


    public class LDToolbox : EditorWindow
    {
        [MenuItem("Window/Leveldesign Toolbox")]
        public static void ShowWindow()
        {
            GetWindow<LDToolbox>("Leveldesign Toolbox");
        }

        //On initialize 
        private void OnEnable()
        {
            //Create the scene camera
            CreateSceneCamera();

            //Load all the level entities
            LevelEntities.Clear();
            foreach (var obj in FindObjectsOfType<LevelEntity>())
            {
                var entity = obj.GetComponent<LevelEntity>();
                if (entity != null)
                {
                    LevelEntities.Add(entity);
                }
            }

            //Set the selected entity to the first one
            _selectedEntity = null;

        }

        //On destroy
        private void OnDisable()
        {
            //Destroy the scene camera
            DestroyImmediate(_sceneCamera.gameObject);
        }


        public List<LevelEntity> LevelEntities = new List<LevelEntity>();

        private LevelEntity _selectedEntity;
        private int _levelEntityIndex = 0;
        private string _filterText = "";
        private bool _entityLocked = false;
        private SerializedObject _serializedObject;

        private enum FilterTypes
        {
            All,
            Type,
            Name
        }

        private FilterTypes _filterType = FilterTypes.All;

        Vector2 scrollPos;

        private Camera _sceneCamera;

        private void OnGUI()
        {
            DrawEntity();
            DrawEntitySelector();
        }

        private void CreateSceneCamera()
        {
            //Create the scene camera, don't allow it to be saved or show up in the hierarchy
            _sceneCamera = new GameObject("SceneCamera").AddComponent<Camera>();
            _sceneCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;

        }

        private void DrawEntityCamera()
        {
            //If the current entity isn't null, point the camera at it and draw it using Handles.DrawCamera
            if (_selectedEntity != null)
            {
                _sceneCamera.transform.position = _selectedEntity.transform.position;
                _sceneCamera.transform.rotation = _selectedEntity.transform.rotation;
                Handles.DrawCamera(new Rect(0, 0, 200, 200), _sceneCamera);
            }

        }

        /// <summary>
        /// This draws any UnityEvents that are attached to the selected entity.
        /// </summary>
        private void DrawEntity()
        {
            //Return if the selected entity is null
            if (_selectedEntity != null)
            {
                //Vertical group
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                //Horizontal group
                EditorGUILayout.BeginHorizontal();
                //Draw the name of the entity in a label
                EditorGUILayout.LabelField("Selected Entity: " + _selectedEntity.name, EditorStyles.boldLabel);

                //Draw a button to lock the entity at the right of the window
                if (GUILayout.Button(_entityLocked ? "Unlock" : "Lock"))
                {
                    _entityLocked = !_entityLocked;
                }


                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("Entity Events");


                EditorGUI.BeginChangeCheck();
                //Iterate over the fields of the object only drawing the ones that are UnityEvents
                foreach (var field in _selectedEntity.GetType().GetFields())
                {
                    if (field.FieldType == typeof(UnityEvent))
                    {
                        _serializedObject.Update();
                        //Draw the unityevent and allow us to edit it
                        EditorGUILayout.PropertyField(_serializedObject.FindProperty(field.Name), true);

                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                    Debug.Log("Wahoo");
                    //Apply the changes to the serialized object
                    _serializedObject.ApplyModifiedProperties();
                    _serializedObject.UpdateIfRequiredOrScript();

                }


                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();



            }

        }

        public static void RepaintInspector(System.Type t)
        {
            UnityEditor.Editor[] ed = (UnityEditor.Editor[])Resources.FindObjectsOfTypeAll<UnityEditor.Editor>();
            for (int i = 0; i < ed.Length; i++)
            {
                if (ed[i].GetType() == t)
                {
                    ed[i].Repaint();
                    return;
                }
            }
        }

        private void DrawEntitySelector()
        {

            //Vertical group
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawEntityFilter();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //Use beginscrollview to draw a scrollable list of level entities
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));

            var filteredList = GetFilteredList();

            //Draw a list of all level entities
            for (int i = 0; i < filteredList.Count; i++)
            {
                var entity = filteredList[i];
                if (entity != null)
                {
                    //Get the name of the entity as EntityName (Type)
                    var name = entity.name + " (" + entity.GetType().Name + ")";

                    if (GUILayout.Button(name))
                    {
                        _levelEntityIndex = LevelEntities.IndexOf(entity);
                        //Select the entity in the scene
                        Selection.activeGameObject = entity.gameObject;
                        _selectedEntity = entity;
                        _serializedObject = new SerializedObject(_selectedEntity);
                        //Set the focus to the button
                        GUI.FocusControl(null);

                    }
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            //End vertical group
            EditorGUILayout.EndVertical();

        }

        private void DrawEntityFilter()
        {

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            //Draw a filtered selector for level entities

            _filterType = (FilterTypes)EditorGUILayout.EnumPopup(_filterType); ;

            //Draw a text field for filtering with the example text "Filter"
            _filterText = EditorGUILayout.TextField(_filterText);

            //End horizontal group
            EditorGUILayout.EndHorizontal();
        }

        private List<LevelEntity> GetFilteredList()
        {
            var filteredList = new List<LevelEntity>();

            //Iterate over all the level entities
            foreach (var entity in LevelEntities)
            {
                //Filter the list based on the filter type using regex to check if the name is similar to the filter text
                switch (_filterType)
                {
                    case FilterTypes.All:
                        if (entity.name.Contains(_filterText))
                        {
                            filteredList.Add(entity);
                        }
                        else if (entity.GetType().Name.Contains(_filterText))
                        {
                            filteredList.Add(entity);
                        }
                        break;
                    case FilterTypes.Type:
                        if (entity.GetType().Name.Contains(_filterText))
                        {
                            filteredList.Add(entity);
                        }
                        break;
                    case FilterTypes.Name:
                        if (entity.name.Contains(_filterText))
                        {
                            filteredList.Add(entity);
                        }
                        break;
                }
            }

            return filteredList;
        }


        //A method which calls ongui when the selection changes
        private void OnSelectionChange()
        {

        }

        //A method which calls when an object is created or destroyed
        private void OnHierarchyChange()
        {
            //Add every level entity to the list
            LevelEntities.Clear();
            foreach (var obj in FindObjectsOfType<LevelEntity>())
            {
                var entity = obj.GetComponent<LevelEntity>();
                if (entity != null)
                {
                    LevelEntities.Add(entity);
                }
            }
            //Reorder the list alphabetically
            LevelEntities.Sort((x, y) => x.name.CompareTo(y.name));

            Repaint();
        }


    }
}
