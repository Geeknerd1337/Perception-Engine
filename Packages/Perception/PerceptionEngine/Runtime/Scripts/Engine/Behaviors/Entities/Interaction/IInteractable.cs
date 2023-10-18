using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Perception.Engine
{
    public interface IInteractable
    {
        void OnInteract();

        void OnContinueInteract();

        void OnInteractEnd();
    }
}
