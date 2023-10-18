using System.Linq;
using UnityEditor;
using UnityEngine;
using Perception.Engine;

namespace Perception.Editor
{
    /// <summary>
    /// Draws properties with the ModifiableProperty Attribute  calling the three methods of PropertyModifierAttribute.
    /// This way you can effectively stack multiple property attributes on a single property.
    /// </summary>
    [CustomPropertyDrawer(typeof(ModifiablePropertyAttribute), true)]
    public class ModifiablePropertyDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var modifiable = (ModifiablePropertyAttribute)attribute;
            if (modifiable.modifiers == null)
                modifiable.modifiers = fieldInfo.GetCustomAttributes(typeof(PropertyModifierAttribute), false)
                .Cast<PropertyModifierAttribute>().OrderBy(s => s.order).ToList();

            float height = ((ModifiablePropertyAttribute)attribute).GetPropertyHeight(property, label);
            foreach (var attr in modifiable.modifiers)
                height = attr.GetHeight(property, label, height);
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var modifiable = (ModifiablePropertyAttribute)attribute;

            bool visible = true;
            foreach (var attr in modifiable.modifiers.AsEnumerable().Reverse())
            {
                visible = attr.BeforeGUI(ref position, property, label, visible);

            }

            if (visible)
                modifiable.OnGUI(position, property, label);

            foreach (var attr in modifiable.modifiers)
                attr.AfterGUI(position, property, label);
        }
    }
}