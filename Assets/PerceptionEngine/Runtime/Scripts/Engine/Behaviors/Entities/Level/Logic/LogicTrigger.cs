using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Perception.Engine
{
    public class LogicTrigger : LevelEntity
    {

        [BoxGroup("Config")]
        public bool TriggerOnce = true;

        [BoxGroup("Config")]
        [ShowIf("TriggerOnce")]
        [ModifiableProperty]
        public float TimeBetweenTriggers;

        [BoxGroup("Config")]
        public List<string> Tags = new List<string>();

        [BoxGroup("Events")]
        public UnityEvent OnTriggerEnterEvent;

        [BoxGroup("Events")]
        public UnityEvent OnTriggerExitEvent;

        [BoxGroup("Events")]
        public UnityEvent OnTriggerStayEvent;

        private TimeSince _timeSinceLastTriggered;
        private bool _triggered = false;


        public override void Fire()
        {
            //If we're only supposed to trigger once, and we've already triggered, return
            if (TriggerOnce)
            {
                if (_triggered)
                {
                    return;
                }
                _triggered = true;
            }

            if (_timeSinceLastTriggered > TimeBetweenTriggers)
            {
                _timeSinceLastTriggered = 0;
                OnTriggerEnterEvent.Invoke();
                base.Fire();
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (Tags.Contains(other.tag))
            {
                Debug.Log($"FIRING TRIGGER {this.name}");
                Fire();
            }
        }

        //On trigger stay
        public virtual void OnTriggerStay(Collider other)
        {
            if (_timeSinceLastTriggered > TimeBetweenTriggers && Tags.Contains(other.tag))
            {
                _timeSinceLastTriggered = 0;
                OnTriggerStayEvent.Invoke();
            }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            if (_timeSinceLastTriggered > TimeBetweenTriggers && Tags.Contains(other.tag))
            {
                _timeSinceLastTriggered = 0;
                OnTriggerExitEvent.Invoke();

            }
        }
    }
}
