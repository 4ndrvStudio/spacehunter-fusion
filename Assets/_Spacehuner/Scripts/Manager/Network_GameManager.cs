using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Fusion.Sockets;
using System;
using SH.PlayerData;

namespace SH.Multiplayer
{

    public sealed class Network_GameManager : SimulationBehaviour, INetworkRunnerCallbacks
    {
        // PUBLIC MEMBERS

        public static UnityAction PlayerJoined;

        // PRIVATE MEMBERS

        [SerializeField] private NetworkObject _playerPrefab;

        private Dictionary<PlayerRef, NetworkObject> _players = new(100);

        [SerializeField]
        private Dictionary<int, Vector3> _spawnPosition = new Dictionary<int, Vector3> {
            {2 , new Vector3(0.0700000003f,13.6230001f,-70.5400009f)},
            {3 , new Vector3(4.78000021f,-0.349999994f, 114.089996f)}
        };


        // MONOBEHAVIOUR

        private void Awake()
        {
            var networkEvents = GetComponent<NetworkEvents>();
            networkEvents.OnSceneLoadDone.AddListener(OnSceneLoaded);
        }




        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("Connected To Server");

        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.Log("Connected To Fail");
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {

        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            if (Runner.IsClient == false) return;

            GameManager.Instance.RequireReConnect = true;

        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {

        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            if (Runner.IsServer == true) return;
            Debug.Log("Input missing");
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            if (Runner.IsServer == false)
                return;

            Vector3 spawnPos = _spawnPosition[(int)runner.CurrentScene];


            var player = Runner.Spawn(_playerPrefab, spawnPos, Quaternion.identity, inputAuthority: playerRef);

            _players.Add(playerRef, player);


            Runner.SetPlayerObject(playerRef, player);

            Debug.Log("Player Connected To Room: " + playerRef.PlayerId);
            PlayerJoined?.Invoke();


        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
        {

            if (Runner.IsServer == false)
                return;

            if (_players.TryGetValue(playerRef, out NetworkObject player) == false)
                return;

            Runner.Despawn(player);
            Debug.Log("Player Left Room: " + playerRef.PlayerId);
            _players.Remove(playerRef);
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {

        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {


            if (Runner.IsServer)
            {
                Debug.Log("Joined to " + SceneManager.GetActiveScene().name);
            }
            else
            {

                StartCoroutine(HideWaiting());

            }


        }

        IEnumerator HideWaiting()
        {
            yield return new WaitForSeconds(4f);
            UIManager.Instance.HideLoadScene();
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            if (Runner.IsServer)
            {
                Debug.Log("Scene Load to " + SceneManager.GetActiveScene().name);
            }
            else
            {
               // UIManager.Instance.ShowLoadScene(false);
            }

        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (Runner.IsClient == false) return;
            Debug.Log("is Shutdown");

            
            if (Network_Player.Local != null)
            {

                Debug.Log(Network_Player.Local.transform.position);

            }

            if(shutdownReason == ShutdownReason.PhotonCloudTimeout) GameManager.Instance.RequireReConnect = true;


        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }


        public async Task LeaveRom()
        {
            if(Runner.IsRunning) {
                 await Runner.Shutdown(false, ShutdownReason.Ok, true);
            }

        }

        // PRIVATE METHODS

        private void OnSceneLoaded(NetworkRunner runner)
        {
            var behaviours = runner.SimulationUnityScene.FindObjectsOfTypeInOrder<SimulationBehaviour>();

            for (int i = 0; i < behaviours.Length; i++)
            {
                Runner.AddSimulationBehaviour(behaviours[i]);
            }
        }
    }
}
