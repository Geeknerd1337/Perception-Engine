using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class UIService : PerceptionService
    {
        public Canvas MainCanvas;

        public override void Awake()
        {
            base.Awake();
            InitializeElements();
        }

        public void InitializeElements()
        {
            var canvasPrefab = AssetService.GetUI("Main UI Canvas");
            MainCanvas = Instantiate(canvasPrefab).GetComponent<Canvas>();
            MainCanvas.transform.SetParent(this.transform);
            //Set the canvas to be a Screen Space - Camera canvas
            MainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            //Set the canvas render camera to our camera
            MainCanvas.worldCamera = GameManager.GetService<CameraService>().ViewmodelCamera;
            //Set the canvas screen space distance to 0.01
            MainCanvas.planeDistance = 0.16f;
        }
    }
}
