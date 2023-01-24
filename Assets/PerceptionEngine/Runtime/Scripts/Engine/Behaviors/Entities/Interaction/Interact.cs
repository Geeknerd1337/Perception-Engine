using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A basic class for handling interactions with objects. Allows for the ability to interact with objects in the world, even continiously like with cranks or valves.
    /// </summary>
    public class Interact : MonoBehaviour
    {
        public float Range = 3.0f;

        /// <summary>
        /// A delay between each interaction
        /// </summary>
        public float InteractionDelay = 0.5f;

        private TimeSince _timeSinceInteract;
        public KeyCode InteractKey = KeyCode.E;


        private bool _interacted;

        private IInteractable _lastInteractable;

        public void Start()
        {
        }

        public void Update()
        {
            Interaction();
        }

        void Interaction()
        {
            if (GameManager.Pawn is Actor actor)
            {
                var _camPosition = actor.Eyes.position;
                var _camForward = actor.Eyes.forward;

                //Create a raycast to check if we are looking at an interactable
                RaycastHit hit;
                var _interactableFound = Physics.Raycast(_camPosition, _camForward, out hit, Range, PerceptionPhysics.InteractMask);

                IInteractable interactable = null;
                if (_interactableFound)
                {
                    interactable = hit.collider.GetComponent<IInteractable>();
                    _lastInteractable = interactable;
                }

                //If we are looking at an interactable and we press the interact key
                if (Input.GetKeyDown(InteractKey))
                {
                    //And we haven't interacted with it yet
                    if (_interactableFound && !_interacted && _timeSinceInteract >= InteractionDelay)
                    {
                        interactable = hit.collider.GetComponent<IInteractable>();
                        _lastInteractable = interactable;
                        if (interactable != null)
                        {
                            interactable.OnInteract();
                            _timeSinceInteract = 0f;
                            _interacted = true;
                        }
                    }
                }

                //If we're holding down the interact key, call the continue interact method
                if (Input.GetKey(InteractKey))
                {
                    if (interactable != null)
                    {
                        interactable.OnContinueInteract();
                    }
                }

                //If we release the interact key, call the interact end method
                if (Input.GetKeyUp(InteractKey))
                {
                    if (interactable != null)
                    {
                        interactable.OnInteractEnd();
                    }
                    _interacted = false;
                }

                //If for whatever reason the interactable doesn't exist anymore, end the interaction
                if (interactable == null && _lastInteractable != null)
                {
                    _lastInteractable.OnInteractEnd();
                    _lastInteractable = null;
                }

            }
        }

    }
}
