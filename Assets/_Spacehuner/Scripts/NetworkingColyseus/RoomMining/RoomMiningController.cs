using Colyseus;
using NativeWebSocket;
using SH.Networking.PVE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SH.Networking.Mining
{
    public class RoomMiningController
    {
        public const string RoomName = "RoomMining";

        public static event Action<string, RoomMiningNetworkEntity> OnAddNetworkEntity;

        public static event Action<string, RoomMiningNetworkEntity> OnRemoveNetworkEntity;

        public static event Action<string, RoomMiningNetworkMineral> OnAddMineralEntity;

        public static event Action<string, RoomMiningNetworkMineral> OnRemoveMineralEntity;

        public static event Action<string, RoomPVENetworkEnemy> OnAddNetworkEnemy;

        public static event Action<string, RoomPVENetworkEnemy> OnRemoveNetworkEnemy;

        public static event Action<string> OnAttackNormal;

        public static event Action<RoomPVEController.EnemyHurtParam> OnEnemyHurt;

        public static event Action<RoomPVENetworkEnemy> OnEnemyRespawn = default;

        public static event Action<string> OnMining;

        public static event Action<RoomMiningNetworkMineral> OnMineralHurt;

        public static event Action<RoomMiningNetworkMineral> OnMineralRespawn = default;

        public event Action<RoomMiningState> OnRoomStateChange;

        public Dictionary<string, RoomMiningNetworkEntity> Entities { get; private set; } = new Dictionary<string, RoomMiningNetworkEntity>();

        public Dictionary<string, RoomMiningNetworkEntityView> EntityViews { get; private set; } = new Dictionary<string, RoomMiningNetworkEntityView>();

        public Dictionary<string, RoomMiningNetworkMineral> Minerals { get; private set; } = new Dictionary<string, RoomMiningNetworkMineral>();

        public Dictionary<string, RoomPVENetworkEnemy> Enemies { get; private set; } = new Dictionary<string, RoomPVENetworkEnemy>();

        public ColyseusRoom<RoomMiningState> Room { get; private set; }

        public RoomMiningNetworkEntity CurrentNetworkEntity { get; private set; }

        public long Ping { set; get; }

        private RoomMiningEntityFactory _entityFactory;

        private ColyseusClient _client;

        private Thread _pingThread;

        private long _lastPing;

        private long _lastPong;

        public RoomMiningController(ColyseusClient client)
        {
            this._client = client;
        }

        public bool HasEntityView(string entityId)
        {
            return EntityViews.ContainsKey(entityId);
        }

        public RoomMiningNetworkEntityView GetEntityView(string entityId)
        {
            if (EntityViews.ContainsKey(entityId))
                return EntityViews[entityId];
            return null;
        }

        public async Task JoinOrCreateRoom(Dictionary<string, object> options)
        {
            try
            {
                Room = await _client.JoinOrCreate<RoomMiningState>(RoomName, options);
                RegisterRoomHandlers();
                Debug.Log($"Joined room {RoomName} success!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Join room {RoomName} failed with reason: {e.Message}");
                UIManager.Instance.ShowAlert(e.Message, AlertType.Error);
            }
        }

        public async Task JoinRoomId(string roomId)
        {
            ClearRoomHandlers();
            if (Room == null)
            {
                try
                {
                    Room = await _client.JoinById<RoomMiningState>(roomId);
                    RegisterRoomHandlers();
                    Debug.Log($"Joined room {roomId} success!");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Join room fail with {e.Message}");
                }
            }
        }

        public async Task LeaveRoom(bool consented, Action onLeave = null)
        {
            if (Room != null)
            {
                await Room.Leave(consented);
                Entities.Clear();
                EntityViews.Clear();
                ClearRoomHandlers();
                onLeave?.Invoke();
            }
        }

        private void RegisterRoomHandlers()
        {
            if (_pingThread != null)
            {
                _pingThread.Abort();
                _pingThread = null;
            }

            _pingThread = new Thread(RunPingThread);
            _pingThread.Start(Room);

            Room.OnLeave += OnLeaveRoom;
            Room.OnStateChange += OnStateChange;
            Room.State.NetworkEntities.OnAdd += OnEntityAdd;
            Room.State.NetworkEntities.OnRemove += OnEntityRemove;
            Room.State.NetworkMinerals.OnAdd += OnMineralAdd;
            Room.State.NetworkMinerals.OnRemove += OnMineralRemove;
            Room.State.NetworkEnemies.OnAdd += OnEnemyAdd;
            Room.State.NetworkEnemies.OnRemove += OnEnemyRemove;
            Room.State.TriggerAll();
            Room.colyseusConnection.OnError += OnRoomError;
            Room.colyseusConnection.OnClose += OnRoomClose;

            Room.OnMessage<long>(RoomMiningAction.Ping, (serverTime) =>
            {
                _lastPong = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (_lastPong > _lastPing)
                    Ping = _lastPong - _lastPing;
            });

            Room.OnMessage<RoomMiningNetworkEntity>(RoomMiningAction.Join, (entity) =>
            {
                CurrentNetworkEntity = entity;
            });

            Room.OnMessage<string>(RoomMiningAction.Mining, (data) =>
            {
                OnMining?.Invoke(data);
            });

            Room.OnMessage<RoomMiningNetworkMineral>(RoomMiningAction.MineralHurt, (data) =>
            {
                OnMineralHurt?.Invoke(data);
            });

            Room.OnMessage<RoomMiningNetworkMineral>(RoomMiningAction.MineralSpawn, (data) =>
            {
                OnMineralRespawn?.Invoke(data);
            });


            Room.OnMessage<string>(RoomPVEAction.AttackNormal, (id) =>
            {
                OnAttackNormal?.Invoke(id);
            });

            Room.OnMessage<RoomPVEController.EnemyHurtParam>(RoomPVEAction.EnemyHurt, (data) =>
            {
                OnEnemyHurt?.Invoke(data);
            });

            Room.OnMessage<RoomPVENetworkEnemy>(RoomMiningAction.EnemyRespawn, (data) =>
            {
                OnEnemyRespawn?.Invoke(data);
            });
        }

        private void ClearRoomHandlers()
        {
            if (_pingThread != null)
            {
                _pingThread.Abort();
                _pingThread = null;
            }

            if (Room == null)
                return;

            Room.OnLeave -= OnLeaveRoom;
            Room.OnStateChange -= OnStateChange;
            Room.State.NetworkEntities.OnAdd -= OnEntityAdd;
            Room.State.NetworkEntities.OnRemove -= OnEntityRemove;
            Room.State.NetworkMinerals.OnAdd -= OnMineralAdd;
            Room.State.NetworkMinerals.OnRemove -= OnMineralRemove;
            Room.State.NetworkEnemies.OnAdd += OnEnemyAdd;
            Room.State.NetworkEnemies.OnRemove += OnEnemyRemove;
            Room.colyseusConnection.OnError -= OnRoomError;
            Room.colyseusConnection.OnClose -= OnRoomClose;
        }

        private void OnEnemyRemove(string key, RoomPVENetworkEnemy value)
        {
            if (Enemies.ContainsKey(value.Id))
            {
                OnRemoveNetworkEnemy?.Invoke(value.Id, value);
                Enemies.Remove(value.Id);
            }
        }

        private void OnEnemyAdd(string key, RoomPVENetworkEnemy value)
        {
            Enemies.Add(value.Id, value);
            OnAddNetworkEnemy?.Invoke(key, value);
        }


        private void OnLeaveRoom(int code)
        {
            WebSocketCloseCode closeCode = WebSocketHelpers.ParseCloseCodeEnum(code);
            Debug.Log($"Room: Leave reason: {closeCode} {code}");
            _pingThread.Abort();
            _pingThread = null;
            Room = null;
        }

        private void OnStateChange(RoomMiningState state, bool isFirstState)
        {
            OnRoomStateChange?.Invoke(state);
        }

        private void OnEntityAdd(string key, RoomMiningNetworkEntity entity)
        {
            Entities.Add(entity.Id, entity);
            OnAddNetworkEntity?.Invoke(key, entity);
        }

        private void OnEntityRemove(string key, RoomMiningNetworkEntity entity)
        {
            if (Entities.ContainsKey(entity.Id))
            {
                OnRemoveNetworkEntity?.Invoke(key, entity);
                Entities.Remove(entity.Id);
                EntityViews.Remove(entity.Id);
            }
        }

        private void OnMineralAdd(string key, RoomMiningNetworkMineral mineral)
        {
            Minerals.Add(mineral.Id, mineral);
            OnAddMineralEntity?.Invoke(mineral.Id, mineral);
        }

        private void OnMineralRemove(string key, RoomMiningNetworkMineral mineral)
        {
            if (Minerals.ContainsKey(mineral.Id))
            {
                OnRemoveMineralEntity?.Invoke(key, mineral);
                Minerals.Remove(mineral.Id);
            }
        }




        private void OnRoomClose(int closeCode)
        {
            Debug.LogError("OnRoomClose: " + closeCode);
        }

        private void OnRoomError(string errorMsg)
        {
            Debug.LogError("OnRoomError: " + errorMsg);
        }

        private async void RunPingThread(object roomToPing)
        {
            ColyseusRoom<RoomMiningState> currentRoom = (ColyseusRoom<RoomMiningState>)roomToPing;

            while (roomToPing != null)
            {
                _lastPing = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await currentRoom.Send(RoomMiningAction.Ping);
                Thread.Sleep(5000);
            }
        }

        
    }

    public class MineralHurt
    {
        public string Id;
        public float Damage;
        public bool IsDeath;
    }
}
