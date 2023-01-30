using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Perception.Engine;


namespace Perception.Editor
{
    public class LDToolboxHelper
    {
        public static void ConvertToTrigger(GameObject gameObject)
        {
            //Convert the gameobjects collider
            BoxCollider boxCollider = ConvertToBoxCollider(gameObject);
            boxCollider.isTrigger = true;
            Material triggerMaterial = Resources.Load<Material>("Perception/Materials/Trigger");
            //Set the shared material
            if (gameObject.GetComponent<MeshRenderer>() != null)
            {
                gameObject.GetComponent<MeshRenderer>().sharedMaterial = triggerMaterial;
            }
            else if (gameObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial = triggerMaterial;
            }
            //Add a logic trigger to it
            LogicTrigger logicTrigger = gameObject.AddComponent<LogicTrigger>();

            //Add a hide on load component
            HideOnLoad hideOnLoad = gameObject.AddComponent<HideOnLoad>();

            logicTrigger.Tags.Add("Player");


        }

        public static BoxCollider ConvertToBoxCollider(GameObject gameObject)
        {
            //Check to see if this gameobject has a collider
            if (gameObject.GetComponent<Collider>() == null)
            {
                //If not, add a box collider
                return gameObject.AddComponent<BoxCollider>();
            }
            else
            {
                //If it does, convert it to a box collider
                Collider collider = gameObject.GetComponent<Collider>();
                if (!(collider is BoxCollider))
                {
                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                    boxCollider.center = collider.bounds.center;
                    boxCollider.size = collider.bounds.size;
                    GameObject.DestroyImmediate(collider);
                    return boxCollider;
                }
                else
                {
                    return collider as BoxCollider;
                }
            }
        }

    }
}
