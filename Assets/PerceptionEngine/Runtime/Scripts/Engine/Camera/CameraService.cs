using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class CameraService : PerceptionService
    {
        public override void Awake()
        {
            base.Awake();
        }

        private Camera _camera;

        public void InitializeCamera()
        {
            GameObject tripod = new GameObject("[Main] Camera");
            //Add an audio listener to the object
            tripod.AddComponent<AudioListener>();

            //Add the camera component
            _camera = tripod.AddComponent<Camera>();
            _camera.transform.position = Vector3.zero;
            _camera.transform.rotation = Quaternion.identity;
            _camera.renderingPath = RenderingPath.DeferredShading;
            //Make it so it doesn't render viewmodels
            _camera.cullingMask = ~LayerMask.GetMask(new string[] { "Viewmodel" });
        }
    }
}
