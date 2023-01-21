using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Perception.Engine;

/// <summary>
/// A playground class I'm using to test my editor code.
/// </summary>
public class InspectorPlayground : MonoBehaviour
{
    [Tab("Test")]
    public float Test;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventService.Run("Bark");
        }
    }

}

