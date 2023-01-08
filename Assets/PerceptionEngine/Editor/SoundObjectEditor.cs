using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Perception.Engine;

namespace Perception.Engine.Editor
{
    [CustomEditor(typeof(SoundObject), true)]
    public class SoundObjectEditor : PerceptionEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SoundObject sound = (SoundObject)target;

            EditorGUILayout.Space(10f);
            if (GUILayout.Button("Play"))
            {
                Debug.Log(sound.Volume.x);
            }
        }
    }
}
