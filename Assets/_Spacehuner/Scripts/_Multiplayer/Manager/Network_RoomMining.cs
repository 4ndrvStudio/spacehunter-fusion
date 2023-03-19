using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using SH.Define;

namespace SH.Multiplayer
{
    public class Network_RoomMining : NetworkBehaviour
    {
        enum GameState
        {
            Starting,
            Running,
            Ending
        }

        [SerializeField] private List<Network_SpawnPoint> _spawnPosition = new List<Network_SpawnPoint>();
        //[SerializeField] private List<GameObject> _mineralOb = new List<GameObject>();
        [SerializeField] private NetworkPrefabRef _mineralOb = NetworkPrefabRef.Empty;
        [SerializeField] private List<Transform> _mineralTransform = new List<Transform>();
        [SerializeField] private List<NetworkId> _mineralId = new List<NetworkId>();


        [Networked] private GameState _gameState { get; set; }

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;
            _gameState = GameState.Starting;

        }
        public override void FixedUpdateNetwork()
        {
            switch (_gameState)
            {
                case GameState.Starting:
                    SpawnMineral();
                    break;
                case GameState.Running:
                    Debug.Log("IsRunning");
                    break;
                case GameState.Ending:
                    Debug.Log("Ending");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        public void SpawnMineral()
        {

            _spawnPosition.ForEach(pos =>
            {   
                Vector3 Postemp = pos.GetSpawnPosition();
                var mineral = Runner.Spawn(_mineralOb, pos.GetSpawnPosition(), Quaternion.identity, PlayerRef.None);
                _mineralId.Add(mineral.Id);
                _mineralTransform.Add(mineral.transform);
            });



            _gameState = GameState.Running;

        }
    }

}
