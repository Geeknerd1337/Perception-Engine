using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Perception.Engine
{
    /// <summary>
    /// A logic relay which will fire its events after a short delay
    /// </summary>
    public class LogicTimer : LevelEntity
    {
        [BoxGroup("Config")]
        public float Delay;
        public void StartTimer()
        {
            StartCoroutine(StartDelay());
        }

        //Create a coroutine which will run the fire after a short delay
        IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(Delay);
            Fire();
        }

    }
}
