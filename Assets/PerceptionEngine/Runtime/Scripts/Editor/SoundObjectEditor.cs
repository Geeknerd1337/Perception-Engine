using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Netscape.Engine;

namespace Perception.Editor
{
    [CustomEditor(typeof(SoundObject), true)]
    public class SoundObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SoundObject sound = (SoundObject)target;
            if (GUILayout.Button("Play"))
            {

            }
        }
    }
}
