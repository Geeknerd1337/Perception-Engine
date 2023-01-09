using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Perception.Engine
{
    /// <summary>
    /// Used on Scriptable Objects to organize properties into tabs in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TabAttribute : Attribute
    {
        public readonly string Name;
        public TabAttribute(string n)
        {
            Name = n;
        }
    }

    /// <summary>
    /// Used for Vector2s to show a min max slider in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float Min;
        public float Max;


        public MinMaxSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    public enum ButtonMode
    {
        AlwaysEnabled,
        EnabledInPlayMode,
        DisabledInPlayMode
    }

    [Flags]
    public enum ButtonSpacing
    {
        None = 0,
        Before = 1,
        After = 2
    }


    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string Name;

        public ButtonAttribute() { }

        public ButtonAttribute(string n)
        {
            Name = n;
        }

        /// <summary>
        /// The enable behavior of the button. Always enabled by default.
        /// </summary>
        public ButtonMode Mode { get; set; } = ButtonMode.AlwaysEnabled;

        /// <summary>
        /// Whether or not the button has some space before or after it.
        /// </summary>
        public ButtonSpacing Spacing { get; set; } = ButtonSpacing.None;

        /// <summary>
        /// Whether or not to expand the paramters foldout by default.
        /// </summary>
        public bool Expanded { get; set; }
    }


    /// <summary>
    /// Shows a field only if the value of the field specified in the inspector is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string ConditionalSourceField;


        public ShowIfAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
        }
    }

    /// <summary>
    /// Hides a field if the value of the field specified in the inspector is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class HideIfAttribute : PropertyAttribute
    {
        public readonly string ConditionalSourceField;
        public HideIfAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
        }
    }

    /// <summary>
    /// An attribute you apply to call a method on a class when the value changes in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OnEditorValueChangedAttribute : PropertyAttribute
    {
        public readonly string CallbackName;

        public OnEditorValueChangedAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }
    }

    #region Modifiable Attributes
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public abstract class PropertyModifierAttribute : Attribute
    {

        public int order { get; set; }

#if UNITY_EDITOR
        public virtual float GetHeight(SerializedProperty property, GUIContent label, float height)
        {
            return height;
        }

        public virtual bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible) { return true; }
        public virtual void AfterGUI(Rect position, SerializedProperty property, GUIContent label) { }
#endif
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ModifiablePropertyAttribute : PropertyAttribute
    {
        public List<PropertyModifierAttribute> modifiers = null;

#if UNITY_EDITOR
        public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        public virtual float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
#endif
    }

    #endregion

}