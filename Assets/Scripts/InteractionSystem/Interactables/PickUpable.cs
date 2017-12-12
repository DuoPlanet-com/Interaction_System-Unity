using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables {
    [RequireComponent(typeof(Collider),typeof(Rigidbody))] 
    public class PickUpable : Abstracts.Interactable
    {

        [Header("Pick up settings")]
        public float smooth = 1;

        [Tooltip("The location at which the object you are trying to pick will stick to")]
        Transform pickUpLoc;

        public override void OnStart()
        {
            base.OnStart();
            pickUpLoc = GameObject.FindGameObjectWithTag("PickUpLoc").transform;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //velocity = GetComponent<Rigidbody>().velocity;
        }

        public override void OnInteractionStay()
        {
            base.OnInteractionStay();

            // transform.position = Vector3.SmoothDamp(transform.position, pickUpLoc.position, ref velocity, smooth);
            transform.position = Vector3.Lerp(transform.position, pickUpLoc.position, smooth);
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        }

    }
}