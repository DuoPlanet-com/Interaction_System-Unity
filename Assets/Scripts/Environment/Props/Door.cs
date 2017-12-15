    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
    public class Door : MonoBehaviour {

       

        [System.Serializable]
        public class DoorSettingsContainer
        {
            [Tooltip("The axis with which to rotate around")]
            public Vector3 axis = Vector3.up;

            [Tooltip("The object which to rotate around\n\nNOTE: leave blank if itself")]
            public Transform pivotPoint;

            [Space(10)]

            [Tooltip("The degrees with which to rotate\nOr if sliding the amount of distance it to slide with")]
            public float Amount = 90;


        }
        public DoorSettingsContainer doorSettings = new DoorSettingsContainer();

        [System.Serializable]
        public class MotionSettingsContainer
        {

            [Tooltip("Is this a sliding door? (doesn't rotate)")]
            public bool isSliding = false;

            [Tooltip("How should the movement be interpolated?\n\n Note: Dampened does not work if not sliding, using SLERP instead")]
            public InterpolationMode interpolation = InterpolationMode.LERP;

            [Space(10)]

            [Tooltip("The smoothing multiplier of the opening motion\n\nHigher is faster\nLower is slower")]
            public float smooth = 1;

            [Tooltip("This is only used in dampened interpolation")]
            public float time = 1;


            
            
        }
        public MotionSettingsContainer motionSettings = new MotionSettingsContainer();


        [System.Serializable]
        public class AudioSettingsContainer
        {
            public AudioClip openSound;
            public float openVolume;

            [Space(10)]

            public AudioClip closeSound;
            public float closeVolume;
        }
        public AudioSettingsContainer audioSettings = new AudioSettingsContainer();


        

        [Header("Door internals")]
        [SerializeField]
        bool negative = false;

        Vector3 targetPos;
        Vector3 originalPos;
        Vector3 newSlidePos;

        Quaternion targetRotation;

        Vector3 velocity = Vector3.zero;

        public enum InterpolationMode
        {
            NONE , LERP , SLERP, DAMPENED
        }

        private void Start()
        {

            newSlidePos = transform.position + (doorSettings.axis * doorSettings.Amount);
            originalPos = transform.position;
            targetPos = originalPos;
            if (!motionSettings.isSliding) { 

                if (motionSettings.interpolation == InterpolationMode.DAMPENED)
                {
                    print("<color=olive>Warning! Cannot use dampened interpolation, using slerp instead</color>\nNon-sliding doors does not support dampened interpolation");
                }

                if (doorSettings.pivotPoint == null)
                {
                    doorSettings.pivotPoint = transform;
                } else
                {
                    doorSettings.pivotPoint.parent = transform.parent;
                    transform.parent = doorSettings.pivotPoint;
                }
                targetRotation = transform.rotation;
            }
        }

        private void Update()
        {
            if (motionSettings.isSliding)
            {
                switch(motionSettings.interpolation)
                {
                    case InterpolationMode.LERP:
                        transform.position = Vector3.Lerp(transform.position, targetPos, 5 * motionSettings.smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.SLERP:
                        transform.position = Vector3.Slerp(transform.position, targetPos, 5 * motionSettings.smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.DAMPENED:
                        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity , 5 * motionSettings.smooth * Time.deltaTime, motionSettings.time * Time.deltaTime);
                        break;
                    case InterpolationMode.NONE:
                        transform.position = targetPos;
                        break;
                }
            } else {
                switch (motionSettings.interpolation)
                {
                    case InterpolationMode.LERP:
                        doorSettings.pivotPoint.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * motionSettings.smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.SLERP:
                        doorSettings.pivotPoint.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * motionSettings.smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.DAMPENED:
                        doorSettings.pivotPoint.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * motionSettings.smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.NONE:
                        doorSettings.pivotPoint.rotation = targetRotation;
                        break;
                }
            }
        }

        public void Open()
        {
            PlaySound(negative);

            if (motionSettings.isSliding)
            {
                Slide();
            } else {
                Rotate();
            }
        }

        public bool IsOpen()
        {
            return negative;
        }

        void Slide()
        {
            if (negative)
            {
                targetPos = originalPos;
                negative = false;
            }
            else
            {
                targetPos = newSlidePos;
                negative = true;
            }
        }

        void Rotate()
        {
            if (negative)
            {
                targetRotation *= Quaternion.AngleAxis(-doorSettings.Amount, doorSettings.axis);
                negative = false;
            }
            else
            {
                negative = true;
                targetRotation *= Quaternion.AngleAxis(doorSettings.Amount, doorSettings.axis);
            }
        }

        void PlaySound(bool neg)
        {
            
            if (neg) { 
                if (audioSettings.closeSound != null) { 
                    AudioSource.PlayClipAtPoint(audioSettings.closeSound, transform.position, audioSettings.closeVolume);
                }
            } else { 
                if (audioSettings.openSound != null) { 
                    AudioSource.PlayClipAtPoint(audioSettings.openSound, transform.position, audioSettings.openVolume);
                }
            }
        }

    }
}