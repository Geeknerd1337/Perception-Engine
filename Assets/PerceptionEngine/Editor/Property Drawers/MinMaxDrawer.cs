using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Perception.Engine;

namespace Perception.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Only allow usage on Vector2s
            if (property.propertyType != SerializedPropertyType.Vector2)
            {
                EditorGUI.LabelField(position, label.text, "Use only with Vector2s");
                return;
            }

            //Get the attributes
            MinMaxSliderAttribute minMax = attribute as MinMaxSliderAttribute;

            Vector2 range = property.vector2Value;

            //Draw the label
            EditorGUI.LabelField(position, label);

            Rect oldPosition = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, label));

            //Get the width of the label
            oldPosition.width -= EditorGUIUtility.labelWidth;


            oldPosition.x += EditorGUIUtility.labelWidth - 10f;

            //Draw the slider
            EditorGUI.MinMaxSlider(oldPosition, ref range.x, ref range.y, minMax.Min, minMax.Max);

            //Draw a label for range.x at the minmaxslider's left slider handle position
            var leftLabelPosition = oldPosition;
            leftLabelPosition.x += oldPosition.width * (range.x - minMax.Min) / (minMax.Max - minMax.Min);
            leftLabelPosition.y += 15;
            //Get the width of the string
            var stringWidth = GUI.skin.label.CalcSize(new GUIContent(range.x.ToString("0.000"))).x;
            //Subtract half the width of the string to center it
            leftLabelPosition.x -= (float)stringWidth / 2f;


            //Draw the label field, center the text
            EditorGUI.LabelField(leftLabelPosition, range.x.ToString("0.000"));

            //Do the same for the right hand side
            var rightLabelPosition = oldPosition;
            rightLabelPosition.x += oldPosition.width * (range.y - minMax.Min) / (minMax.Max - minMax.Min);
            rightLabelPosition.y += 15;
            stringWidth = GUI.skin.label.CalcSize(new GUIContent(range.y.ToString("0.000"))).x;
            rightLabelPosition.x -= (float)stringWidth / 2f;
            EditorGUI.LabelField(rightLabelPosition, range.y.ToString("0.000"));

            //Set the value
            property.vector2Value = range;

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 15;
        }
    }
}
