using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace Perception.Engine
{
    public class CameraService : PerceptionService
    {
        public Camera Camera
        {
            get
            {
                return _camera;
            }
        }

        public Camera ViewmodelCamera
        {
            get
            {
                return _viewmodelCamera;
            }
        }

        #region Private Members
        private Camera _camera;
        private Camera _viewmodelCamera;
        #endregion

        public override void Awake()
        {
            base.Awake();
        }



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

            //Create a second camera which renders the viewmodel and UI
            //This is done this way because I want UI to be affected by post processing.
            GameObject viewmodelCamera = new GameObject("[Viewmodel] Camera");
            viewmodelCamera.transform.SetParent(tripod.transform);
            viewmodelCamera.transform.localPosition = Vector3.zero;
            viewmodelCamera.transform.localRotation = Quaternion.identity;
            _viewmodelCamera = viewmodelCamera.AddComponent<Camera>();
            _viewmodelCamera.renderingPath = RenderingPath.DeferredShading;
            _viewmodelCamera.clearFlags = CameraClearFlags.Depth;
            _viewmodelCamera.depth = 1;
            _viewmodelCamera.cullingMask = LayerMask.GetMask(new string[] { "Viewmodel", "UI" });
        }


    }
}
