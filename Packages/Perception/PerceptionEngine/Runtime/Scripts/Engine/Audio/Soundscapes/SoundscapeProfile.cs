using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    /// <summary>
    /// Essentially just a list of different soundscape sounds.
    /// TODO: Maybe add some cool features to this.
    /// </summary>
    [CreateAssetMenu(fileName = "Soundscape Profile", menuName = "Perception/Audio/New Soundscape Profile")]
    public class SoundscapeProfile : ScriptableObject
    {
        public List<SoundscapeSoundObject> Sounds = new List<SoundscapeSoundObject>();
    }
}
