using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    public class SoundscapeSoundObject : SoundObject
    {
        /// <summary>
		/// Whether the sound is affected by reverb
		/// </summary>
		public bool AffectedByReverb;

        /// <summary>
        /// The time range for a sound to play
        /// </summary>
        public Vector2 PlayInterval;
    }
}
