using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables {
    public class ToggleIsInteractableBy : Abstracts.Interactable {

        public Abstracts.Interactable[] interactableToToggle;

        public ActivationMethod activateBy = ActivationMethod.INTERACT;


        public override void OnInteractionEnter()
        {
            base.OnInteractionEnter();
            if (activateBy == ActivationMethod.INTERACT)
                Toggle();
        }

        public override void OnWatchEnter()
        {
            base.OnWatchEnter();
            if (activateBy == ActivationMethod.WATCH)
                Toggle();
        }

        public override void OnTouchEnter(Collision interactorCollision, GameObject interactor)
        {
            base.OnTouchEnter(interactorCollision, interactor);
            if (activateBy == ActivationMethod.TOUCH)
                Toggle();
        }

        public void Toggle() {
            foreach(Abstracts.Interactable interactable in interactableToToggle) { 
                if (interactable.IsInteractable()) {
                    interactable.SetInteractable(false);
                } else
                {
                    interactable.SetInteractable(true);
                }
            }
        }
    }
}