using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools { 
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Toucher : MonoBehaviour {

        private void OnCollisionEnter(Collision collision)
        {
            Abstracts.Interactable interactable = collision.gameObject.GetComponent<Abstracts.Interactable>();
            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    interactable.OnTouchEnter(collision, gameObject);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            Abstracts.Interactable interactable = collision.gameObject.GetComponent<Abstracts.Interactable>();
            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    interactable.OnTouchStay(collision, gameObject);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            Abstracts.Interactable interactable = collision.gameObject.GetComponent<Abstracts.Interactable>();
            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    interactable.OnTouchExit(collision, gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Abstracts.Interactable interactable = other.gameObject.GetComponent<Abstracts.Interactable>();
            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    interactable.OnTouchTriggerEnter(other, gameObject);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Abstracts.Interactable interactable = other.gameObject.GetComponent<Abstracts.Interactable>();
            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    interactable.OnTouchTriggerStay(other, gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Abstracts.Interactable interactable = other.gameObject.GetComponent<Abstracts.Interactable>();
            if (interactable != null)
            {
                if (interactable.IsInteractable()) { 
                    interactable.OnTouchTriggerExit(other, gameObject);
                }
            }
        }

    }
}