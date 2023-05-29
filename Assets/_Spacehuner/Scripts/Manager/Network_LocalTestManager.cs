using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using SH.Define;
using SH.AzureFunction;
using SH.PlayerData;
using PlayFab;
using PlayFab.ClientModels;

namespace SH.Multiplayer
{
    public class Network_LocalTestManager : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunner;
        [SerializeField] private bool _isRunning;
        [SerializeField] private SceneDefs SceneTest;

        async void Start() {

           //StartCoroutine(StartGameTest());       
        }


        IEnumerator StartGameTest() {
            yield return new WaitForSeconds(7);
             StartGame(GameMode.Host);

        }
        async void StartGame(GameMode mode)
        {
            _networkRunner.ProvideInput = true;

            await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "testRoom",
                Scene = (int) SceneTest,
                SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>()
            });

            _isRunning = true;
            Application.targetFrameRate = 60;
        }

        private void OnGUI()
        {
            if(_isRunning) return;

            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
            if (GUI.Button(new Rect(0, 80, 200, 40), "Server"))
            {
                StartGame(GameMode.Server);
            }
  

        }

        

    }



}
