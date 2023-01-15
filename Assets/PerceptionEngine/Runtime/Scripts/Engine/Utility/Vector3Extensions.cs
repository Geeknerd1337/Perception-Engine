using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public static class Vector3Extensions
    {
        // With

        public static Vector3 WithX(this Vector3 vector, float value)
        {
            return new Vector3(value, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float value)
        {
            return new Vector3(vector.x, value, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float value)
        {
            return new Vector3(vector.x, vector.y, value);
        }

        // Flip

        public static Vector3 FlipX(this Vector3 v)
        {
            return new Vector3(-v.x, v.y, v.z);
        }

        public static Vector3 FlipY(this Vector3 v)
        {
            return new Vector3(v.x, -v.y, v.z);
        }

        public static Vector3 FlipZ(this Vector3 v)
        {
            return new Vector3(v.x, v.y, -v.z);
        }

        public static Vector3 Approach(this Vector3 v, float length, float amount)
        {
            return v.normalized * length.Approach(length, amount);
        }

        // Lerp

        public static Vector3 LerpTo(this Vector3 input, Vector3 b, float t)
        {
            return Vector3.Lerp(input, b, t);
        }

        public static Vector3 SlerpTo(this Vector3 input, Vector3 b, float t)
        {
            return Vector3.Slerp(input, b, t);
        }

        public static Vector3 ProjectileCalculateLead(Vector3 startPosition, Vector3 targetPosition, Vector3 targetVelocity, float velocity)
        {
            float distance = Vector3.Distance(startPosition, targetPosition);
            float travelTime = distance / (velocity);
            return (targetPosition + targetVelocity * travelTime);
        }

        public static float Dot(this Vector3 input, Vector3 b)
        {
            return Vector3.Dot(input, b);
        }
    }
}