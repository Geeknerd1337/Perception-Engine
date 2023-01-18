using UnityEngine;

namespace Perception.Engine.AI
{
    /// <summary>
    /// A scriptable object representing a decision that can be made by a state machine.
    /// </summary>
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine state);
        public virtual void Enter(BaseStateMachine state) { }
    }
}
