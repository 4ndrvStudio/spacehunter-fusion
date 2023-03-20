using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_RoomPVE : NetworkBehaviour
    {
        enum GameState
        {
            Starting,
            Running,
            Ending
        }
        
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

        [Networked] private GameState _gameState { get; set; }

        
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
                    InitialSpawnEnemy();
                    break;
                case GameState.Running:
                    RespawnEnemy();
                    break;
                case GameState.Ending:
                    //
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SpawnEnemy(EnemyData enemy) {

                Network_SpawnPoint spawnPoint = enemy.SpawnPoint;

                // Simple Random
                // int randomInt = UnityEngine.Random.Range(0, 20);

                // int targetInt = randomInt > 15 &&  randomInt <=19 ? 2 : randomInt > 8  &&  randomInt <=15 ? 1 : 0;
 
                // NetworkPrefabRef targetToSpawn = _mineralObList[targetInt];

                var enemySpawned = Runner.Spawn(_enemyObList[0], spawnPoint.GetSpawnPosition(), Quaternion.identity , PlayerRef.None);

               
                enemy.WasSpawn = true;
                 Debug.Log(enemy.WasSpawn);

                enemySpawned.GetComponent<Network_EnemyMovement>().SetWaypoint(enemy.PatrolPointList);

                enemy.EnemySpawned = enemySpawned;
        }

        public void InitialSpawnEnemy()
        {

            _enemyDataList.ForEach(enemy =>
            {
                SpawnEnemy(enemy);
            });

            _gameState = GameState.Running;

        }

         public void RespawnEnemy()
        {

            _enemyDataList.ForEach(enemy =>
            {

                if (enemy.WasSpawn == false)
                {

                    if (enemy.RespawnTimeStarted == false)
                    {

                        enemy.RespawnTime = TickTimer.CreateFromSeconds(Runner, 10);
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


    }

}
