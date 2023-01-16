using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A class which represents a surface in the game world. This is used to determine impact sounds and other effects.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Surface : MonoBehaviour
    {
        [ResourceDropdown(ResourceType.Surface)]
        public string SurfaceName = "Custom";

        [ShowIf("_surfaceIsCustom")]
        [ModifiableProperty]
        public SurfaceObject Surf;

        public bool _surfaceIsCustom
        {
            get
            {
                return SurfaceName.Equals("Custom");
            }
        }


        public static SurfaceObject GetSurfaceObject(Vector3 position, float range = 1f)
        {
            RaycastHit hit;

            if (Physics.Raycast(new Ray(position, Vector3.down), out hit, range, PerceptionPhysics.GroundLayerMask, QueryTriggerInteraction.Ignore))
            {
                var fl = hit.collider.gameObject.GetComponent<Surface>();
                if (fl != null)
                {
                    return fl.Surf;
                }
            }

            return null;
        }



    }
}
