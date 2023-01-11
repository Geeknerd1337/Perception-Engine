using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class CameraUtility
    {
        /// <summary>
        /// Since the viewmodel camera can have a different FOV than the main camera, a position on one camera will appear wrong on the other.
        /// This helps to convert from a viewmodel position to a standard world space position. Useful for any time you need an effect to come from the viewmodel camera.
        /// </summary>
        public static Vector3 ViewmodelToStandard(Vector3 position)
        {
            var _sourcePosition = GameManager.GetService<CameraService>().ViewmodelCamera.WorldToViewportPoint(position);
            var _finalPosition = GameManager.GetService<CameraService>().Camera.ViewportToWorldPoint(_sourcePosition);

            return _finalPosition;
        }

        /// <summary>
        /// Converts a point in standard space to viewmodel space.
        /// </summary>
        public Vector3 StandardToViewModel(Vector3 position)
        {
            var _sourcePosition = GameManager.GetService<CameraService>().Camera.WorldToViewportPoint(position);
            var _finalPosition = GameManager.GetService<CameraService>().ViewmodelCamera.ViewportToWorldPoint(_sourcePosition);

            return _finalPosition;
        }
    }
}
