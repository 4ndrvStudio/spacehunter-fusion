
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NpcAI : MonoBehaviour
{
    [SerializeField] private List<Transform> _paths = new List<Transform>();
    [SerializeField] private float _moveDuration = 150f;

    private Transform nextPos;
    private List<Vector3> _pathWays = new List<Vector3>();


    // Start is called before the first frame update
    void Awake()
    {
        _paths.ForEach(path => _pathWays.Add(path.position));
          
    }
    private void Start() {

        transform.DOPath(_pathWays.ToArray(), _moveDuration, PathType.Linear, PathMode.Full3D).SetLoops(-1).SetEase(Ease.Linear).OnWaypointChange(waypoint => RotatePlayer(waypoint));        
    }

    private void RotatePlayer(int waypointIndex) {
        nextPos = _paths[waypointIndex];

    }
    private void Update()
    {
        var rotationAngle = Quaternion.LookRotation(nextPos.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 5);
    }
}
