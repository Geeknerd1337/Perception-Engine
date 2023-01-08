using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Perception.Engine
{
    /// <summary>
    /// This incredibly useful script is thanks to a bit of reading I've done on sound assets in unity from Baruchadi on github. You can see the original gist
    /// this is based on over here:
    /// https://gist.github.com/baruchadi/3c23caf609fa0f4bd349d9ea432eb9c4
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Sound", menuName = "Perception/Audio/New Sound")]
    public class SoundObject : PerceptionScriptableObject
    {
        /// <summary>
        /// Semitones are a unit of measurement for pitch. This is the number of semitones that the pitch of the sound will be shifted by.
        /// This is better than just randoming a pitch because it allows us to have a more consistent sound.
        /// </summary>
        private static readonly float SEMITONES_TO_PITCH_CONVERSION_UNIT = 1.05946f;

        /// <summary>
        /// The number of clips we want to use
        /// </summary>
        public AudioClip[] Clips;

        /// <summary>
        /// The random volume of the sound
        /// </summary>
        [MinMaxSlider(0f, 1f)]
        public Vector2 Volume = new Vector2(0.5f, 0.5f);

        [Tab("Pitch")]
        public float WhatsUp;

    }
}

