using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Perception.Engine;
namespace Perception.Editor
{
    /// <summary>
    /// Checks for the OnEditorValueChanged attribute on a field and calls the method if it exists.
    /// </summary>
    public class OnEditorValueChangedHandler
    {
        /// <summary>
        /// Checks for the OnEditorValueChanged attribute on a field and calls the method if it exists.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="field">The field that has changed.</param>
        public static void CheckForOnEditorValueChanged(object target, FieldInfo field)
        {
            var onEditorValueChangedAttribute = field.GetCustomAttribute<OnEditorValueChangedAttribute>();
            if (onEditorValueChangedAttribute == null)
                return;

            var method = target.GetType().GetMethod(onEditorValueChangedAttribute.CallbackName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
            {
                Debug.LogError($"Method {onEditorValueChangedAttribute.CallbackName} not found on {target.GetType().Name}.");
                return;
            }

            method.Invoke(target, null);
        }

    }
}
