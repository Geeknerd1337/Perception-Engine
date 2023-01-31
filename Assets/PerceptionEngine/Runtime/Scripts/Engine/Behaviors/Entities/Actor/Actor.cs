using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    public class Actor : AnimatedEntity
    {
        public Controller Controller;
        public EyeHelper EyesHelper;
        public ActorHealth Health;
        public Transform Eyes;

        /// <summary>
        /// Does this need to be here?
        /// </summary>
        public virtual void BuildInput()
        {

        }

        public override void Awake()
        {
            base.Awake();
            InitializeHealth();
            InitializeEyes();
        }


        void InitializeHealth()
        {
            Health = GetComponent<ActorHealth>();
            if (Health == null)
            {
                Health = gameObject.AddComponent<ActorHealth>();
            }
            Health.OnDeath += OnDeath;
        }

        void InitializeEyes()
        {
            //If you haven't assigned an eyes transform, then create one
            if (Eyes == null)
            {
                var e = new GameObject("Eyes");
                e.transform.SetParent(transform);
                Eyes = e.transform;
                Eyes.localPosition = Vector3.zero;
                Eyes.localRotation = Quaternion.identity;
            }

            EyesHelper = new EyeHelper();
        }

        public override void TakeDamage(DamageInfo info)
        {
            base.TakeDamage(info);
            Health.TakeDamage(info);
        }


        public virtual void OnDeath(DamageInfo info)
        {
            Controller.Disable();
        }

        public virtual Controller ActiveController()
        {
            return Controller;
        }
    }
}
