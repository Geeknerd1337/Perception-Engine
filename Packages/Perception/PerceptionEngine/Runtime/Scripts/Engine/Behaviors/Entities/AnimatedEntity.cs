using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class AnimatedEntity : ModelEntity
    {
        public Animator Animator;

        /// <summary>
        /// String representing the name of the transform holding our model
        /// </summary>
        public string AnimatorHolder = string.Empty;

        public override void Awake()
        {
            base.Awake();
            GetAnimator();
        }

        /// <summary>
        /// A method for returning if the animator is currently playing a state.
        /// </summary>
        public bool IsPlaying(string stateName, int animLayer = 0)
        {
            var anim = Animator;

            if (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
                    anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
                return true;
            else
                return false;
        }

        private void GetAnimator()
        {
            //If we've already assigned it, no need to get it
            if (Animator != null)
            {
                return;
            }
            //First, check to see if we can find a holder
            if (AnimatorHolder != string.Empty && transform.Find(AnimatorHolder).gameObject is GameObject hold)
            {
                Animator = hold.GetComponentInChildren<Animator>();
            }
            else
            {
                //Otherwise, first check to see if this object has a renderer component
                if (GetComponentInChildren<Animator>() is Animator a)
                {
                    Animator = a;
                }
                else
                {
                    Debug.LogWarning("Animated Entity contains no animator, did you foget to add one or could you be missing a reference?");
                }
            }
        }
    }
}
