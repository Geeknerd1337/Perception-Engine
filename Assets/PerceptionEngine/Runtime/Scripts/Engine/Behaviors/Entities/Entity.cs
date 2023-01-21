using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class Entity : MonoBehaviour
    {
        /// <summary>
        /// A static list of all the entities which makes running queries on them easier and more performant as opposed to all gameobjects.
        /// </summary>
        public static List<Entity> All = new List<Entity>();

        public virtual void OnEnable()
        {
            EventService.Register(this);
        }

        public virtual void OnDisable()
        {
            EventService.Unregister(this);
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Awake()
        {
            All.Add(this);
        }


        public virtual void OnDestroy()
        {
            All.Remove(this);
        }

        public virtual void TakeDamage(DamageInfo info)
        {

        }
    }
}