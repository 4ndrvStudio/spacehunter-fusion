using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBubbleChat : MonoBehaviour
{
    [Multiline]
    public string[] thingsToSay = new string[] { "Hello world" };
    public Transform mouthAI;
    public float timeBetweenSpeak = 5f;
    private float timeToNextSpeak;
    // Use this for initialization
    void Start()
    {
        timeToNextSpeak = timeBetweenSpeak;
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextSpeak -= Time.deltaTime;

        if (timeToNextSpeak <= 0 && thingsToSay.Length > 0)
            SaySomething();
    }

    public void SaySomething()
    {
        string message = thingsToSay[Random.Range(0, thingsToSay.Length)];
        SaySomething(message);
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

