using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A controller is a component that is attached to an actor and is used to control the actor. This is the base class for all controllers.
    /// </summary>
    public class Controller : MonoBehaviour
    {
        /// <summary>
		/// The current velocity of this controller
		/// </summary>
		public Vector3 Velocity;
        public Quaternion EyeRot;
        public Vector3 EyePos;

        public virtual void Disable()
        {

        }

        public virtual void Awake()
        {

        }

        public virtual void BuildInput()
        {

        }
    }
}
