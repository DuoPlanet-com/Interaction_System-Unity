using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools { 
    public sealed class TurnTowards : MonoBehaviour {

        [Tooltip("The target to rotate towards")]
        public Transform target;

        [Tooltip("Should the object be locked into position upon start?")]
        public bool startLocked = false;



        [System.Serializable]
        public class MotionSettingsContainer
        {
            [Tooltip("Using NONE will ignore the damping setting")]
            public TurnMode turnMode;

            [Tooltip("The smoothness of the turning motion")]
            public float speed = 1;

        }
        public MotionSettingsContainer motionSettings = new MotionSettingsContainer();


        [System.Serializable]
        public class ClampSettingsContainer
        {
            [Tooltip("Should the rotation be clamped?")]
            public bool clampRotation = false;

            public Vector3 clampMin;
            public Vector3 clampMax;

        }
        public ClampSettingsContainer clampSettings = new ClampSettingsContainer();


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
            switch(motionSettings.turnMode)
            {
                case TurnMode.NONE:
                    transform.rotation = rotation;
                    break;
                case TurnMode.LERP:
                    transform.rotation = Quaternion.Lerp
                        (transform.rotation, rotation, motionSettings.speed / 100);
                    break;
                case TurnMode.SLERP:
                    transform.rotation = Quaternion.Slerp
                        (transform.rotation, rotation, motionSettings.speed / 100);
                    break;
            }
        }

        Quaternion NewRotation ()
        {
            targetPos = target.position;
            Quaternion newRot = Quaternion.LookRotation(targetPos - transform.position);

            if (clampSettings.clampRotation)
            {
                Vector3 newRotEuler = new Vector3( 
                    Mathf.Clamp( newRot.eulerAngles.x , clampSettings.clampMin.x , clampSettings.clampMax.x ), 
                    Mathf.Clamp( newRot.eulerAngles.y , clampSettings.clampMin.y , clampSettings.clampMax.y ), 
                    Mathf.Clamp( newRot.eulerAngles.z , clampSettings.clampMin.z , clampSettings.clampMax.z ));

                newRot = Quaternion.Euler(newRotEuler);
            }

            return newRot;
        }

    }
}