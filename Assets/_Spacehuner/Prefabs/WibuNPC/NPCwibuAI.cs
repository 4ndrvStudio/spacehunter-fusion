using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCwibuAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private List<Transform> _paths = new List<Transform>();
    [SerializeField] private float _moveDuration = 150f;
    [SerializeField] private float distanceStop = 3f;
    [SerializeField] private Animator anim;

    private List<Vector3> _pathWays = new List<Vector3>();

    private Transform nextPos;

    GameObject _player;

    [Header("Bubble chat")]
    [Multiline]
    public string[] thingsToSay = new string[] { "Hello world" };
    public Transform mouthAI;
    public float timeBetweenSpeak = 5f;
    private float timeToNextSpeak;
    // Start is called before the first frame update
    void Awake()
    {
        _paths.ForEach(path => _pathWays.Add(path.position));

    }
    private void Start()
    {
        timeToNextSpeak = timeBetweenSpeak;
        PlayDOPath();
    }
    void PlayDOPath()
    {
        transform.DOPath(_pathWays.ToArray(), _moveDuration, PathType.Linear, PathMode.Full3D).SetLoops(-1).SetEase(Ease.Linear).OnWaypointChange(waypoint => RotatePlayer(waypoint));
    }
    private void RotatePlayer(int waypointIndex)
    {
        nextPos = _paths[waypointIndex];
    }
    private void Update()
    {
       timeToNextSpeak -= Time.deltaTime;
        var players = GameObject.FindGameObjectsWithTag("Player");

      
        if (_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) <= distanceStop)
            {
                anim.SetBool("Idle", true);
                DOTween.Pause(transform);
                var offset = _player.transform.position - transform.position;
                offset.y = 0f;
                var rotationAngle = Quaternion.LookRotation(offset);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 5);
                if (timeToNextSpeak <= 0 && thingsToSay.Length > 0)
                    SaySomething();
            }
            else
            {
                var rotationAngle = Quaternion.LookRotation(nextPos.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 5);
                DOTween.Play(transform);
                anim.SetBool("Idle", false);
            }
        }
      
    }
    public void SaySomething()
    {
       
        string message = thingsToSay[Random.Range(0, thingsToSay.Length)];
        SaySomething(message);
        Debug.Log("haha");
    }

    public void SaySomething(string message)
    {
        SaySomething(message, SpeechBubbleManager.Instance.GetRandomSpeechbubbleType());
    }

    public void SaySomething(string message, SpeechBubbleManager.SpeechbubbleType speechbubbleType)
    {
        if (mouthAI == null)
            SpeechBubbleManager.Instance.AddSpeechBubble(transform, message, speechbubbleType);
        else
            SpeechBubbleManager.Instance.AddSpeechBubble(mouthAI, message, speechbubbleType);

        timeToNextSpeak = timeBetweenSpeak;
    }
}
