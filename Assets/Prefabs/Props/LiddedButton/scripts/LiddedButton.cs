using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiddedButton : Abstracts.Interactable {

    bool lidOpen = false;

    public override void OnInteractionEnter()
    {
        base.OnInteractionEnter();

        if (!lidOpen) { 
            GetComponent<Animator>().SetBool("isOpen",true);
        } else {
            GetComponent<Animator>().SetTrigger("isPressing");
            if (GetComponent<LiddedButtonEvent>() != null)
            {
                foreach (var eventx in GetComponents<LiddedButtonEvent>())
                {
                    eventx.PressEvent();
                }
            }
        }

    }

    public void OpenLid()
    {
        lidOpen = true;
    }

    public void unTrigger()
    {
        GetComponent<Animator>().ResetTrigger("isPressing");
    }

}
