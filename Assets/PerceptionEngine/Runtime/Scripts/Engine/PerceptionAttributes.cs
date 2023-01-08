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
}