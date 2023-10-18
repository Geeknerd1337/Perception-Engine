using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace Perception.Engine
{
    /// <summary>
    /// A class meant for the playing of audio in the game. 
    /// </summary>
    public class PerceptionAudio
    {

        public static void FadeTo(AudioSource audioSource, float FadeTime, float TargetVolume)
        {
            GameManager.GetService<AudioService>().StartCoroutine(GameManager.GetService<AudioService>().FadeTo(audioSource, FadeTime, TargetVolume));
        }

        /// <summary>
        /// Plays a given sound at a point in the world.
        /// </summary>
        public static AudioSource FromWorld(SoundObject sound, Vector3 position, AudioSource source = null)
        {
            AudioSource sourceObject = null;

            //Get the default audio source from the asset service
            AudioSource defaultSource = AssetService.GetAudioSource("Default");

            //Instantiate the default source if none is specified
            if (source == null)
            {
                sourceObject = Object.Instantiate(defaultSource);
            }
            else
            {
                //Otherwise create a copy of the specified source
                sourceObject = Object.Instantiate(source);
            }

            sourceObject.name = sound.name;
            sourceObject.transform.SetParent(null);

            //Set the source's position
            sourceObject.transform.position = position;

            //Play the sound
            return sound.Play(sourceObject);
        }

        public static AudioSource FromWorld(string sound, Vector3 position, AudioSource source = null)
        {
            return FromWorld(AssetService.GetSound(sound), position, source);
        }

        /// <summary>
		/// Unity has a default limit of 32 voices, plays the sound from a provided audio source.
		/// </summary>
		public static void FromAudioSouce(SoundObject sound, AudioSource source)
        {
            sound.PlayOneShot(source);
        }

        public static void FromAudioSouce(string sound, AudioSource source)
        {
            FromAudioSouce(AssetService.GetSound(sound), source);
        }


        /// <summary>
        /// Plays an audio non positionally, useful for when you want to play a sound from the camera.
        /// </summary>
        public static AudioSource FromScreen(SoundObject sound, AudioSource source = null, bool destroy = true)
        {
            AudioSource sourceObject = null;
            AudioSource screenSource = AssetService.GetAudioSource("Screen");

            //Instantiate the screen source if none is specified
            if (source == null)
            {
                sourceObject = Object.Instantiate(screenSource);
            }
            else
            {
                //Otherwise create a copy of the specified source
                sourceObject = Object.Instantiate(source);
            }

            sourceObject.name = sound.name;
            sourceObject.transform.SetParent(null);

            //Play the sound
            return sound.Play(sourceObject, destroy);
        }

        public static AudioSource FromScreen(string sound, AudioSource source = null, bool destroy = true)
        {
            return FromScreen(AssetService.GetSound(sound), source, destroy);
        }

        /// <summary>
        /// Same as FromScreen but uses an audio mixer which can have reverb applied to it. Useful for soundscapes.
        /// </summary>
        public static AudioSource FromScreenReverb(SoundObject sound, AudioSource source = null)
        {
            AudioSource sourceObject = null;
            AudioSource screenSource = AssetService.GetAudioSource("Screen With Reverb");

            //Instantiate the default source if none is specified
            if (source == null)
            {
                sourceObject = Object.Instantiate(screenSource);
            }
            else
            {
                //Otherwise create a copy of the specified source
                sourceObject = Object.Instantiate(source);
            }

            sourceObject.name = sound.name;
            sourceObject.transform.SetParent(null);

            //Play the sound
            return sound.Play(sourceObject);
        }

        /// <summary>
        /// Allow the sound from a game object, allowing it to be parented
        /// </summary>
        public static AudioSource FromGameObject(SoundObject sound, GameObject g, AudioSource source = null)
        {
            var obj = FromWorld(sound, g.transform.position, source);
            obj.transform.SetParent(g.transform);
            return obj;
        }

        public static AudioSource FromGameObject(string sound, GameObject g, AudioSource source = null)
        {
            return FromGameObject(AssetService.GetSound(sound), g, source);
        }
    }
}
