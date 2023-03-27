using Colyseus;
using NativeWebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SH.Networking.PVE
{
    public class RoomPVEController
    {
        public const string RoomName = "RoomPVE";

        public static event Action<string, RoomPVENetworkEntity> OnAddNetworkEntity;

        public static event Action<string, RoomPVENetworkEntity> OnRemoveNetworkEntity;

        public static event Action<string, RoomPVENetworkEnemy> OnAddNetworkEnemy;

        public static event Action<string, RoomPVENetworkEnemy> OnRemoveNetworkEnemy;

        public static event Action<string> OnAttackNormal;

        public static event Action<EnemyHurtParam> OnEnemyHurt;

        public event Action<RoomPVEState> OnRoomStateChange;

        public Dictionary<string, RoomPVENetworkEntity> Entities { get; private set; } = new Dictionary<string, RoomPVENetworkEntity>();

        public Dictionary<string, RoomPVENetworkEntityView> EntityViews { get; private set; } = new Dictionary<string, RoomPVENetworkEntityView>();

        public Dictionary<string, RoomPVENetworkEnemy> Enemies { get; private set; } = new Dictionary<string, RoomPVENetworkEnemy>();

        public ColyseusRoom<RoomPVEState> Room { get; private set; }

        public RoomPVENetworkEntity CurrentNetworkEntity { get; private set; }

        public long Ping { set; get; }

        private RoomPVEEntityFactory _entityFactory;

        private ColyseusClient _client;

        private Thread _pingThread;

        private long _lastPing;

        private long _lastPong;

        public RoomPVEController(ColyseusClient client)
        {
            this._client = client;
        }

        public bool HasEntityView(string entityId)
        {
            return EntityViews.ContainsKey(entityId);
        }

        public RoomPVENetworkEntityView GetEntityView(string entityId)
        {
            if (EntityViews.ContainsKey(entityId))
                return EntityViews[entityId];
            return null;
        }

        public async Task JoinOrCreateRoom(Dictionary<string, object> options)
        {
            try
            {
                Room = await _client.JoinOrCreate<RoomPVEState>(RoomName, options);
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
                    Room = await _client.JoinById<RoomPVEState>(roomId);
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
            Room.State.NetworkEnemies.OnAdd += OnEnemyAdd;
            Room.State.NetworkEnemies.OnRemove += OnEnemyRemove;

            Room.State.TriggerAll();
            Room.colyseusConnection.OnError += OnRoomError;
            Room.colyseusConnection.OnClose += OnRoomClose;

            Room.OnMessage<long>(RoomPVEAction.Ping, (serverTime) =>
            {
                _lastPong = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (_lastPong > _lastPing)
                    Ping = _lastPong - _lastPing;
            });

            Room.OnMessage<RoomPVENetworkEntity>(RoomPVEAction.Join, (entity) =>
            {
                CurrentNetworkEntity = entity;
            });

            Room.OnMessage<string>(RoomPVEAction.AttackNormal, (id) =>
            {
                OnAttackNormal?.Invoke(id);
            });

            Room.OnMessage<EnemyHurtParam>(RoomPVEAction.EnemyHurt, (data) =>
            {
                OnEnemyHurt?.Invoke(data);
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
            Room.State.NetworkEnemies.OnAdd -= OnEnemyAdd;
            Room.State.NetworkEnemies.OnRemove -= OnEnemyRemove;
            Room.colyseusConnection.OnError -= OnRoomError;
            Room.colyseusConnection.OnClose -= OnRoomClose;
        }

        private void OnLeaveRoom(int code)
        {
            WebSocketCloseCode closeCode = WebSocketHelpers.ParseCloseCodeEnum(code);
            Debug.Log($"Room: Leave reason: {closeCode} {code}");
            _pingThread.Abort();
            _pingThread = null;
            Room = null;
        }

        private void OnStateChange(RoomPVEState state, bool isFirstState)
        {
            OnRoomStateChange?.Invoke(state);
        }

        private void OnEntityAdd(string key, RoomPVENetworkEntity entity)
        {
            Entities.Add(entity.Id, entity);
            OnAddNetworkEntity?.Invoke(key, entity); // add key to check isMine
        }

        private void OnEntityRemove(string key, RoomPVENetworkEntity entity)
        {
            if (Entities.ContainsKey(entity.Id))
            {
                OnRemoveNetworkEntity?.Invoke(entity.Id, entity);
                Entities.Remove(entity.Id);
                EntityViews.Remove(entity.Id);
            }
        }

        private void OnEnemyAdd(string key, RoomPVENetworkEnemy enemy)
        {
            Enemies.Add(enemy.Id, enemy);
            OnAddNetworkEnemy?.Invoke(key, enemy);
        }

        public void OnEnemyRemove(string key, RoomPVENetworkEnemy enemy)
        {
            if (Enemies.ContainsKey(enemy.Id))
            {
                OnRemoveNetworkEnemy?.Invoke(enemy.Id, enemy);
                Enemies.Remove(enemy.Id);
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
            ColyseusRoom<RoomPVEState> currentRoom = (ColyseusRoom<RoomPVEState>)roomToPing;

            while (roomToPing != null)
            {
                _lastPing = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await currentRoom.Send(RoomPVEAction.Ping);
                Thread.Sleep(5000);
            }
        }

        public class EnemyHurtParam
        {
            public string Id;
            public int Health;

            public EnemyHurtParam()
            {

            }
        }
    }
}