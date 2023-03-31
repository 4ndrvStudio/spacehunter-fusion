using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SH.Multiplayer
{
    public class NPC_Movement : MonoBehaviour
    {
        public NPC_Brain _NPC_Brain;
             public Transform Body;
        public Transform pathContainer;
        public float moveDuration = 5f;
        public float idleDuration = 2f;
   

        private Tweener pathTween;
        private bool isIdle = false;

        private Vector3 currentPos;

        private bool isRewalk;

        public void Start()
        {

            Transform[] waypoints = pathContainer.GetComponentsInChildren<Transform>();


            Vector3[] pathPoints = new Vector3[waypoints.Length - 1];

            for (int i = 1; i < waypoints.Length; i++)
            {
                pathPoints[i - 1] = waypoints[i].position;
            }


            pathTween = transform.DOPath(pathPoints, moveDuration, PathType.CatmullRom, PathMode.Full3D)
                .SetEase(Ease.Linear)
                .SetLookAt(0.01f)
                .OnWaypointChange((int index) =>
                {   
                    if(index == pathPoints.Length) {
                        Body.transform.localEulerAngles = Body.transform.localEulerAngles + 180f * Vector3.up;
                    } else if( index == 0) {
                        Body.transform.localEulerAngles = 0 * Vector3.up;
                    }

                    int randomfactor = Random.Range(0,6);
                    if(randomfactor < 2) isIdle = true;
                    
                })
                .OnStepComplete(() =>
                {   
                    isIdle = true;
                    
                })
                .SetLoops(-1,LoopType.Yoyo)
                .Pause();
                
        }

        private void Update()
        {
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0,0);

            if (isIdle)
            {
                _NPC_Brain.NpcState = NPCState.Idle;
                StartCoroutine(Idle());

                
            }
            else
            {
                _NPC_Brain.NpcState = NPCState.Walking;
                pathTween.PlayForward();
            }
        }

        private IEnumerator Idle()
        {
            pathTween.Pause();
            yield return new WaitForSeconds(idleDuration);
            isIdle = false;
            pathTween.PlayBackwards();
        }

    }

}
