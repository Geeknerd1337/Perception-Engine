using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    public class HideOnLoad : MonoBehaviour
    {
        //Gets the renderer and disables it on start
        void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.enabled = false;
        }
    }
}
