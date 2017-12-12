using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables { 
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class FallOnEvent : Abstracts.Interactable {

        [Header("FallOnEvent Settings")]
        [Tooltip("What event should activate the timer?")]
        public ActivationMethod activateBy;

        [Tooltip("Should the object remain in its place if forces act on them?")]
        public bool useKinematic = true;
        [Space(10)]
        [Tooltip("The sound that will player after the timer runs out and the object falls")]
        public AudioClip fallSound;
        public float fallVolume = 1;
        [Space(10)]
        [Tooltip("The timer which will after activation make the object turn gravity on\nIn seconds")]
        public float timer = 1;
        [Tooltip("For how long time should the force be added after falling?")]
        public float forceTimer = 0;
        [Tooltip("Ignore the forceTimer and only add force on first frame after the timer runs out")]
        public bool forceOnce = true;
        public bool resetAfterForce = false;

        [Space(10)]
        [Tooltip("In what space should force be added after the timer runs out?")]
        public Space space = Space.World;
        [Tooltip("In what direction should the force be added after the timer runs out?")]
        public Vector3 force = Vector3.up;
        [Tooltip("How much should the force be multiplied with after the timer runs out?")]
        public float forceMultiplier = 1;

        bool doneForce = false;

        bool isStarted = false;

        Rigidbody rb;

        float resetTimer;
        float resetForceTimer;

        public override void OnStart()
        {
            base.OnStart();
            rb = GetComponent<Rigidbody>();
            
            resetTimer = timer;
            resetForceTimer = forceTimer;

            ResetAll();

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isStarted)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                if (useKinematic)
                {
                    rb.isKinematic = false;
                }
                rb.useGravity = true;
                forceTimer -= Time.deltaTime;
                if ((forceTimer > 0 && !forceOnce) || (!doneForce && forceOnce))
                {
                    addForce();
                }
                if (forceTimer <= 0 && resetAfterForce)
                {
                    ResetAll();
                }
            }
        }

        public override void OnInteractionEnter()
        {
            base.OnInteractionEnter();

            if (IsInteractable()) { 
                if (activateBy == ActivationMethod.INTERACT)
                {
                    isStarted = true;
                }
            }
        }

        public override void OnTouchEnter(Collision interactorCollision, GameObject interactor)
        {
            base.OnTouchEnter(interactorCollision, interactor);
            if (IsInteractable())
            {
                if (activateBy == ActivationMethod.TOUCH)
                {
                    isStarted = true;
                }
            }
        }

        public override void OnTouchTriggerEnter(Collider other, GameObject interactor)
        {
            base.OnTouchTriggerEnter(other, interactor);
            if (IsInteractable())
            {
                if (activateBy == ActivationMethod.TRIGGER)
                {
                    isStarted = true;
                }
            }
        }

        public override void OnWatchEnter()
        {
            base.OnWatchEnter();
            if (IsInteractable())
            {
                if (activateBy == ActivationMethod.WATCH)
                {
                    isStarted = true;
                }
            }
        }

    
        void addForce()
        {
            if (space == Space.Self)
            {
                rb.AddRelativeForce(force * forceMultiplier * Time.deltaTime * 100 );
            }
            else
            {
                rb.AddForce(force * forceMultiplier * Time.deltaTime * 100);
            }
            doneForce = true;

        }

        void ResetAll()
        {
            timer = resetTimer;
            forceTimer = resetForceTimer;

            doneForce = false;
            isStarted = false;
            if (useKinematic)
            {
                rb.isKinematic = true;
            }
            rb.useGravity = false;
        }

    }
}