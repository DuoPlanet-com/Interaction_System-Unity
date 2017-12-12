using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(Camera))]
    public class Eye : MonoBehaviour {

        BaseClasses.Watchable target;
        BaseClasses.Watchable lastTarget;

        public bool tutorialMode = false;

        public InteractionCrosshairFader crosshair;


        // Update is called once per frame
        void Update () {
            See();
        }

        public BaseClasses.Watchable LookingAt()
        {
            RaycastHit hit;
            GameObject result = null;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.gameObject.GetComponent<Abstracts.Interactable>() != null)
                {
                    result = hit.collider.gameObject;
                    return new BaseClasses.Watchable(result, Vector3.Distance(transform.position, result.transform.position), hit);
                }
            }
            return new BaseClasses.Watchable(result, 0f, hit);
        }

        bool LookAtEnter(BaseClasses.Watchable toInteractWith)
        {

            if (!toInteractWith.isNull())
            {
                Abstracts.Interactable[] components = toInteractWith.gameObject.GetComponents<Abstracts.Interactable>();
                foreach (Abstracts.Interactable component in components)
                {
                    if (component.IsWithinRange(transform.parent.gameObject))
                    {
                        component.OnWatchEnter();
                        crosshair.highlight = true;
                        return true;
                    }
                }
            }
            crosshair.highlight = false;
            return false;
        }

        bool LookAtStay(BaseClasses.Watchable toInteractWith)
        {
            if (!toInteractWith.isNull())
            {
                Abstracts.Interactable[] components = toInteractWith.gameObject.GetComponents<Abstracts.Interactable>();
                foreach (Abstracts.Interactable component in components)
                {
                    if (component.IsWithinRange(transform.parent.gameObject))
                    {
                        component.OnWatchStay();
                        crosshair.highlight = true;
                        return true;
                    }
                }
            }
            crosshair.highlight = false;
            return false;
        }

        bool LookAtExit(BaseClasses.Watchable toInteractWith)
        {
            if (!toInteractWith.isNull())
            {
                Abstracts.Interactable[] components = toInteractWith.gameObject.GetComponents<Abstracts.Interactable>();
                foreach (Abstracts.Interactable component in components)
                {
                    if (component.IsWithinRange(transform.parent.gameObject))
                    {
                        component.OnWatchExit();

                        crosshair.highlight = false;
                        return true;
                    }
                }
            }
            crosshair.highlight = false;
            return false;
        }

        void interactWith(BaseClasses.Watchable toInteractWith)
        {

            if (!toInteractWith.isNull())
            {

                Abstracts.Interactable[] components = toInteractWith.gameObject.GetComponents<Abstracts.Interactable>();
                foreach (Abstracts.Interactable component in components)
                {

                    if (component.IsWithinRange(transform.parent.gameObject))
                    {
                        if (Input.GetButtonDown("Interact"))
                        {

                            component.OnInteractionEnter();
                        }

                        if (Input.GetButton("Interact"))
                        {

                            component.OnInteractionStay();
                        }

                        if (Input.GetButtonUp("Interact"))
                        {

                            component.OnInteractionExit();
                        }
                    }
                }
            }
        }

        void See()
        {
            target = LookingAt();
            if (target != null)
            {
                if (lastTarget != null)
                {
                    if (target.isNull() && !lastTarget.isNull())
                    {
                        LookAtExit(lastTarget);
                        lastTarget = null;
                    }
                }
                if (LookAtStay(target))
                {
                    if (lastTarget != null) { 
                        if (target.gameObject != lastTarget.gameObject) {
                            LookAtEnter(target);
                            LookAtExit(lastTarget);
                            lastTarget = target;
                        }
                    } else
                    {
                        LookAtEnter(target);
                        lastTarget = target;
                    }
                }
                interactWith(target);
            } else
            {
                if (lastTarget != null)
                {
                    LookAtExit(lastTarget);
                    lastTarget = null;
                }
            }
        }
        

    }

}