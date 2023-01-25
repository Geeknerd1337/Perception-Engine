using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Perception.Engine
{

    public class LDToolbox : EditorWindow
    {
        [MenuItem("Window/Leveldesign Toolbox")]
        public static void ShowWindow()
        {
            GetWindow<LDToolbox>("Leveldesign Toolbox");
        }

        private void OnGUI()
        {
            GUILayout.Label("Leveldesign Toolbox", EditorStyles.boldLabel);
            if (GUILayout.Button("Make Trigger"))
            {
            }
        }

    }
}
