using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine.AI
{
    [CreateAssetMenu(menuName = "Perception/AI/States/State")]
    public class State : BaseState
    {
        public List<Action> Actions = new List<Action>();
        public List<Transition> Transitions = new List<Transition>();

        public override void Enter(BaseStateMachine machine)
        {
            base.Enter(machine);

            foreach (Transition transition in Transitions)
            {
                transition.Enter(machine);
            }

            foreach (Action action in Actions)
            {
                action.Enter(machine);
            }
        }

        public override void Execute(BaseStateMachine machine)
        {
            foreach (var action in Actions)
                action.Execute(machine);

            foreach (var transition in Transitions)
                transition.Execute(machine);
        }

    }
}
