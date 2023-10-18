using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A simple ragdoll which will activate when the actor dies.
    /// </summary>
    public class Ragdoll : MonoBehaviour
    {
        public Rigidbody[] _rigidBodies;
        private Animator _animator;
        public bool DisableAnimator = true;

        private ActorHealth _health;

        void Awake()
        {
            _rigidBodies = GetComponentsInChildren<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _health = GetComponentInParent<ActorHealth>();
            if (_health != null)
            {
                _health.OnDeath += ActivateRagdoll;
            }
            DeactivateRagdoll();
        }

        void ActivateRagdoll(DamageInfo info)
        {
            if (_animator != null && DisableAnimator)
            {
                _animator.enabled = false;
            }
            foreach (var rigidBody in _rigidBodies)
            {
                rigidBody.isKinematic = false;
                rigidBody.AddForce(info.Force, ForceMode.Impulse);
                //Players should likely not be able to intereact with ragdolls
                rigidBody.gameObject.layer = LayerMask.NameToLayer("Player Ignore");
            }
        }

        public void DeactivateRagdoll()
        {
            if (_animator != null)
            {
                _animator.enabled = true;
            }
            foreach (var rigidBody in _rigidBodies)
            {
                rigidBody.isKinematic = true;
            }
        }
    }
}
