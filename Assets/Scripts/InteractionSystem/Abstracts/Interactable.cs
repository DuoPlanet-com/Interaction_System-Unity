using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstracts {
    [RequireComponent(typeof(Collider))]
    public abstract class Interactable : MonoBehaviour {

        [Header("Interaction settings")]
        [Tooltip("Within what range should the player be able to interact with this object?")]
        public float interactionRange = 5;
        public float cooldown = 0;
        public bool oneTime = false;
        public bool startUnlocked = true;

        [Tooltip("Used for debugging purposes")]
        public bool printDistance = false;

        [Space(10)]
        public AudioClip interactionSound;
        public float interactionSoundVolume = 1;
        public AudioClip watchSound;
        public float watchSoundVolume = 1;
        public AudioClip touchSound;
        public float touchSoundVolume = 1;
        [Space(5)]
        public AudioClip lockedSound;
        public float lockedVolume = 1;


        [Header("Interaction internals")]
        [SerializeField]
        bool doneTime = false;

        [SerializeField]
        bool isInteractable;

        [SerializeField]
        float cooldownTimer;

        public enum ActivationMethod
        {
            TOUCH, WATCH, INTERACT, TRIGGER
        }

        private void Start()
        {
            //if (gameObject.tag != "Interactable")
           // {
           //     print("<color=olive>Warning ! Interactable objects must be tagged 'Interactable' @ " +gameObject.name+ "</color>\nHave you forgotten to tag it?");
            //}
            RequisiteCheck();

            if (startUnlocked)
            {
                isInteractable = true;
            } else
            {
                isInteractable = false;
            }
            cooldownTimer = cooldown;
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        public bool IsWithinRange(GameObject interactor)
        {

            if (cooldownTimer <= 0 && !(!doneTime && oneTime))
            {
                float distance = Vector3.Distance(interactor.transform.position, transform.position);
                if (printDistance)
                {
                    print(distance);
                }
                if (Vector3.Distance(interactor.transform.position, transform.position) <= interactionRange)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate()
        {
            cooldownTimer -= Time.deltaTime;
        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnWatchEnter()
        {
          
            if (watchSound != null)
            {
                AudioSource.PlayClipAtPoint(watchSound, transform.position, watchSoundVolume);
            }
        }

        public virtual void OnWatchStay()
        {

        }

        public virtual void OnWatchExit()
        {

        }

        public virtual void OnInteractionEnter()
        {

            if (isInteractable == false)
            {
                AudioSource.PlayClipAtPoint(lockedSound, transform.position, lockedVolume);
            }
            else
            { 
                cooldownTimer = cooldown;
                doneTime = true;
                if (interactionSound != null)
                {
                    AudioSource.PlayClipAtPoint(interactionSound, transform.position,interactionSoundVolume);
                }
            }
        }

        public virtual void OnInteractionStay()
        {

        }

        public virtual void OnInteractionExit()
        {

        }

        public virtual void OnTouchEnter(Collision interactorCollision, GameObject interactor)
        {

            if (touchSound != null)
            {
                AudioSource.PlayClipAtPoint(touchSound, transform.position, touchSoundVolume);
            }
        }

        public virtual void OnTouchStay(Collision interactorCollision, GameObject interactor)
        {

        }

        public virtual void OnTouchExit(Collision interactorCollision, GameObject interactor)
        {

        }

        public virtual void OnTouchTriggerEnter(Collider other, GameObject interactor)
        {
            
            if (touchSound != null)
            {
                AudioSource.PlayClipAtPoint(touchSound, transform.position, touchSoundVolume);
            }
        }

        public virtual void OnTouchTriggerStay(Collider other, GameObject interactor)
        {

        }

        public virtual void OnTouchTriggerExit(Collider other, GameObject interactor)
        {

        }


        public void SetInteractable(bool status)
        {
            isInteractable = status;
        }

        public bool IsInteractable() {
            return isInteractable;
        }

        bool RequisiteCheck()
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                if (Camera.main.GetComponent<Tools.Eye>() == null)
                {
                    print("<color=olive>Warning! Cannot find the player's eye</color>\nObject tagged 'Player' does not have a 'Tools.Eye' script component");
                    return false;
                }
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Tools.Toucher>() == null)
                {
                    print("<color=olive>Warning! Cannot find the player's hands</color>\nObject tagged 'Player' does not have a 'Tools.Toucher' script component");
                    return false;
                }
            }
            else
            {
                print("<color=olive>Warning! Cannot find player</color>\nNo player is present or is not tagged 'Player'");
                return false;
            }
            return true;
        }

    }
}