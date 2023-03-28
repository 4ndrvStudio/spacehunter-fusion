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
        
        [SerializeField] private Dictionary<int, Vector3> _spawnPosition = new Dictionary<int, Vector3> {
            {2 , new Vector3(-62.1300011f, -1.31900001f, 96.1299973f) },
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

        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {

        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            if (Runner.IsServer == false)
                return;
          
            Vector3 spawnPos = _spawnPosition[(int) runner.CurrentScene];
            

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
            }else {
                  UIManager.Instance.HideWaiting(); 
                   // if(Object.HasInputAuthority == true) {
                 
            }
          


        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            if (Runner.IsServer)
            {
                Debug.Log("Scene Load to " + SceneManager.GetActiveScene().name);
            } else {
                 UIManager.Instance.ShowWaiting();      
            }
       
         


        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
             
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }


        public async Task LeaveRom()
        {
            UIManager.Instance.ShowWaiting();  
            await Runner.Shutdown(false, ShutdownReason.Ok, true);
            UIManager.Instance.HideWaiting();  

           
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
