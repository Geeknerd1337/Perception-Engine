
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;
using Perception.Engine;

namespace Perception.Editor
{
    /// <summary>
    /// This is the core class for how buttons get drawn in the PerceptionEditor. 
    /// Found a repository that did basic editor buttons very well so some concepts and architecture have been borrowed from that.
    /// You can see that repository here (MIT License):
    /// https://github.com/madsbangh/EasyButtons
    /// </summary>
    public abstract class Button
    {
        /// <summary>
        /// The display name of the button.
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// The method to be invoked when the button is pressed.
        /// </summary>
        public readonly MethodInfo Method;

        protected Button(MethodInfo method, ButtonAttribute attribute)
        {
            Method = method;

            //If we don't provide a name just name it the name of the method.
            DisplayName = string.IsNullOrEmpty(attribute.Name)
                ? ObjectNames.NicifyVariableName(method.Name)
                : attribute.Name;

            bool inAppropriateMode = EditorApplication.isPlaying
                ? attribute.Mode == ButtonMode.EnabledInPlayMode
                : attribute.Mode == ButtonMode.DisabledInPlayMode;
        }
    }
}