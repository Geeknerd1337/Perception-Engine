using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Perception.Engine
{
    public class PerceptionStyles
    {
        public static GUIStyle BoxGroupLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },


        };

        public static GUIStyle BoxGroupStyle = new GUIStyle(EditorStyles.helpBox)
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },

            //Draw a background
            margin = new RectOffset(1, 1, 1, 1),
            padding = new RectOffset(20, 20, 10, 10),

        };

        public static GUIStyle BoxGroupHeaderStyle = new GUIStyle(EditorStyles.helpBox)
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },

            //Draw a background
            margin = new RectOffset(0, 0, 10, -1),
            padding = new RectOffset(20, 2, 4, 4),

        };

        public static GUIStyle BoxGroupFoldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            alignment = TextAnchor.MiddleLeft,
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white,

            },
            //Draw an outline
            border = new RectOffset(1, 1, 1, 1),
            //Draw a background
            margin = new RectOffset(1, 1, 1, 1),

        };
    }
}
