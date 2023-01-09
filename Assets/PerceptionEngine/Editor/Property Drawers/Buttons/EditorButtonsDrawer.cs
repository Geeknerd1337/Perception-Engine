
using System.Collections.Generic;
using System.Reflection;
using Perception.Engine;
namespace Perception.Editor
{


    /// <summary>
    /// Helper class that can be used in custom Editors to draw methods marked with the <see cref="ButtonAttribute"/> as buttons.
    /// </summary>
    public class ButtonsDrawer
    {
        /// <summary>
        /// A list of buttons that can be drawn for the class.
        /// </summary>
        public readonly List<EditorButton> Buttons = new List<EditorButton>();

        /// <summary>
        /// Initializes a new instance of the EditorButtonDrawer class and fills buttons with
        /// methods marked with the ButtonAttribute. Recommended to instantiate it in OnEnable to improve
        /// performance of the custom editor.
        /// </summary>
        /// <param name="target">Editor's target.</param>
        public ButtonsDrawer(object target)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var methods = target.GetType().GetMethods(flags);

            foreach (MethodInfo method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute == null)
                    continue;

                Buttons.Add(EditorButton.Create(method, buttonAttribute));
            }
        }

        /// <summary>
        /// Draws all the methods marked with button attributes
        /// </summary>
        public void DrawButtons(object[] targets)
        {
            foreach (EditorButton button in Buttons)
            {
                button.Draw(targets);
            }
        }
    }
}