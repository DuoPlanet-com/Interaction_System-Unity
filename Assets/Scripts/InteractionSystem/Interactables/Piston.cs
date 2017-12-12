using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables { 
    public class Piston : Abstracts.Interactable
    {

        [Header("Piston settings")]
        [Tooltip("The object to push")]
        public Transform piston;

        [Tooltip("In what direction and how far should the piston be pushed?")]
        public Vector3 pistonOffset = new Vector3(0,-1,0);

        [Tooltip("How quickly should the piston be pushed / detracted?\n\nIF USING SMOOTH\nHigher means slower\n\nIF NOT USING SMOOTH\nHigher means faster")]
        public float time = 2;

        [Tooltip("How sensitive should the isUp and isDown variables be?\n\nAt 0 the piston must be exactly at the up or down position, this must be greater than 0 if using smooth")]
        public float upDownSensitivity = .1f;

        [Tooltip("Should the piston movement be dampened?\n\nTrue : Vector3.SmoothDamp\nFasle:Vector3.Lerp")]
        public bool smooth = true;

        public bool startDown = false;

        public ActivationMethod activateBy = ActivationMethod.INTERACT;

        [Header("Piston internals")]
        [SerializeField]
        bool isUp;
        [SerializeField]
        bool isDown;
        [SerializeField]
        float upDistance;
        [SerializeField]
        float downDistance;

        Vector3 upPos;
        Vector3 downPos;
        Vector3 targetPos;
        Vector3 velocity = Vector3.zero;


        public override void OnStart()
        {
            base.OnStart();
            SetSettings();
            if (startDown)
            {
                Pull();
            }

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            Move();
            CheckPosition();
        }

        public override void OnInteractionEnter()
        {
            base.OnInteractionEnter();
            if (activateBy == ActivationMethod.INTERACT) { 
                if (IsInteractable())
                {
                    if (targetPos == downPos)
                    {
                        Push();
                    }
                    else
                    {
                        Pull();
                    }
                }
            }
        }


        public override void OnWatchEnter()
        {
            base.OnWatchEnter();
            if (activateBy == ActivationMethod.WATCH)
            {
                if (IsInteractable())
                {
                    if (targetPos == downPos)
                    {
                        Push();
                    }
                    else
                    {
                        Pull();
                    }
                }
            }
        }

        public override void OnTouchEnter(Collision interactorCollision, GameObject interactor)
        {
            base.OnTouchEnter(interactorCollision, interactor);
            if (activateBy == ActivationMethod.TOUCH)
            {
                if (IsInteractable())
                {
                    if (targetPos == downPos)
                    {
                        Push();
                    }
                    else
                    {
                        Pull();
                    }
                }
            }
        }


        public void Push()
        {
            targetPos = upPos;
        }

        public void Pull()
        {
            targetPos = downPos;
        }

        public bool IsUp()
        {
            return isUp;
        }

        public bool IsDown()
        {
            return isDown;
        }

        public float DownDistance()
        {
            return downDistance;
        }

        public float UpDistance()
        {
            return upDistance;
        }


        void SetSettings()
        {
            upPos = piston.position;
            downPos = new Vector3(
                piston.position.x + pistonOffset.x,
                piston.position.y + pistonOffset.y,
                piston.position.z + pistonOffset.z);
            targetPos = upPos;
        }

        void Move()
        {
            if (smooth)
            {
                piston.position = Vector3.SmoothDamp
                    (piston.position, targetPos, ref velocity, time * Time.deltaTime);
            }
            else
            {
                piston.position = Vector3.Lerp
                    (piston.position, targetPos, time * Time.deltaTime);
            }
        }
        
        void CheckPosition()
        {
            upDistance = Vector3.Distance(piston.position, upPos);
            downDistance = Vector3.Distance(piston.position, downPos);
            
            if (upDistance <= upDownSensitivity)
                isUp = true;
            else
                isUp = false;

            if (downDistance <= upDownSensitivity)
                isDown = true;
            else
                isDown = false;
        }

    }
}