using System.Reflection;
using UnityEngine;
using Perception.Engine;
using System.Collections;
using System.Collections.Generic;

namespace Perception.Editor
{

    internal class EditorButtonWithoutParams : EditorButton
    {
        public EditorButtonWithoutParams(MethodInfo method, ButtonAttribute buttonAttribute)
            : base(method, buttonAttribute) { }

        protected override void DrawInternal(IEnumerable<object> targets)
        {
            if (!GUILayout.Button(DisplayName))
                return;

            foreach (object obj in targets)
            {
                Method.Invoke(obj, null);
            }
        }
    }
}