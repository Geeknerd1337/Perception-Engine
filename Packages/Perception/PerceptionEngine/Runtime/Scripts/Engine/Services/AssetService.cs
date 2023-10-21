using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// This service allows us to load various types from the Resources folder in unity. That way we have an easy way to
    /// access them at runtime without necessarily requiring a scene reference. This is considered sacrilege by some,
    /// I've done a good amount of research on this topic at this point and it's honestly a mess.
    /// https://forum.unity.com/threads/scriptableobject-references-in-addressables.777155/
    /// Feel free to read the above. I use scriptable objects a lot, and at some point this system will need to be replaced with either addressables
    /// or asset bundles, but for the time being there are more important systems I'd like to build.
    /// 
    /// Only put stuff in your resources you dont intend to have directly referenced in your scenes. Try and keep it small. I will eventually replace this system
    /// with something better
    /// </summary>
    public class AssetService : PerceptionService
    {
        /// <summary>
        /// A dictionary which maps resource types to a list of strings mapped to objects. This is used to cache
        /// resources so we don't have to load them every time we need them. The reason ResourceType exists is so this can all be in one data structure
        /// which can be more easily queried.
        /// </summary>
        public Dictionary<ResourceType, Dictionary<string, Object>> Library = new Dictionary<ResourceType, Dictionary<string, Object>>();

        public override void Awake()
        {
            base.Awake();
            InitializeAssetLibrary();
        }

        /// <summary>
        /// A dictionary which maps resource types to their paths in the resources folder.
        /// </summary>
        public static Dictionary<ResourceType, string> ResourcePaths = new Dictionary<ResourceType, string>()
        {
            {ResourceType.Sound, "Perception/Audio/Sound"},
            {ResourceType.AudioSource, "Perception/Audio/Audio Sources"},
            {ResourceType.Entity, "Perception/Entity"},
            {ResourceType.UI, "Perception/Prefabs/UI"},
            {ResourceType.Surface, "Perception/Surfaces"},
            {ResourceType.Material, "Perception/Materials"},
        };


        /// <summary>
        /// A dictionary which maps types to resource types. This is used to determine what type of resource we're trying to load
        /// TODO: This should be combined with the ResourcePaths dictionary as a single dictionary which maps types to a tuple of resource type and path
        /// </summary>
        public static Dictionary<System.Type, ResourceType> TypeToResourceType = new Dictionary<System.Type, ResourceType>()
        {
            {typeof(SoundObject), ResourceType.Sound},
            {typeof(AudioSource), ResourceType.AudioSource},
            {typeof(Entity), ResourceType.Entity},
            {typeof(GameObject), ResourceType.UI},
            {typeof(Surface), ResourceType.Surface},
            {typeof(Material), ResourceType.Material},
        };

        public void InitializeAssetLibrary()
        {
            //Adding these paths for now so things can be a little more organized on the unity side. Should probably be another system.
            Dictionary<ResourceType, string> paths = ResourcePaths;

            //Iterate over the resourcetype enumerator
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                //Create a new dictionary for this resource type
                Library.Add(type, new Dictionary<string, Object>());


                this.Log($"Loading {type.ToString()} Resources");

                //Get all the resources of this type
                Object[] resources = Resources.LoadAll(paths[type]);


                //Iterate over the resources and add them to the dictionary
                foreach (Object resource in resources)
                {
                    Library[type].Add(resource.name, resource);
                    this.Log($"\t\tLoaded {resource.name}");
                }
            }
        }

        /// <summary>
        /// This is essentially just short hand for returning a sound object from the asset service. Cuts down on boilrer plate
        /// </summary>
        public static SoundObject GetSound(string name)
        {
            //Try to get the sound from the library
            bool potentialSound = GameManager.GetService<AssetService>().Library[ResourceType.Sound].TryGetValue(name, out Object sound);

            //If we couldn't find it, log an error and return null
            if (!potentialSound)
            {
                GameManager.GetService<AssetService>().LogError($"Could not find sound {name}");
                return null;
            }
            else
            {
                return (SoundObject)sound;
            }
        }

        /// <summary>
        ///  This is essentially just short hand for returning an AudioSource from the asset service. Cuts down on boilrer plate
        /// </summary>
        public static AudioSource GetAudioSource(string name)
        {
            //Try to get the sound from the library
            bool potentialSource = GameManager.GetService<AssetService>().Library[ResourceType.AudioSource].TryGetValue(name, out Object source);

            //If we couldn't find it, log an error and return null
            if (!potentialSource)
            {
                GameManager.GetService<AssetService>().LogError($"Could not find audio source {name}");
                return null;
            }
            else
            {
                return (source as GameObject).GetComponent<AudioSource>();
            }
        }

        public static GameObject GetUI(string name)
        {
            //Try to get the sound from the library
            bool potentialUI = GameManager.GetService<AssetService>().Library[ResourceType.UI].TryGetValue(name, out Object ui);

            //If we couldn't find it, log an error and return null
            if (!potentialUI)
            {
                GameManager.GetService<AssetService>().LogError($"Could not find UI {name}");
                return null;
            }
            else
            {
                return (GameObject)ui;
            }
        }

        public static T GetResource<T>(string name) where T : Object
        {
            //Get the type of the resource we're looking for
            ResourceType type = TypeToResourceType[typeof(T)];

            //Try to get the sound from the library
            bool potentialResource = GameManager.GetService<AssetService>().Library[type].TryGetValue(name, out Object resource);

            //If we couldn't find it, log an error and return null
            if (!potentialResource)
            {
                GameManager.GetService<AssetService>().LogError($"Could not find resource {name}");
                return null;
            }
            else
            {
                if (typeof(T) != typeof(GameObject) && resource is GameObject)
                {
                    return (T)(resource as GameObject).GetComponent<T>();
                }
                else
                {
                    return (T)resource;
                }

            }
        }
    }

    /// <summary>
    /// A simple enmuerator which shows us the different resource types. Purely organizational. 
    /// </summary>
    public enum ResourceType
    {
        Sound,
        AudioSource,
        Entity,
        UI,
        Surface,
        Material
    }
}
