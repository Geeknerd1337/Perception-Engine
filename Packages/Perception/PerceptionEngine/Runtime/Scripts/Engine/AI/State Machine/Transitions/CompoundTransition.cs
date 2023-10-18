using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine.AI
{
    /// <summary>
    /// A transition which evaluates multiple decisions instead of just one
    /// </summary>
    [CreateAssetMenu(menuName = "Perception/AI/Tansitions/Compound Transition")]
    public class CompoundTransition : Transition
    {
        public Decision[] Decisions;

        public override void Enter(BaseStateMachine stateMachine)
        {
            foreach (Decision decision in Decisions)
            {
                decision.Enter(stateMachine);
            }
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (Decide(stateMachine) && !(TrueState is RemainInState))
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



        public bool Decide(BaseStateMachine stateMachine)
        {
            foreach (Decision decision in Decisions)
            {
                if (!decision.Decide(stateMachine))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
