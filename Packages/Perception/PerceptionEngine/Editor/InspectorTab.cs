using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Perception.Engine;
using System.Reflection;
using System.Linq;
using System;

namespace Perception.Editor
{
    public class InspectorTab

    {
        public string Name;
        public List<SerializedProperty> Fields = new List<SerializedProperty>();

        public InspectorTab()
        {
            Name = "";
        }

        public InspectorTab(string s)
        {
            Name = s;
        }
    }
}