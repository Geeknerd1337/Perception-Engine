using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    [CreateAssetMenu(fileName = "Soundscape Profile", menuName = "Netscape/Soundscapes/Sound Scape Profile", order = 1)]
    public class SoundscapeProfile : ScriptableObject
    {
        public List<SoundscapeSoundObject> Sounds = new List<SoundscapeSoundObject>();
    }
}
