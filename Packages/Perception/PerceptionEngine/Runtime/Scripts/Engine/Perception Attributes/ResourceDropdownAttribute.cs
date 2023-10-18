using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Linq;

namespace Perception.Engine
{
    /// <summary>
    /// A class which takes a resource type and only works on strings. It will display a dropdown of all the names resources of that type and set the string to the name of the resource.
    /// </summary>
    public class ResourceDropdownAttribute : ModifiablePropertyAttribute
    {
        public ResourceType ResourceType;

        public ResourceDropdownAttribute(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //If the property is not a string
            if (property.propertyType != SerializedPropertyType.String)
            {
                //Display an error message
                EditorGUI.LabelField(position, label.text, "Use only with strings");
                return;
            }



            //Get the names of all the resources of the given type
            List<string> resourceNames = Resources.LoadAll(AssetService.ResourcePaths[ResourceType])
            //Select the name of each resource
            .Select(r => r.name).ToList();

            //Alphabetize the list
            resourceNames.Sort();

            //Add "Custom" to the list of names
            resourceNames.Add("Custom");

            //Get the index of the current resource
            int index = Array.IndexOf(resourceNames.ToArray(), property.stringValue);

            //If the index is -1 (not found) then set it to the last index (custom), log an error about a potentially delted surface
            if (index == -1)
            {
                index = resourceNames.Count - 1;
                Debug.LogError("The resource " + property.stringValue + " was not found. It may have been deleted.");
            }

            //Draw the popup
            index = EditorGUI.Popup(position, label.text, index, resourceNames.ToArray());


            //If it is not custom
            if (index != resourceNames.Count - 1)
            {
                //Set the property to the name of the resource
                property.stringValue = resourceNames[index];
            }
            else
            {
                //Otherwise make it custom
                property.stringValue = "Custom";
            }


        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
#endif

    }
}
