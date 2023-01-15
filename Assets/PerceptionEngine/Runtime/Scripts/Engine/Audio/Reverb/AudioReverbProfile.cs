using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    [CreateAssetMenu(fileName = "Audio Reverb Profile", menuName = "Perception/Audio/Audio Reverb Profile", order = 1)]
    public class AudioReverbProfile : ScriptableObject
    {
        public float HFReference;
        public float density;
        public float diffusion;
        public float reverbDelay;
        public float reverb;
        public float reflectionsDelay;
        public float reflections;
        public float decayHFRatio;
        public float decayTime;
        public float roomHF;
        public float room;
        public float roomLF;
        public float LFReference;
    }
}
