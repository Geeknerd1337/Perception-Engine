using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    /// <summary>
    /// The way the camera actually gets modified by code in the game. Used as a base for controlling cameras in the game.
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
