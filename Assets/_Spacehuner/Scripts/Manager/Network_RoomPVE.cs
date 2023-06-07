using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    
        public enum PVEGameState
        {
            Starting,
            Running,
            Ending
        }
    public class Network_RoomPVE : NetworkBehaviour
    {
        
        [System.Serializable]
        public class EnemyData
        {
            public bool WasSpawn;
            public Network_SpawnPoint SpawnPoint;
            public Transform[] PatrolPointList;
            [HideInInspector] public NetworkObject EnemySpawned;
            [Networked] public TickTimer RespawnTime { get; set; }
            public bool RespawnTimeStarted;
        }

        
        [SerializeField] private List<EnemyData> _enemyDataList = new List<EnemyData>();

        [SerializeField] private List<NetworkPrefabRef> _enemyObList = new List<NetworkPrefabRef>();

        [Networked] private PVEGameState _gameState { get; set; }

        public int ExpCollectedCount;
        
        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;

            _gameState = PVEGameState.Starting;

        }

        public override void FixedUpdateNetwork()
        {
            if(Runner.IsServer == false) return;
            
            switch (_gameState)
            {
                case PVEGameState.Starting:
                    InitialSpawnEnemy();
                    break;
                case PVEGameState.Running:
                    RespawnEnemy();
                    break;
                case PVEGameState.Ending:
                    //
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SpawnEnemy(EnemyData enemy) {

                Network_SpawnPoint spawnPoint = enemy.SpawnPoint;

              
                int randomInt = UnityEngine.Random.Range(0, _enemyObList.Count);

                var enemySpawned = Runner.Spawn(_enemyObList[randomInt], spawnPoint.GetSpawnPosition(), Quaternion.identity , PlayerRef.None);

               
                enemy.WasSpawn = true;
                enemySpawned.GetComponent<Network_EnemyDamageable>().RoomPVE = this;
                enemySpawned.GetComponent<Network_EnemyMovement>().SetWaypoint(enemy.PatrolPointList);

                enemy.EnemySpawned = enemySpawned;
        }

        public void InitialSpawnEnemy()
        {

            _enemyDataList.ForEach(enemy =>
            {
                SpawnEnemy(enemy);
            });

            _gameState = PVEGameState.Running;

        }

        public void RespawnEnemy()
        {

            _enemyDataList.ForEach(enemy =>
            {

                if (enemy.WasSpawn == false)
                {

                    if (enemy.RespawnTimeStarted == false)
                    {
                        enemy.RespawnTime = TickTimer.CreateFromSeconds(Runner, 5);
                        enemy.RespawnTimeStarted = true;
                    }
                    else
                    {

                        if (enemy.RespawnTime.Expired(Runner))
                        {
                            SpawnEnemy(enemy);

                            enemy.RespawnTimeStarted = false;
                        }
                    }
                }
            });

        }

         public void EnemyDefeated(NetworkObject enemy)
        {
            if (Runner.IsServer == false) return;

            EnemyData enemyData = _enemyDataList.Find(spawn => spawn.EnemySpawned == enemy);

            enemyData.EnemySpawned = null;

            enemyData.WasSpawn = false;

            int exp = UnityEngine.Random.Range(500, 1000);
            ExpCollectedCount += exp;
            
            Runner.Despawn(enemy);

        
            
        }



    }

}
