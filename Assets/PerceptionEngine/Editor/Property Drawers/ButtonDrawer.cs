using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Perception.Engine;
using System.Reflection;

namespace Perception.Editor
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Debug.Log("HMM");
            // Get the ButtonAttribute from the PropertyDrawer's attribute
            ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;

            string methodName = property.name;

            if (property.serializedObject.targetObject is MonoBehaviour)
            {
                MonoBehaviour mono = (MonoBehaviour)property.serializedObject.targetObject;
                MethodInfo method = mono.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (method != null)
                {
                    if (GUI.Button(position, label.text))
                    {
                        method.Invoke(mono, null);
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Method not found");
                }
            }
            else if (property.serializedObject.targetObject is ScriptableObject)
            {
                ScriptableObject scriptable = (ScriptableObject)property.serializedObject.targetObject;
                MethodInfo method = scriptable.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (method != null)
                {
                    if (GUI.Button(position, label.text))
                    {
                        method.Invoke(scriptable, null);
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Method not found");
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use only with MonoBehaviours or ScriptableObjects");
            }
        }

    }
}