using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class ActorHealth : MonoBehaviour
    {
        public float Health { get => _health; set => _health = value; }
        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

        public delegate void OnDeathAction(DamageInfo info);
        /// <summary>
        /// An event that is called when the actor dies.
        /// </summary>
        public event OnDeathAction OnDeath;

        [SerializeField] private float _health = 100f;
        [SerializeField] private float _maxHealth = 100f;

        public void Reset()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(DamageInfo info)
        {

            Health -= info.Damage;
            if (_health <= 0f)
            {
                OnDeath?.Invoke(info);
            }
        }
    }
}
