using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Perception.Engine
{
    /// <summary>
    /// This is the primary class involved in managing the game. This controls the camera as well as any relevant services which it maintains.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// A list of our services.
        /// TODO: This should probably be its own class which manages this list itself. 
        /// </summary>
        private List<PerceptionService> services = new List<PerceptionService>();

        public static GameManager Instance { get; private set; }

        /// <summary>
        /// A pawn is the main character in the game. This is the character is the physical representation of the player.
        /// </summary>
        public static Entity Pawn { get; set; }

        public List<string> ServicesToLoad = new List<string>();

        public virtual void Awake()
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

                //Register to the scene loaded event
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                if (Instance != this.gameObject)
                {
                    Destroy(this.gameObject);
                }
            }

        }


        public virtual void InitializeServices()
        {
            AddService<AssetService>();
            AddService<AudioService>();
            AddService<CameraService>();
            AddService<UIService>();
            AddService<SettingsService>();
            AddService<EventService>();
        }

        public void AddService<T>() where T : PerceptionService
        {
            //Create a new game object called "NameOfService Service"
            //Remove Service from the type name
            string serviceName = typeof(T).Name;
            serviceName = serviceName.Substring(0, serviceName.Length - "Service".Length);
            GameObject serviceObject = new GameObject(serviceName + " Service");

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

        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Pawn == null)
            {
                SpawnPlayer();
            }
        }

        public virtual void SpawnPlayer()
        {
            InfoPlayerStart[] playerStart = FindObjectsOfType<InfoPlayerStart>();
            if (playerStart.Length > 0)
            {
                //Pick a random player start
                int index = UnityEngine.Random.Range(0, playerStart.Length);

                //Spawn the player
                Pawn = Instantiate(AssetService.GetResource<Entity>("Player"));
                Pawn.transform.position = playerStart[index].transform.position;
                Pawn.transform.rotation = playerStart[index].transform.rotation;
            }
            else
            {
                //Otherwise just spawn the player at the origin
                Pawn = Instantiate(AssetService.GetResource<Entity>("Player"));
                Pawn.transform.position = Vector3.zero;
                Pawn.transform.rotation = Quaternion.identity;
            }

        }
    }

}
