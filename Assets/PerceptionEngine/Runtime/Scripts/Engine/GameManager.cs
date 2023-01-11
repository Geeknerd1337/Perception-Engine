using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// This is the primary class involved in managing the game. This controls the camera as well as any relevant services which it maintains.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// A list of our services.
        /// TODO: This should probably be its own class which manages this list itself. However I am trying to avoid having to do GameManager.Instance.Services.GetService as opposed to just GameManager.GetService
        /// </summary>

        private List<PerceptionService> services = new List<PerceptionService>();

        public static GameManager Instance { get; private set; }

        public void Awake()
        {
            //Create a singleton instance of the game manager that persists throughout the entire game.
            //This is done so that the game manager can be accessed from anywhere in the game and it doesn't need to restart initialization every time.
            if (Instance == null)
            {
                //Set to not be destroyed on load
                DontDestroyOnLoad(this.gameObject);

                Instance = this;

                //Initialize the services
                InitializeServices();
            }
            else
            {
                if (Instance != this.gameObject)
                {
                    Destroy(this.gameObject);
                }
            }

        }


        public void InitializeServices()
        {
            AddService<AssetService>();
            //Add the audio service
            AddService<AudioService>();
        }


        public void AddService<T>() where T : PerceptionService
        {
            //Create a new game object called "NameOfService Service"
            GameObject serviceObject = new GameObject(typeof(T).Name + " Service");

            //Add the given component
            var service = serviceObject.AddComponent(typeof(T));

            //Add the service to our services
            services.Add((PerceptionService)service);

            serviceObject.transform.SetParent(this.transform);
        }


        public static T GetService<T>() where T : PerceptionService
        {

            //Loop through all of the services
            foreach (var service in GameManager.Instance.services)
            {
                //If the service is of the given type, return it
                if (service is T)
                {
                    return (T)service;
                }
            }

            //If we didn't find the service, return null
            return null;
        }
    }

}
