
using System.Reflection;
using UnityEditor;
using System.Collections.Generic;

namespace Perception.Editor
{
    /// <summary>
    /// This is the core class for how buttons get drawn in the PerceptionEditor. 
    /// This is partially based an MIT licensed repository I thought did this concept incredibly well, so I'm basing the architecture of this on that.
    /// You can see the code this is inspired by here:
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
    }
}