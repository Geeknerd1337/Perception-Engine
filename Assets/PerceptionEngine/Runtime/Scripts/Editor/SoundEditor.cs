using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Netscape.Engine;

namespace Netscape.Editor
{
    [CustomEditor(typeof(Sound), true)]
    public class SoundEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Sound sound = (Sound)target;
            if (GUILayout.Button("Play"))
            {

            }
        }
    }
}
