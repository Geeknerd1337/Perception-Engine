using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A script which will always make a gameobject face the camera
    /// </summary>
    public class BillboardTransform : MonoBehaviour
    {
        private Quaternion _initialRotation;

        public Vector3 EulerOffset;


        // Update is called once per frame
        void Update()
        {
            transform.LookAt(GameManager.GetService<CameraService>().Camera.transform);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + EulerOffset);
        }
    }
}
