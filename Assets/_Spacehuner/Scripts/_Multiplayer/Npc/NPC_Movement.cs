using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


namespace SH.Multiplayer
{



    public class NPC_Movement : MonoBehaviour
    {


        [SerializeField] private NPC_Brain _NPC_Brain;
        [SerializeField] private Transform _body;

        [System.Serializable]
        public class MovingPathContainer
        {
            public PathType MovingType;
            public Transform PathContainer;
        }

        [SerializeField] private List<MovingPathContainer> _movingPathContainers = new List<MovingPathContainer>();

        [SerializeField] private float _moveSpeed = 1.2f;

        [SerializeField] private float _idleDuration = 2f;

        [SerializeField] private int _pathIndex = 0;
        
        [SerializeField] private bool _canIdle = false;
        [SerializeField] private bool _isRightWay = false;
        [SerializeField] private bool _isIdle = false;

        public void Start()
        {
            MovingNextState();
        }


        private void Update()
        {

            if (_isIdle)
            {
                _NPC_Brain.NpcState = NPCState.Idle;
            }
            else
            {
                _NPC_Brain.NpcState = NPCState.Walking;

            }
        }

        private void MovingNextState()
        {
            Debug.Log("Moving next State Called");
            PathType targetPathType = _movingPathContainers[_pathIndex].MovingType;

            Transform[] waypoints = _movingPathContainers[_pathIndex].PathContainer.GetComponentsInChildren<Transform>();

            Vector3[] pathPoints = new Vector3[waypoints.Length - 1];

            

            for (int i = 1; i < waypoints.Length; i++)
            {
                pathPoints[i - 1] = waypoints[i].position;
            }

            if(_isRightWay) Array.Reverse(pathPoints);

            switch (targetPathType)
            {
                case PathType.Linear:
                    MovingNPC(pathPoints, targetPathType);
                    break;
                case PathType.CatmullRom:
                    MovingNPC(pathPoints, targetPathType);
                    break;
            }

        }

        private void MovingNPC(Vector3[] path, PathType targetPath)
        {
            transform.DOPath(path, _moveSpeed, targetPath, PathMode.Full3D)
                .SetEase(Ease.Linear)
                .SetLookAt(0.01f)
                .SetSpeedBased()
                .SetOptions(AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
                .OnComplete(() =>
                {
                    _pathIndex += 1;
                    if (_pathIndex >= _movingPathContainers.Count) _pathIndex = 0;

                    int randomFactor = UnityEngine.Random.Range(0, 8);

                    if (randomFactor <= 2 && _canIdle) 
                        StartCoroutine(Idle());
                    else
                        MovingNextState();

                })
                .SetAutoKill(true);
        }



        private IEnumerator Idle()
        {
            _isIdle = true;
            yield return new WaitForSeconds(_idleDuration);
            _isIdle = false;
            MovingNextState();

        }

    }

}
