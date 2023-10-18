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

        public Tripod LastTripod;

        #region Private Members
        private Camera _camera;
        private Camera _viewmodelCamera;

        #endregion

        public override void Awake()
        {
            base.Awake();
            InitializeCamera();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            Build();
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

            //Since the camera persists between scenes, make it not destroy on load
            DontDestroyOnLoad(tripod);
        }


        private Tripod.Setup _lastSetup = new Tripod.Setup()
        {
            FieldOfView = 68f,
            Rotation = Quaternion.identity,
            Position = Vector3.zero,
            ViewModel = new Tripod.Setup.ViewModelSetup()
            {
                FieldOfView = 46.6921257105f,
                Clipping = new Vector2(0.14f, 10f),
                Offset = Vector3.zero,
                Angles = Quaternion.identity
            }
        };


        public void Build()
        {
            _lastSetup.FieldOfView = 68f;
            _lastSetup.ClipPlanes = new Vector2(0.1f, 700);
            _lastSetup.Camera = _camera;
            _lastSetup.ViewModel.Angles = Quaternion.identity;
            _lastSetup.ViewModel.Offset = Vector3.zero;

            _viewmodelCamera.fieldOfView = _lastSetup.ViewModel.FieldOfView;
            _viewmodelCamera.nearClipPlane = _lastSetup.ViewModel.Clipping.x;
            _viewmodelCamera.farClipPlane = _lastSetup.ViewModel.Clipping.y;



            _lastSetup = BuildTripod(_lastSetup);
            Finalize(_lastSetup);
        }

        public Tripod.Setup BuildTripod(Tripod.Setup camSetup)
        {
            var cam = FindActiveCamera();

            //Disable the last tripod if we've switched to a new one
            //Use on enabled events in order to maintain what a tripod should do when activated
            if (LastTripod != cam)
            {
                if (LastTripod != null)
                {
                    LastTripod.enabled = false;
                }
                LastTripod = cam;
                if (LastTripod != null)
                {
                    LastTripod.enabled = true;
                }
            }

            //Build the tipods
            LastTripod?.OnTripodBuild(ref camSetup);
            PostCameraSetup(ref camSetup);

            return camSetup;

        }

        private Tripod FindActiveCamera()
        {
            if (GameManager.Pawn != null)
            {
                return GameManager.Pawn.GetComponent<Tripod>();
            }

            //Otherwise, return null
            return null;
        }

        /// <summary>
		/// Allows us to apply modifications to the current tripod setup for various things such as the player
		/// </summary>
		public void PostCameraSetup(ref Tripod.Setup camSetup)
        {

        }


        public void Finalize(Tripod.Setup setup)
        {
            var trans = _camera.transform;
            trans.localPosition = setup.Position;
            trans.localRotation = setup.Rotation;
            _camera.fieldOfView = setup.FieldOfView;
            _camera.nearClipPlane = setup.ClipPlanes.x;
            _camera.farClipPlane = setup.ClipPlanes.y;
        }


    }
}
