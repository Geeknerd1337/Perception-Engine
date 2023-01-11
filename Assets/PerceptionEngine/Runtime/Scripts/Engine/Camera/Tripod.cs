using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    /// <summary>
    /// A way of maintaining singleton camera reference which is controlled by a tripod. 
    /// Essentially a port from Espionage.Engine which uses Unity's standard game loop instead of a custom built one
    /// </summary>
    public class Tripod : MonoBehaviour
    {

        public virtual void OnTripodBuild(ref Setup setup)
        {

        }

        /// <summary>
        /// A struct for managing basic aspects of our tripod, allowing us to manipulate it and move it in the world.
        /// Updated in late update
        /// </summary>
        public struct Setup
        {
            public float FieldOfView;
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector2 ClipPlanes;
            public Camera Camera;

            public ViewModelSetup ViewModel;

            public struct ViewModelSetup
            {
                public float FieldOfView;
                public Vector2 Clipping;

                public Vector3 Offset;
                public Quaternion Angles;
            }
        }
    }
}
