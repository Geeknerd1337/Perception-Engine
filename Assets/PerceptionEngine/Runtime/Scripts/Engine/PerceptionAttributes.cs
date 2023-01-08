using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Perception.Engine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TabAttribute : Attribute
    {
        public readonly string Name;
        public TabAttribute(string n)
        {
            Name = n;
        }
    }

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
}