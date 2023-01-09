using System;
using UnityEngine;
using UnityEditor;


namespace Perception.Engine
{

    /// <summary>
    /// Shows a field only if the value of the field specified in the inspector is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ShowIfAttribute : PropertyModifierAttribute
    {
        public readonly string ConditionalSourceField;
        private bool _enabled;


        public ShowIfAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
        }
#if UNITY_EDITOR

        public override float GetHeight(SerializedProperty property, GUIContent label, float height)
        {
            if (_enabled)
            {
                return base.GetHeight(property, label, height);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
        {
            if (!visible)
            {
                return false;
            }
            ShowIfAttribute showIf = this;
            _enabled = GetConditionalHideAttributeResult(showIf, property);
            return _enabled;

        }

        private bool GetConditionalHideAttributeResult(ShowIfAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Attempting to use a ShowIfAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }

            return enabled;
        }
#endif
    }
}