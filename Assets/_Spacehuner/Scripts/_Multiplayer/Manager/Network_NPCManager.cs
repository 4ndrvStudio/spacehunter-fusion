using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

namespace SH.Multiplayer
{
     
    public class Network_NPCManager : NetworkBehaviour
    {
        public enum GameState
        {
            Starting,
            Running,
            Ending
        }

        public NetworkPrefabRef _npcPrefabs;
        public Transform PositionSpawn;

        public GameState _gameState;

        public override void Spawned()
        {
           
        
            if (Object.HasStateAuthority == false) return;

            _gameState = GameState.Starting;

        }
        public override void FixedUpdateNetwork()
        {   
             if(Runner.IsServer == false) return;

            switch (_gameState)
            {
                case GameState.Starting:
                    InitialSpawnNPC();
                    break;
                case GameState.Running:
                   // RespawnMineral();
                    break;
                case GameState.Ending:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
           
        }

        void InitialSpawnNPC() {
            
            Runner.Spawn(_npcPrefabs,PositionSpawn.position,Quaternion.identity, PlayerRef.None);

            _gameState = GameState.Running;
        }
    }

}
