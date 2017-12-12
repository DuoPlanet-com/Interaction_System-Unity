using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables { 
    [RequireComponent(typeof(Tools.Door))]
    public class AutoDoor : Abstracts.Interactable {
        [Header("AutoDoor Settings")]
        public ActivationMethod activateBy = ActivationMethod.INTERACT;

        public override void OnInteractionEnter()
        {
            base.OnInteractionEnter();
            if (IsInteractable()) { 
                if (activateBy == ActivationMethod.INTERACT) { 
                    GetComponent<Tools.Door>().Open();
                }
            }
        }

        public override void OnWatchEnter()
        {
            base.OnWatchEnter();

            if (IsInteractable()) { 
                if (activateBy == ActivationMethod.WATCH)
                {
                    GetComponent<Tools.Door>().Open();
                }
            }
        }

        public override void OnTouchEnter(Collision interactorCollision, GameObject interactor)
        {
            base.OnTouchEnter(interactorCollision, interactor);
            if (IsInteractable()) { 
                if (activateBy == ActivationMethod.TOUCH)
                {
                    GetComponent<Tools.Door>().Open();
                }
            }
        }
    }
}