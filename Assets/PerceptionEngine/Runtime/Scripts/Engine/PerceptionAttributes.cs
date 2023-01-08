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


    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public readonly string Name;
        public ButtonAttribute(string n)
        {
            Name = n;
        }
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

}