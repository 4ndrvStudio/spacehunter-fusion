using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using SH.Define;

namespace SH.Multiplayer
{
        public enum MiningGameState
        {
            Starting,
            Running,
            Ending
        }
    public class Network_RoomMining : NetworkBehaviour
    {
    

        [System.Serializable]
        public class MineralData
        {
            public bool WasSpawn;
            public Network_SpawnPoint SpawnPoint;
            [HideInInspector] public NetworkObject MineralSpawned;
            [Networked] public TickTimer RespawnTime { get; set; }
            public bool RespawnTimeStarted;

        }


        [SerializeField] private List<MineralData> _mineralDataList = new List<MineralData>();

        [SerializeField] private List<NetworkPrefabRef> _mineralObList = new List<NetworkPrefabRef>();



        [Networked] private MiningGameState _gameState { get; set; }



        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            _gameState = MiningGameState.Starting;

        }
        public override void FixedUpdateNetwork()
        {
            if(Runner.IsServer == false) return;

            switch (_gameState)
            {
                case MiningGameState.Starting:
                    InitialSpawnMineral();
                    break;
                case MiningGameState.Running:
                    RespawnMineral();
                    break;
                case MiningGameState.Ending:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void SpawnMineral(MineralData mineral) {

                Network_SpawnPoint spawnPoint = mineral.SpawnPoint;

                // Simple Random
                int randomInt = UnityEngine.Random.Range(0, 20);

                int targetInt = randomInt > 15 &&  randomInt <=19 ? 2 : randomInt > 8  &&  randomInt <=15 ? 1 : 0;
 
                NetworkPrefabRef targetToSpawn = _mineralObList[targetInt];

                var mineralSpawned = Runner.Spawn(targetToSpawn, spawnPoint.GetSpawnPosition(), Quaternion.identity , PlayerRef.None);

                mineral.WasSpawn = true;

                mineralSpawned.GetComponent<Network_Mineral>().Network_RoomMining = this;

                mineral.MineralSpawned = mineralSpawned;

        }


        public void InitialSpawnMineral()
        {

            _mineralDataList.ForEach(mineral =>
            {
                SpawnMineral(mineral);
            });

            _gameState = MiningGameState.Running;

        }

        public void RespawnMineral()
        {

            _mineralDataList.ForEach(mineral =>
            {

                if (mineral.WasSpawn == false)
                {

                    if (mineral.RespawnTimeStarted == false)
                    {

                        mineral.RespawnTime = TickTimer.CreateFromSeconds(Runner, 10);
                        mineral.RespawnTimeStarted = true;
                    }
                    else
                    {

                        if (mineral.RespawnTime.Expired(Runner))
                        {
                            SpawnMineral(mineral);

                            mineral.RespawnTimeStarted = false;
                        }
                    }
                }
            });

        }

        public void MineralCollected(NetworkObject mineral)
        {
            if (Runner.IsServer == false) return;

            MineralData mineralData = _mineralDataList.Find(spawn => spawn.MineralSpawned == mineral);

            mineralData.MineralSpawned = null;
            mineralData.WasSpawn = false;

            Runner.Despawn(mineral);

        }



    }

}
