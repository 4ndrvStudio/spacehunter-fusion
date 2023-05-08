using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


namespace SH.NPC
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
        [SerializeField] private List<Transform> _movingRandomPathContainers = new List<Transform>();

        [SerializeField] private float _moveSpeed = 1.2f;

        [SerializeField] private float _idleDuration = 10f;

        [SerializeField] private int _pathIndex = 0;

        [SerializeField] private bool _canIdle = false;
        [SerializeField] private bool _isRightWay = false;
        [SerializeField] private bool _isIdle = false;
        [SerializeField] private bool _isRandomMoving = false;

        private Tween _movingRandomTween;
        private Tween _movingNextStateTween;




        void ExcuteMove() {
            if (_isRandomMoving)
            {
                MovingRandom();
            }
            else
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
            PathType targetPathType = _movingPathContainers[_pathIndex].MovingType;

            Transform[] waypoints = _movingPathContainers[_pathIndex].PathContainer.GetComponentsInChildren<Transform>();

            Vector3[] pathPoints = new Vector3[waypoints.Length - 1];

            for (int i = 1; i < waypoints.Length; i++)
            {
                pathPoints[i - 1] = waypoints[i].position;
            }

            if (_isRightWay) Array.Reverse(pathPoints);

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

        private void MovingRandom()
        {

            Transform targetPoint = _movingRandomPathContainers[UnityEngine.Random.Range(0, _movingRandomPathContainers.Count)];
            transform.LookAt(targetPoint);
            _movingRandomTween =
                transform.DOMove(targetPoint.position, _moveSpeed)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .OnComplete(() =>
                {
                    _idleDuration = UnityEngine.Random.Range(5, 15);
                    StartCoroutine(Idle());
                });
        

        }

        private void MovingNPC(Vector3[] path, PathType targetPath)
        {
            _movingNextStateTween =
                transform.DOPath(path, _moveSpeed, targetPath, PathMode.Full3D)
                .SetEase(Ease.Linear)
                .SetLookAt(0.01f)
                .SetSpeedBased()
                .SetOptions(AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
                .OnComplete(() =>
                {
                    _pathIndex += 1;
                    if (_pathIndex >= _movingPathContainers.Count) _pathIndex = 0;

                    if (_isRandomMoving)
                    {
                        StartCoroutine(Idle());
                    }
                    else
                    {
                        int randomFactor = UnityEngine.Random.Range(0, 8);
                        if (randomFactor <= 2 && _canIdle)
                            StartCoroutine(Idle());
                        else
                            MovingNextState();

                    }
                })
                .SetAutoKill(true);
 
        }



        private IEnumerator Idle()
        {
            _isIdle = true;
            yield return new WaitForSeconds(_idleDuration);
            _isIdle = false;

            ExcuteMove();

        }
        
        private void OnEnable() {
           ExcuteMove();
        }

        private void OnDisable()
        {
            _movingRandomTween.Kill(false);
            _movingNextStateTween.Kill(false);
            StopCoroutine(this.Idle());
            _isIdle = false;

            
        }

    }

}
