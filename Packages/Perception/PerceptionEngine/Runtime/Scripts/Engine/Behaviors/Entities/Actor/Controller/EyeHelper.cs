using UnityEngine;
using Unity;

namespace Perception.Engine
{
    /// <summary>
    /// A struct to help with the managing of Eye rotation
    /// </summary>
    public struct EyeHelper
    {
        public float rotX;
        public float rotY;
        /// <summary>
        /// A function which takes in a delta and adds it to the rotX and rotY variables, clamping the 
        /// rotY variable so you can't look behind you.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Rotate(float x, float y)
        {
            //Reset these values to 0 since we're using local rotation in the controller
            //This is important for moving platforms to work.
            rotX = 0;
            rotX += x;
            rotY += y;
            rotY = Mathf.Clamp(rotY, -90f, 90f);
        }
    }
}