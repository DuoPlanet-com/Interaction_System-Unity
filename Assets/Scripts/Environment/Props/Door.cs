using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
    public class Door : MonoBehaviour {

        [Header("Door settings")]
        [Tooltip("The axis with which to rotate around")]
        public Vector3 axis = Vector3.up;

        [Tooltip("The degrees with which to rotate\nOr if sliding the amount of distance it to slide with")]
        public float Amount = 90;

        [Tooltip("The object which to rotate around\n\nNOTE: leave blank if itself")]
        public Transform pivotPoint;

        [Space(10)]
        [Tooltip("The smoothing multiplier of the opening motion\n\nHigher is faster\nLower is slower")]
        public float smooth = 1;

        [Tooltip("This is only used in dampened interpolation")]
        public float time = 1;

        [Space(10)]
        [Tooltip("Is this a sliding door? (doesn't rotate)")]
        public bool isSliding = false;

        [Tooltip("How should the movement be interpolated?\n\n Note: Dampened does not work if not sliding, using SLERP instead")]
        public InterpolationMode interpolation = InterpolationMode.LERP;

        [Space(10)]
        public AudioClip openSound;
        public float openVolume;
        public AudioClip closeSound;
        public float closeVolume;

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

            newSlidePos = transform.position + (axis * Amount);
            originalPos = transform.position;
            targetPos = originalPos;
            if (!isSliding) { 

                if (interpolation == InterpolationMode.DAMPENED)
                {
                    print("<color=olive>Warning! Cannot use dampened interpolation, using slerp instead</color>\nNon-sliding doors does not support dampened interpolation");
                }

                if (pivotPoint == null)
                {
                    pivotPoint = transform;
                } else
                {
                    pivotPoint.parent = transform.parent;
                    transform.parent = pivotPoint;
                }
                targetRotation = transform.rotation;
            }
        }

        private void Update()
        {
            if (isSliding)
            {
                switch(interpolation)
                {
                    case InterpolationMode.LERP:
                        transform.position = Vector3.Lerp(transform.position, targetPos, 5 * smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.SLERP:
                        transform.position = Vector3.Slerp(transform.position, targetPos, 5 * smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.DAMPENED:
                        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity , 5 * smooth * Time.deltaTime,time * Time.deltaTime);
                        break;
                    case InterpolationMode.NONE:
                        transform.position = targetPos;
                        break;
                }
            } else {
                switch (interpolation)
                {
                    case InterpolationMode.LERP:
                        pivotPoint.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.SLERP:
                        pivotPoint.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.DAMPENED:
                        pivotPoint.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * smooth * Time.deltaTime);
                        break;
                    case InterpolationMode.NONE:
                        pivotPoint.rotation = targetRotation;
                        break;
                }
            }
        }

        public void Open()
        {
            PlaySound(negative);

            if (isSliding)
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
                targetRotation *= Quaternion.AngleAxis(-Amount, axis);
                negative = false;
            }
            else
            {
                negative = true;
                targetRotation *= Quaternion.AngleAxis(Amount, axis);
            }
        }

        void PlaySound(bool neg)
        {
            
            if (neg) { 
                if (closeSound != null) { 
                    AudioSource.PlayClipAtPoint(closeSound, transform.position, closeVolume);
                }
            } else { 
                if (openSound != null) { 
                    AudioSource.PlayClipAtPoint(openSound, transform.position, openVolume);
                }
            }
        }

    }
}