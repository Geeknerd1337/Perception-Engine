using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using System.Reflection;
using Perception.Engine;
using System;
using System.Linq;

namespace Perception.Editor
{


    public class LDToolbox : EditorWindow
    {
        [MenuItem("Window/Leveldesign Toolbox")]
        public static void ShowWindow()
        {
            GetWindow<LDToolbox>("Leveldesign Toolbox");
        }

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
            SceneView.duringSceneGui += OnSceneGUI;
            SceneView.beforeSceneGui += BeforeSceneGUI;

            GetAllLevelEntityTypes();


        }

        //On destroy
        private void OnDisable()
        {
            //Destroy the scene camera
            DestroyImmediate(_sceneCamera.gameObject);

            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.beforeSceneGui -= BeforeSceneGUI;
        }

        public List<LevelEntity> LevelEntities = new List<LevelEntity>();
        private LevelEntity _selectedEntity;
        private int _levelEntityIndex = 0;
        private string _filterText = "";
        private bool _entityLocked = false;
        private bool _receivedClickDownEvent = false;
        private bool _receivedClickUpEvent = false;
        private bool _shiftDownEvent = false;

        private bool _shiftUpEvent = false;

        private bool _hasPlaceableEntity = false;
        private SerializedObject _serializedObject;
        private List<System.Type> _types = new List<System.Type>();
        private int _typeIndex = 0;
        public static bool DrawGizmos;

        private enum FilterTypes
        {
            All,
            Type,
            Name
        }

        private enum SortTypes
        {
            Alphabetical,
            Distance
        }

        private FilterTypes _filterType = FilterTypes.All;
        private SortTypes _sortType = SortTypes.Alphabetical;

        Vector2 scrollPos;

        private Camera _sceneCamera;

        private void OnGUI()
        {
            DrawConfig();
            DrawToolbox();
            DrawEntitySelector();
            DrawEntity();
            DrawEntitySearch();
        }


        //On scene gui
        private void OnSceneGUI(SceneView sceneView)
        {
            if (_shiftDownEvent)
            {
                Handles.color = Color.red;
                Handles.DrawWireDisc(GetCurrentMousePositionInScene(), Vector3.up, 0.5f);
                Handles.color = Color.white;
                sceneView.Repaint();

                if (_receivedClickUpEvent)
                {
                    //Instantiate a new object of the selected type at the mouse position with an undo event
                    GameObject newObject = new GameObject();
                    newObject.transform.position = GetCurrentMousePositionInScene();
                    newObject.AddComponent(_types[_typeIndex]);
                    newObject.name = _types[_typeIndex].Name;
                    Undo.RegisterCreatedObjectUndo(newObject, "Created new object");
                    var ent = newObject.GetComponent(_types[_typeIndex]) as LevelEntity;
                    //Set the ints ID to a new GUID
                    ent.ID = Guid.NewGuid().ToString();
                    ent.DrawGizmos = DrawGizmos;

                    //Set the selected entity to the new object
                    Selection.activeGameObject = newObject.gameObject;
                    if (!_entityLocked)
                    {
                        _selectedEntity = ent;

                        _serializedObject = new SerializedObject(_selectedEntity);
                        _serializedObject.Update();
                    }


                    _receivedClickUpEvent = false;
                }
            }

        }

        void BeforeSceneGUI(SceneView sceneView)
        {


            if (!_hasPlaceableEntity)
            {
                _receivedClickDownEvent = false;
                _receivedClickUpEvent = false;
                _shiftDownEvent = false;
                _shiftUpEvent = false;
            }
            else
            {
                if (_shiftDownEvent)
                {
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        _receivedClickDownEvent = true;
                        Event.current.Use();
                    }

                    if (_receivedClickDownEvent && Event.current.type == EventType.MouseUp && Event.current.button == 0)
                    {
                        _receivedClickDownEvent = false;
                        _receivedClickUpEvent = true;
                        Event.current.Use();
                    }
                }

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftShift)
                {
                    _shiftDownEvent = true;


                }

