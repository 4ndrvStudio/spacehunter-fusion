using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

namespace SH.Multiplayer
{
    public class Network_LocalTestManager : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _networkRunner;
        [SerializeField] private bool _isRunning;

        async void StartGame(GameMode mode)
        {
            _networkRunner.ProvideInput = true;

            await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "testRoom",
                Scene = (int) SH.Define.SceneDefs.scene_miningFusion,
                SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>()
            });

            _isRunning = true;
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
