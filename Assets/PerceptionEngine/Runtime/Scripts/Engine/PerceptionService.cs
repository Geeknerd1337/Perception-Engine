using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    /// <summary>
    /// A percepttion engine service is something that is used by the perception engine to provide functionality to the game. This is the base class for all perception engine services.
    /// </summary>
    public class PerceptionService : MonoBehaviour
    {
        public static PerceptionService Instance { get; private set; }

        public virtual void Awake()
        {
            //Create a singleton instance of the game manager that persists throughout the entire game.
            //This is done so that the game manager can be accessed from anywhere in the game and it doesn't need to restart initialization every time.
            if (Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                if (Instance != this.gameObject)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        public void AddService<T>()
        {
            //Create a new game object called "NameOfService Service"
            GameObject service = new GameObject(typeof(T).Name + "Service");
        }
    }
}
