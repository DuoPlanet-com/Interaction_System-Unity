using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseClasses { 
    public class Watchable {

        public GameObject gameObject;
        public float distance;
        public RaycastHit hitInfo;

	    public Watchable(GameObject obj,float dist, RaycastHit hit)
        {
            distance = dist;
            gameObject = obj;
            hitInfo = hit;
        }

        public bool isNull()
        {
            if (gameObject == null)
                return true;
            return false;
        }

    }
}