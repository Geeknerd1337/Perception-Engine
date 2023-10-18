using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine.AI
{
    /// <summary>
    /// Generalized agent that can be used for other agent types like swimming or flying agents.
    /// </summary>
    public class PerceptionAgent : MonoBehaviour
    {
        public virtual void Awake()
        {

        }

        public virtual void SetDestination(Vector3 position)
        {

        }

        public virtual float RemainingDistance()
        {
            return 0f;
        }

        public virtual float StoppingDistance()
        {
            return 0f;
        }

        public virtual Vector3 DesiredVelocity()
        {
            return Vector3.zero;
        }

        public virtual void Disable()
        {

        }
    }
}
