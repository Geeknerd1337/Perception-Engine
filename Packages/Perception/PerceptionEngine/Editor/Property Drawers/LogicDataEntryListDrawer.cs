using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Perception.Engine;

namespace Perception.Editor
{



    [CustomPropertyDrawer(typeof(LogicDataEntry))]
    public class LogicDataEntryDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Draw the Key, Type, and appropriate value field
            EditorGUI.BeginProperty(position, label, property);

            //Draw the name of the variable

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;

            // Calculate rects with 5 pixel spacing between each field
            var keyLabel = new Rect(position.x, position.y, 30, position.height);
            var keyRect = new Rect(position.x + 35, position.y, 100, position.height);
            var typeLabel = new Rect(position.x + 140, position.y, 30, position.height);
            var typeRect = new Rect(position.x + 175, position.y, 100, position.height);
            var valueRect = new Rect(position.x + 280, position.y, position.width - 280, position.height);


            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.LabelField(keyLabel, "Key");
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("Key"), GUIContent.none);

            EditorGUI.LabelField(typeLabel, "Type");
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("Type"), GUIContent.none);

            EditorGUI.indentLevel = 0;

            var type = (LogicDataEntryType)property.FindPropertyRelative("Type").enumValueIndex;

            switch (type)
            {
                case LogicDataEntryType.String:
                    EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("StringValue"), GUIContent.none);
                    break;
                case LogicDataEntryType.Int:
                    EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("IntValue"), GUIContent.none);
                    break;
                case LogicDataEntryType.Float:
                    EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("FloatValue"), GUIContent.none);
                    break;
                case LogicDataEntryType.Bool:
                    EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("BoolValue"), GUIContent.none);
                    break;
                case LogicDataEntryType.Vector3:
                    EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Vector3Value"), GUIContent.none);
                    break;
                default:
                    break;
            }

            // Set indent back to what it was

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

        }



    }
}
