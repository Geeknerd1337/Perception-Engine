using System.Reflection;
using UnityEditor;
using System.Collections.Generic;
using Perception.Engine;
using Perception.Editor.Utility;

namespace Perception.Editor
{
    /// <summary>
    /// This is the core class for how buttons get drawn in the PerceptionEditor. 
    /// Found a repository that did basic editor buttons very well so some concepts and architecture have been borrowed from that.
    /// You can see that repository here (MIT License):
    /// https://github.com/madsbangh/EasyButtons
    /// </summary>
    public abstract class EditorButton
    {
        /// <summary>
        /// The display name of the button.
        /// </summary>
        public readonly string DisplayName;

        /// <summary>
        /// The method to be invoked when the button is pressed.
        /// </summary>
        public readonly MethodInfo Method;

        private readonly bool _disabled;
        private readonly ButtonSpacing _spacing;

        protected EditorButton(MethodInfo method, ButtonAttribute attribute)
        {
            Method = method;

            //If we don't provide a name just name it the name of the method.
            DisplayName = string.IsNullOrEmpty(attribute.Name)
                ? ObjectNames.NicifyVariableName(method.Name)
                : attribute.Name;

            bool inAppropriateMode = EditorApplication.isPlaying
                ? attribute.Mode == ButtonMode.EnabledInPlayMode
                : attribute.Mode == ButtonMode.DisabledInPlayMode;

            _spacing = attribute.Spacing;

            _disabled = !(attribute.Mode == ButtonMode.AlwaysEnabled) || inAppropriateMode;
        }

        internal static EditorButton Create(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            var parameters = method.GetParameters();

            if (parameters.Length == 0)
            {
                return new EditorButtonWithoutParams(method, buttonAttribute);
            }
            else
            {
                return new EditorButtonWithParams(method, buttonAttribute, parameters);
            }
        }

        public void Draw(IEnumerable<object> targets)
        {
            using (new EditorGUI.DisabledScope(_disabled))
            {
                using (new EditorDrawUtility.VerticalIndent(
                    _spacing.HasFlag(ButtonSpacing.Before),
                    _spacing.HasFlag(ButtonSpacing.After)))
                {
                    DrawInternal(targets);
                }
            }
        }



        protected abstract void DrawInternal(IEnumerable<object> targets);
    }
}