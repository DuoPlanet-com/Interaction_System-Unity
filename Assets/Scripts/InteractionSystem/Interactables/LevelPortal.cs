using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables { 
    [RequireComponent(typeof(Collider))]
    public class LevelPortal : Abstracts.Interactable {

        [Tooltip("Leave at 0 if this is the entry of a level.")]
        public int sceneIndexToLoad;
        [Space(10)]
        [Tooltip("Check this, if this is the entry of a level.")]
        public bool isEntryPortal;

        [Tooltip("The time it takes for the entry door to open\nLeave at 0 if this is the exit of a level.")]
        [Space(10)]
        public float timeToOpenDoor = 2;

        
        [Space(10)]
        [Tooltip("Should be set to the object that will be obstructing the players' view upon entering a level\nLeave blank if NOT entry portal.")]
        public GameObject doorToOpen;

        bool isDoorTimerStarted;

        public override void OnStart()
        {
            base.OnStart();
            isDoorTimerStarted = false;

            CheckEditorSettings();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isDoorTimerStarted)
                timeToOpenDoor -= Time.deltaTime;

            if (timeToOpenDoor <= 0)
            {
                doorToOpen.SetActive(false);
            }
        }

        public override void OnTouchEnter(Collision interactorCollision, GameObject interactor)
        {
            base.OnTouchEnter(interactorCollision, interactor);
            if (interactor.tag == "Player")
            {
                if (!isEntryPortal)
                {
                    SceneManager.LoadSceneAsync(sceneIndexToLoad);

                    print("<color=blue>Notice: Player entered exit trigger collider </color>\nAttempting to load new scene @ " + sceneIndexToLoad);
                }
                else
                {
                    isDoorTimerStarted = true;

                    print("<color=blue>Notice: Player entered entry trigger collider </color>\nAttempting to remove door in " + timeToOpenDoor + " seconds");
                }
            }
        }

        void CheckEditorSettings()
        {
            if (doorToOpen != null)
            {
                if (!isEntryPortal)
                {
                    print("<color=olive>Warning! Portal is not entry, yet a door to open has been set</color>\nAre you sure this isn't an entry portal?");
                }
            }
            if (sceneIndexToLoad == 0)
            {
                print("<color=olive>Warning! variable levelToLoad is equal to 0</color>\nVariable may not have been set or is loading first scene");
            }
        }

    }
}