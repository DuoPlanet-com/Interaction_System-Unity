using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables { 
    public class RotateByEvent : Abstracts.Interactable {

        [Header("RotateByEvent Settings")]
        public ActivationMethod activateBy = ActivationMethod.INTERACT;

        [Space(8)]
        public Transform objectToRotate;
        public Space rotationSpace = Space.World;
        public Vector3 direction;
        public float speed;


        public override void OnStart()
        {
            base.OnStart();
            if (objectToRotate == null)
            { 
                objectToRotate = transform;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (IsInteractable())
            {
                if (activateBy == ActivationMethod.INTERACT)
                {
                    objectToRotate.Rotate(direction, speed * Input.GetAxis("Interact"), rotationSpace);
                }
            }
        }

        public override void OnTouchStay(Collision interactorCollision, GameObject interactor)
        {
            base.OnTouchStay(interactorCollision, interactor);
            if (activateBy == ActivationMethod.TOUCH)
            {
                objectToRotate.Rotate(direction, speed, rotationSpace);
            }
        }

        public override void OnTouchTriggerStay(Collider other, GameObject interactor)
        {
            base.OnTouchTriggerStay(other, interactor);
            if (activateBy == ActivationMethod.TRIGGER)
            {
                objectToRotate.Rotate(direction, speed, rotationSpace);
            }
        }

        public override void OnWatchStay()
        {
            base.OnWatchStay();
            if (activateBy == ActivationMethod.WATCH) { 
                objectToRotate.Rotate(direction, speed , rotationSpace);
            }
        }

    }
}