using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SH.NPC
{
    public class NPC_StairMoving : MonoBehaviour
    {
        public Transform pathContainer;
        [SerializeField] private int StairPoint = 0;


        // Start is called before the first frame update
        void Start()
        {  
            Transform[] waypoints = pathContainer.GetComponentsInChildren<Transform>();

            Vector3[] pathPoints = new Vector3[waypoints.Length - 1];

            for (int i = 1; i < waypoints.Length; i++)
            {
                pathPoints[i - 1] = waypoints[i].position;
            }
            Sequence movementSequence = DOTween.Sequence();

            transform.DOPath(pathPoints,1.2f,PathType.Linear,PathMode.Full3D)
                .SetEase(Ease.Linear)
                .SetLookAt(0.01f)
                .SetSpeedBased()
                .SetOptions(AxisConstraint.None ,AxisConstraint.Z | AxisConstraint.X);
          
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
