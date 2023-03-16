using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StreamingMusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _source = default;

    [SerializeField] private AudioClip _clip = default;

    private string _path = "http://commondatastorage.googleapis.com/codeskulptor-demos/DDR_assets/Sevish_-__nbsp_.mp3";

    private void Start()
    {
        StartCoroutine(LoadMusic(_path, AudioType.MPEG));
    }


    IEnumerator LoadMusic(string songPath, AudioType type)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(songPath, type))
        {
            ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
                yield break;
            }

            DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;

            if (dlHandler.isDone)
            {
                AudioClip audioClip = dlHandler.audioClip;

                if (audioClip != null)
                {
                    _clip = DownloadHandlerAudioClip.GetContent(uwr);

                    Debug.Log("Playing song using Audio Source!");
                    _source.clip = _clip;
                    _source.Play();

                }
                else
                {
                    Debug.Log("Couldn't find a valid AudioClip :(");
                }
            }
            else
            {
                Debug.Log("The download process is not completely finished.");
            }
        }


    }
}
