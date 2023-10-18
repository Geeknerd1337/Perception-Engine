using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace Perception.Engine
{
    /// <summary>
    /// Used for Vector2s to show a min max slider in the inspector.
    /// </summary>

    public class MinMaxSliderAttribute : ModifiablePropertyAttribute
    {
        public float Min;
        public float Max;


        public MinMaxSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Draw the float slider if it is a Vector2 and the IntSlider if it is a Vector2Int
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                DrawFloatSlider(position, property, label);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                DrawIntSlider(position, property, label);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use only with Vector2s and Vector2Ints");
                return;
            }

        }
        public void DrawIntSlider(Rect position, SerializedProperty property, GUIContent label)
        {
            Vector2Int intRange = property.vector2IntValue;
            Vector2 range = new Vector2(intRange.x, intRange.y);

            //Draw the label
            EditorGUI.LabelField(position, label);

            Rect oldPosition = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, label));

            //Get the width of the label
            oldPosition.width -= EditorGUIUtility.labelWidth;


            oldPosition.x += EditorGUIUtility.labelWidth - 10f;

            //Draw the slider
            EditorGUI.MinMaxSlider(oldPosition, ref range.x, ref range.y, Min, Max);

            //Draw a label for range.x at the minmaxslider's left slider handle position
            var leftLabelPosition = oldPosition;
            leftLabelPosition.x += oldPosition.width * (range.x - Min) / (Max - Min);
            leftLabelPosition.y += 15;
            //Get the width of the string
            var stringWidth = GUI.skin.label.CalcSize(new GUIContent(range.x.ToString("0"))).x;
            //Subtract half the width of the string to center it
            leftLabelPosition.x -= (float)stringWidth / 2f;


            //Draw the label field, center the text
            EditorGUI.LabelField(leftLabelPosition, range.x.ToString("0"));

            //Do the same for the right hand side
            var rightLabelPosition = oldPosition;
            rightLabelPosition.x += oldPosition.width * (range.y - Min) / (Max - Min);
            rightLabelPosition.y += 15;
            stringWidth = GUI.skin.label.CalcSize(new GUIContent(range.y.ToString("0"))).x;
            rightLabelPosition.x -= (float)stringWidth / 2f;
            EditorGUI.LabelField(rightLabelPosition, range.y.ToString("0"));

            //Round the range x and y
            range.x = Mathf.Round(range.x);
            range.y = Mathf.Round(range.y);

            //Set the Vector2 int value
            property.vector2IntValue = new Vector2Int((int)range.x, (int)range.y);


        }

        public void DrawFloatSlider(Rect position, SerializedProperty property, GUIContent label)
        {


            Vector2 range = property.vector2Value;

            //Draw the label
            EditorGUI.LabelField(position, label);

            Rect oldPosition = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, label));

            //Get the width of the label
            oldPosition.width -= EditorGUIUtility.labelWidth;


            oldPosition.x += EditorGUIUtility.labelWidth - 10f;

            //Draw a text field for the min value
            var minPosition = oldPosition;
            minPosition.y += 15;
            //Subtract half the width of the float field to center it


            minPosition.width = 35;
            range.x = EditorGUI.FloatField(minPosition, range.x);


            //Draw the slider
            EditorGUI.MinMaxSlider(oldPosition, ref range.x, ref range.y, Min, Max);

            //Draw a text field for the max value
            var maxPosition = oldPosition;
            maxPosition.y += 15;
            //Subtract half the width of the float field to center it
            maxPosition.x -= 35;
            maxPosition.width = 35;
            maxPosition.x += oldPosition.width;
            range.y = EditorGUI.FloatField(maxPosition, range.y);

            //Draw a label for range.x at the minmaxslider's left slider handle position
            var leftLabelPosition = oldPosition;
            leftLabelPosition.x += oldPosition.width * (range.x - Min) / (Max - Min);
            leftLabelPosition.y += 15;

            //Get the width of the string
            var stringWidth = GUI.skin.label.CalcSize(new GUIContent(range.x.ToString("0.000"))).x;
            //Subtract half the width of the string to center it
            leftLabelPosition.x -= (float)stringWidth / 2f;


            //Do the same for the right hand side
            var rightLabelPosition = oldPosition;
            rightLabelPosition.x += oldPosition.width * (range.y - Min) / (Max - Min);
            rightLabelPosition.y += 15;
            stringWidth = GUI.skin.label.CalcSize(new GUIContent(range.y.ToString("0.000"))).x;
            rightLabelPosition.x -= (float)stringWidth / 2f;

            //Add 15 to the leftLabelPositionY if the two labels overlap
            if (leftLabelPosition.x + stringWidth > rightLabelPosition.x)
            {
                rightLabelPosition.y += 15;
            }
            //A single Boolean for if either label overlap either of the edges of the slider + 35
            bool leftLabelOverlaps = leftLabelPosition.x + stringWidth > oldPosition.x + oldPosition.width - 35 || leftLabelPosition.x < oldPosition.x + 35;
            bool rightLabelOverlaps = rightLabelPosition.x + stringWidth > oldPosition.x + oldPosition.width - 35 || rightLabelPosition.x < oldPosition.x + 35;


            if (!leftLabelOverlaps)
            {
                EditorGUI.LabelField(leftLabelPosition, range.x.ToString("0.000"));
            }

            if (!rightLabelOverlaps)
            {
                EditorGUI.LabelField(rightLabelPosition, range.y.ToString("0.000"));
            }

            //Set the value
            property.vector2Value = range;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 30;
        }
#endif
    }
}
