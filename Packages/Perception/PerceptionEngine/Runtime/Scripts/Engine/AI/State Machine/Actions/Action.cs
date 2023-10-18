using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine.AI
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute(BaseStateMachine stateMachine);
        public virtual void Enter(BaseStateMachine stateMachine) { }
    }
}
