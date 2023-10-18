using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// Physics Helper
    /// </summary>
    public class PerceptionPhysics
    {
        public static int GroundLayerMask = LayerMask.GetMask(new string[] { "Default" });
        public static int InteractMask = LayerMask.GetMask(new string[] { "Interactable" });
    }
}
