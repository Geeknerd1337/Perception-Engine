using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A class which holds the data for a surface. This is used to determine impact sounds and other effects.
    /// </summary>
    [CreateAssetMenu(menuName = "Perception/Surfaces/New Surface")]
    public class SurfaceObject : ScriptableObject
    {
        [Tab("Sounds")]
        public SoundObject ImpactSound;

        [Tab("Sounds")]
        public SoundObject FootstepSound;

        [Tab("Sounds")]
        public SoundObject LandSound;

        public float Friction;
        public float Density;
        public float Bounciness;

    }
}
