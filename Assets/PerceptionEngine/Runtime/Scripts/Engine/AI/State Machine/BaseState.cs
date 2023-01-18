using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine.AI
{
    public class BaseState : ScriptableObject
    {
        public virtual void Execute(BaseStateMachine machine) { }

        public virtual void Enter(BaseStateMachine machine) { }

        /// <summary>
        /// How many seconds it takes per execution of this state.
        /// Some states do not need to run every frame for performance reasons.
        /// </summary>
        public float ThinkRate = 0f;
    }
}
