using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstracts {
    [RequireComponent(typeof(Collider))]
    public abstract class Interactable : MonoBehaviour {

        

        [System.Serializable]
        public class InteractionSettingsContainer
        {
            [Tooltip("Within what range should the player be able to interact with this object?")]
            public float interactionRange = 5;

            [Space(7)]
            public float cooldown = 0;

            [Space(7)]
            public bool oneTime = false;
            public bool startUnlocked = true;

            [Space(7)]
            [Tooltip("Used for debugging purposes")]
            public bool printDistance = false;
        }
        public InteractionSettingsContainer interactionSettings = new InteractionSettingsContainer();


        [System.Serializable]
        public class AudioSettingsContainer
        {
            public AudioClip interactionSound;
            public float interactionSoundVolume = 1;
            public AudioClip watchSound;
            public float watchSoundVolume = 1;
            public AudioClip touchSound;
            public float touchSoundVolume = 1;
            [Space(10)]

            public AudioClip lockedSound;
            public float lockedVolume = 1;
        }
        public AudioSettingsContainer audioSettings = new AudioSettingsContainer();

        [System.Serializable]
        protected class InternalsContainer
        {
            public bool doneTime = false;

            public bool isInteractable;

            public float cooldownTimer;
        }

        [Space(10)]
        [SerializeField]
        protected InternalsContainer internals = new InternalsContainer();

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

            if (interactionSettings.startUnlocked)
            {
                internals.isInteractable = true;
            } else
            {
                internals.isInteractable = false;
            }
            internals.cooldownTimer = interactionSettings.cooldown;
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

            if (internals.cooldownTimer <= 0 && !(!internals.doneTime && interactionSettings.oneTime))
            {
                float distance = Vector3.Distance(interactor.transform.position, transform.position);
                if (interactionSettings.printDistance)
                {
                    print(distance);
                }
                if (Vector3.Distance(interactor.transform.position, transform.position) <= interactionSettings.interactionRange)
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
            internals.cooldownTimer -= Time.deltaTime;
        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnWatchEnter()
        {
          
            if (audioSettings.watchSound != null)
            {
                AudioSource.PlayClipAtPoint(audioSettings.watchSound, transform.position, audioSettings.watchSoundVolume);
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

            if (internals.isInteractable == false)
            {
                AudioSource.PlayClipAtPoint(audioSettings.lockedSound, transform.position, audioSettings.lockedVolume);
            }
            else
            {
                internals.cooldownTimer = interactionSettings.cooldown;
                internals.doneTime = true;
                if (audioSettings.interactionSound != null)
                {
                    AudioSource.PlayClipAtPoint(audioSettings.interactionSound, transform.position, audioSettings.interactionSoundVolume);
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

            if (audioSettings.touchSound != null)
            {
                AudioSource.PlayClipAtPoint(audioSettings.touchSound, transform.position, audioSettings.touchSoundVolume);
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
            
            if (audioSettings.touchSound != null)
            {
                AudioSource.PlayClipAtPoint(audioSettings.touchSound, transform.position, audioSettings.touchSoundVolume);
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
            internals.isInteractable = status;
        }

        public bool IsInteractable() {
            return internals.isInteractable;
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