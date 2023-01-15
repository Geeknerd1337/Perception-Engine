using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A class of useful float extensions like Remap and LerpTo
    /// </summary>
    public static class FloatExtensions
    {
        public static float Remap(float input, float inputMin, float inputMax, float min, float max)
        {
            return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
        }

        public static float LerpTo(this float input, float b, float t)
        {
            return Mathf.Lerp(input, b, t);
        }

        public static float Approach(this float f, float target, float delta)
        {
            if (f > target)
            {
                f -= delta;
                if (f < target)
                {
                    return target;
                }
            }
            else
            {
                f += delta;
                if (f > target)
                {
                    return target;
                }
            }

            return f;
        }
    }
}