                if (_shiftDownEvent && Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftShift)
                {
                    _shiftDownEvent = false;
                    _shiftUpEvent = true;

                }
            }
        }

        Vector3 GetCurrentMousePositionInScene()
        {
            Vector3 mousePosition = Event.current.mousePosition;
            var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
            return placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
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
        /// Draws the portion of the window where you can select from the available entities and spawn them
        /// </summary>
        private void DrawEntitySelector()
        {
            //Vertical group
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //Entity placement label
            EditorGUILayout.LabelField("Entity Placement", EditorStyles.boldLabel);
            //Draw a drop down of all the types in the list that aren't null, if the type is null, draw the word "None"
            _typeIndex = EditorGUILayout.Popup("Type", _typeIndex, _types.Select(x => x == null ? "None" : x.Name).ToArray());
            _hasPlaceableEntity = _typeIndex != 0;



            EditorGUILayout.EndVertical();

        }

        public void DrawToolbox()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Toolbox", EditorStyles.boldLabel);

            //Get the currently selected gameobject
            var selected = Selection.activeGameObject;

            if (selected != null && selected.GetComponent<LogicTrigger>() == null)
            {
                if (GUILayout.Button("Convert to Logic trigger"))
                {
                    LDToolboxHelper.ConvertToTrigger(selected);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawConfig()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Config", EditorStyles.boldLabel);
            bool old = DrawGizmos;
            DrawGizmos = EditorGUILayout.Toggle("Draw Gizmos", DrawGizmos);

            if (old != DrawGizmos)
            {
                foreach (var entity in FindObjectsOfType<LevelEntity>())
                {
                    entity.DrawGizmos = DrawGizmos;
                }
                SceneView.RepaintAll();
            }

            EditorGUILayout.EndVertical();
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

                //Draw a textfield for editing the selected entities name
                _selectedEntity.name = EditorGUILayout.TextField("Name", _selectedEntity.name);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("Entity Events");

                _serializedObject.Update();

                EditorGUI.BeginChangeCheck();
                //Iterate over the fields of the object only drawing the ones that are UnityEvents
                foreach (var field in _selectedEntity.GetType().GetFields())
                {
                    if (field.FieldType == typeof(UnityEvent))
                    {

                        EditorGUILayout.PropertyField(_serializedObject.FindProperty(field.Name), true);
                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                    _serializedObject.ApplyModifiedProperties();

                    //Find the perception editor and call SetSoTarget
                    PerceptionEditor.Instance.SetSoTarget(_serializedObject);
                }



                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();



            }

        }

        /// <summary>
        /// Draws the entity search window where you can search for levelEntities and select them
        /// </summary>
        private void DrawEntitySearch()
        {

            //Vertical group
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DrawEntitySort();
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
                        if (!_entityLocked)
                        {
                            _selectedEntity = entity;
                            _serializedObject = new SerializedObject(_selectedEntity);
                            _serializedObject.Update();
                            //Focus the selected entity

                        }
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

        private void DrawEntitySort()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Sort", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _sortType = (SortTypes)EditorGUILayout.EnumPopup(_sortType);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private List<LevelEntity> GetFilteredList()
        {
            var filteredList = new List<LevelEntity>();

            //Iterate over all the level entities
            foreach (var entity in LevelEntities)
            {
                if (entity == null)
                    continue;
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

            //Sort the list
            switch (_sortType)
            {
                case SortTypes.Alphabetical:
                    filteredList.Sort((x, y) => x.name.CompareTo(y.name));
                    break;
                case SortTypes.Distance:
                    //Sort by distance to scene camera
                    filteredList.Sort((x, y) => Vector3.Distance(x.transform.position, SceneView.lastActiveSceneView.camera.transform.position).CompareTo(Vector3.Distance(y.transform.position, SceneView.lastActiveSceneView.camera.transform.position)));
                    break;
            }

            return filteredList;
        }



        private void OnSelectionChange()
        {

        }

        private void GetAllLevelEntityTypes()
        {
            if (_types == null)
            {
                _types = new List<Type>();
            }
            //Populate the list of level entity types
            _types.Clear();

            //Add an empty type to the list
            _types.Add(null);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(LevelEntity)))
                    {
                        _types.Add(type);
                    }
                    //Or if the type is a level entity
                    else if (type == typeof(LevelEntity))
                    {
                        _types.Add(type);
                    }
                }
            }


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

                    if (entity.ID == null || entity.ID == "")
                    {
                        entity.ID = Guid.NewGuid().ToString();
                    }
                }

            }
            //Reorder the list alphabetically
            LevelEntities.Sort((x, y) => x.name.CompareTo(y.name));

            Repaint();
        }


    }
}
