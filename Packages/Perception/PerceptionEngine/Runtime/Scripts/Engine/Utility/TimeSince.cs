using System;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A really useful struct for keeping track of time since something happened. It's like a float but it's always relative to the current time.
    /// </summary>
    public struct TimeSince : IEquatable<TimeSince>, IEquatable<float>
    {
        private TimeSince(float time)
        {
            _time = Time.time - time;
        }

        public float Relative => this;

        public override string ToString()
        {
            return Relative.ToString();
        }

        private float _time;

        public static implicit operator float(TimeSince ts)
        {
            return Time.time - ts._time;
        }

        public static implicit operator TimeSince(float ts)
        {
            return new TimeSince(ts);
        }

        public override bool Equals(object obj)
        {
            if (obj is float value)
            {
                return value.Equals(Time.time - _time);
            }

            return obj is TimeSince other && Equals(other);
        }

        public bool Equals(TimeSince other)
        {
            return _time.Equals(other._time);
        }

        public bool Equals(float other)
        {
            return (Time.time - _time).Equals(other);
        }

        public override int GetHashCode()
        {
            return _time.GetHashCode();
        }
    }
}

