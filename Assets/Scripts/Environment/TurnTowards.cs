using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools { 
    public class TurnTowards : MonoBehaviour {

        [Tooltip("The target to rotate towards")]
        public Transform target;

        [Tooltip("Using NONE will ignore the damping setting")]
        public TurnMode turnMode;

        [Tooltip("The smoothness of the turning motion")]
        public float speed = 1;

        [Tooltip("Should the object be locked into position upon start?")]
        public bool startLocked = false;

        [Tooltip("Should the rotation be clamped?")]
        public bool clampRotation = false;

        public Vector3 clampMin;
        public Vector3 clampMax;

        Vector3 targetPos;

        bool isLocked;

        public enum TurnMode
        {
            NONE,LERP,SLERP
        }

        private void Start()
        {
            if (startLocked)
            {
                isLocked = true;
            } else
            {
                isLocked = false;
            }
        }

        // Update is called once per frame
        void Update () {
            if (!isLocked) { 
               Rotate( NewRotation() );
            }
        }
        
        public void Lock(bool status)
        {
            isLocked = status;
        }


        void Rotate(Quaternion rotation)
        {
            switch(turnMode)
            {
                case TurnMode.NONE:
                    transform.rotation = rotation;
                    break;
                case TurnMode.LERP:
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed / 100);
                    break;
                case TurnMode.SLERP:
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed / 100);
                    break;
            }
        }

        Quaternion NewRotation ()
        {
            targetPos = target.position;
            Quaternion newRot = Quaternion.LookRotation(targetPos - transform.position);

            if (clampRotation)
            {

                Vector3 newRotEuler = new Vector3( Mathf.Clamp( newRot.eulerAngles.x , clampMin.x, clampMax.x ) , Mathf.Clamp(newRot.eulerAngles.y, clampMin.y, clampMax.y), Mathf.Clamp(newRot.eulerAngles.z, clampMin.z, clampMax.z) );
                newRot = Quaternion.Euler(newRotEuler);
            }

            return newRot;
        }

    }
}