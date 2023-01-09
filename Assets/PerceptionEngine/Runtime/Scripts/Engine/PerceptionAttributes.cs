using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    //On Value Changed Attribute
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OnEditorValueChangedAttribute : PropertyAttribute
    {
        public readonly string CallbackName;

        public OnEditorValueChangedAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }
    }

}