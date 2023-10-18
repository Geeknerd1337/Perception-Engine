using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine.AI
{
    [CreateAssetMenu(menuName = "Perception/AI/Tansitions/Transition")]
    public class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public virtual void Execute(BaseStateMachine stateMachine)
        {
            if (Decision.Decide(stateMachine) && !(TrueState is RemainInState))
            {
                stateMachine.CurrentState = TrueState;
                TrueState.Enter(stateMachine);
                stateMachine.InvokeStateEntered(TrueState);
                stateMachine.TimeInState = 0f;
            }
            else if (!(FalseState is RemainInState))
            {
                stateMachine.CurrentState = FalseState;
                FalseState.Enter(stateMachine);
                stateMachine.InvokeStateEntered(FalseState);
                stateMachine.TimeInState = 0f;
            }
        }

        public virtual void Enter(BaseStateMachine stateMachine)
        {
            Decision.Enter(stateMachine);
        }
    }
}
