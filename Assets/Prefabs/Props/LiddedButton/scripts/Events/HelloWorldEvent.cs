using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorldEvent : LiddedButtonEvent {

    public override void PressEvent()
    {
        base.PressEvent();
        print("Hello world!");
    }
}
