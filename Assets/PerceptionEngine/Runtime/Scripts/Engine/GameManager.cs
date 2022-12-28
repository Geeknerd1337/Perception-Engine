using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// This is the primary class involved in managing the game. This controls the camera as well as any relevant services which it maintains.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private List<PerceptionService> services = new List<PerceptionService>();
    }

}
