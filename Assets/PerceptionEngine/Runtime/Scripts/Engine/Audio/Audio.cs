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
    }
}
