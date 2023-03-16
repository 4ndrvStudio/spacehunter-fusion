using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

namespace SH.Multiplayer
{
    public class Network_RunnerManager : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunner;
        void Start()
        {
            Application.targetFrameRate = 60;

            if (CommandLineUtils.IsHeadlessMode() == false) return;

            StartGame(GameMode.Server);
            Debug.Log("Start Success");
            

        }
    

        async void StartGame(GameMode mode)
        {
           
            _networkRunner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom2",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        
private void OnGUI()
{
  if (_networkRunner != null)
  {
    if (GUI.Button(new Rect(0,0,200,40), "Host"))
    {
        StartGame(GameMode.Host);
    }
    if (GUI.Button(new Rect(0,40,200,40), "Join"))
    {
        StartGame(GameMode.Client);
    }
  }
}



    }

}
