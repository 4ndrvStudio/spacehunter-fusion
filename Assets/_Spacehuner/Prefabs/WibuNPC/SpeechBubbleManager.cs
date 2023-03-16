using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class SpeechBubbleManager : MonoBehaviour
{
    public enum SpeechbubbleType
    {
        NORMAL,
        SERIOUS,
        ANGRY,
        THINKING,
    }
    [System.Serializable]
    public class SpeechbubblePrefab
    {
        public SpeechbubbleType type;
        public GameObject prefab;
    }

    [Header("Default settings:")]
    [FormerlySerializedAs("defaultColor")]
    [SerializeField]
    private Color _defaultColor = Color.white;

    [FormerlySerializedAs("defaultTimeToLive")]
    [SerializeField]
    private float _defaultTimeToLive = 1;

    [FormerlySerializedAs("sizeMultiplier")]
    [SerializeField]
    private float _sizeMultiplier = 1f;

    [SerializeField]
    private bool _isSingleton = true;

    [Header("Prefabs mapping to each type:")]
    [FormerlySerializedAs("prefabs")]
    [SerializeField]
    private List<SpeechbubblePrefab> _prefabs;

    private Dictionary<SpeechbubbleType, GameObject> _prefabsDict = new Dictionary<SpeechbubbleType, GameObject>();
    private Dictionary<SpeechbubbleType, Queue<SpeechBubble>> _speechBubbleQueueDict = new Dictionary<SpeechbubbleType, Queue<SpeechBubble>>();

    [SerializeField]
    private Camera _cam;

    private static SpeechBubbleManager _instance;
    public static SpeechBubbleManager Instance
    {
        get
        {
            UnityEngine.Assertions.Assert.IsNotNull(_instance, "The static variable for Instance has not been set. Did you do this call before Awake() has finished or unchecked \"Is Singleton\" maybe?");
            return _instance;
        }
    }
    public Camera Cam
    {
        get
        {
            return _cam;
        }

        set
        {
            _cam = value;
            foreach (var bubbleQueue in _speechBubbleQueueDict.Values)
            {
                foreach (var bubble in bubbleQueue)
                {
                    bubble.Cam = _cam;
                }
            }
        }
    }

    protected void Awake()
    {
        if (_cam == null) _cam = Camera.main;

        if (_isSingleton)
        {
            UnityEngine.Assertions.Assert.IsNull(_instance, "_intance was not null. Do you maybe have several Speech Bubble Managers in your scene, all trying to be singletons?");
            _instance = this;
        }
        _prefabsDict.Clear();
        _speechBubbleQueueDict.Clear();
        foreach (var prefab in _prefabs)
        {
            _prefabsDict.Add(prefab.type, prefab.prefab);
            _speechBubbleQueueDict.Add(prefab.type, new Queue<SpeechBubble>());
        }
    }

    private IEnumerator DelaySpeechBubble(float delay, Transform objectToFollow, string text, SpeechbubbleType type, float timeToLive, Color color, Vector3 offset)
    {
        yield return new WaitForSeconds(delay);
        if (objectToFollow)
            AddSpeechBubble(objectToFollow, text, type, timeToLive, color, offset);
    }

    public SpeechBubble AddSpeechBubble(Vector3 position, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color))
    {
        if (timeToLive == 0) timeToLive = _defaultTimeToLive;
        if (color == default(Color)) color = _defaultColor;
        SpeechBubble bubbleBehaviour = GetBubble(type);
        bubbleBehaviour.Setup(position, text, timeToLive, color, Cam);
        _speechBubbleQueueDict[type].Enqueue(bubbleBehaviour);
        return bubbleBehaviour;
    }

    public SpeechBubble AddSpeechBubble(Transform objectToFollow, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color), Vector3 offset = new Vector3())
    {
        if (timeToLive == 0) timeToLive = _defaultTimeToLive;
        if (color == default(Color)) color = _defaultColor;
        SpeechBubble bubbleBehaviour = GetBubble(type);
        bubbleBehaviour.Setup(objectToFollow, offset, text, timeToLive, color, Cam);
        _speechBubbleQueueDict[type].Enqueue(bubbleBehaviour);
        return bubbleBehaviour;
    }

    public void AddDelayedSpeechBubble(float delay, Transform objectToFollow, string text, SpeechbubbleType type = SpeechbubbleType.NORMAL, float timeToLive = 0, Color color = default(Color), Vector3 offset = new Vector3())
    {
        StartCoroutine(DelaySpeechBubble(delay, objectToFollow, text, type, timeToLive, color, offset));
    }

    private SpeechBubble GetBubble(SpeechbubbleType type = SpeechbubbleType.NORMAL)
    {
        SpeechBubble bubbleBehaviour;
        if (_speechBubbleQueueDict[type].Count == 0 || _speechBubbleQueueDict[type].Peek().gameObject.activeInHierarchy)
        {
            GameObject newBubble = (GameObject)GameObject.Instantiate(GetPrefab(type));
            newBubble.transform.SetParent(transform);
            newBubble.transform.localScale = _sizeMultiplier * GetPrefab(type).transform.localScale;
            bubbleBehaviour = newBubble.GetComponent<SpeechBubble>();

            var canvas = newBubble.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.overrideSorting = true;
        }
        else
        {
            bubbleBehaviour = _speechBubbleQueueDict[type].Dequeue();
        }
        //Set as last to always place latest on top (in case of screenspace ui that is..)
        bubbleBehaviour.transform.SetAsLastSibling();
        return bubbleBehaviour;
    }

    private GameObject GetPrefab(SpeechbubbleType type)
    {
        return _prefabsDict[type];
    }

    public SpeechbubbleType GetRandomSpeechbubbleType()
    {
        return _prefabs[Random.Range(0, _prefabs.Count)].type;
    }

    public void Clear()
    {
        foreach (var bubbleQueue in _speechBubbleQueueDict)
        {
            foreach (var bubble in bubbleQueue.Value)
            {
                bubble.Clear();
            }
        }
    }

}
