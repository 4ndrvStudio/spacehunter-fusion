using Colyseus;
using NativeWebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SH.Networking.Space
{
    public class RoomSpaceController
    {
        public const string RoomName = "RoomSpace";

        public static event Action<string, RoomSpaceNetworkEntity> OnAddNetworkEntity;

        public static event Action<string, RoomSpaceNetworkEntity> OnRemoveNetworkEntity;

        public static event Action<string, RoomSpaceAsteroidEntity> OnAddAsteroidEntity;

        public static event Action<string, RoomSpaceAsteroidEntity> OnRemoveAsteroidEntity;

        public static event Action<string> OnShooting;

        public event Action<RoomSpaceState> OnRoomStateChange;

        public Dictionary<string, RoomSpaceNetworkEntity> Entities { get; private set; } = new Dictionary<string, RoomSpaceNetworkEntity>();

        public Dictionary<string, RoomSpaceNetworkEntityView> EntityViews { get; private set; } = new Dictionary<string, RoomSpaceNetworkEntityView>();

        public ColyseusRoom<RoomSpaceState> Room { get; private set; }

        public RoomSpaceNetworkEntity CurrentNetworkEntity { get; private set; }

        public long Ping { set; get; }

        private RoomSpaceEntityFactory _entityFactory;

        private ColyseusClient _client;

        private Thread _pingThread;

        private long _lastPing;

        private long _lastPong;

        public RoomSpaceController(ColyseusClient client)
        {
            this._client = client;
        }

        public bool HasEntityView(string entityId)
        {
            return EntityViews.ContainsKey(entityId);
        }

        public RoomSpaceNetworkEntityView GetEntityView(string entityId)
        {
            if (EntityViews.ContainsKey(entityId))
                return EntityViews[entityId];
            return null;
        }

        public async Task JoinOrCreateRoom(Dictionary<string, object> options)
        {
            try
            {
                Room = await _client.JoinOrCreate<RoomSpaceState>(RoomName, options);
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
                    Room = await _client.JoinById<RoomSpaceState>(roomId);
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
            Room.State.Asteroids.OnAdd += OnAsteroidAdd;
            Room.State.Asteroids.OnRemove += OnAsteroidRemove;
            Room.State.TriggerAll();
            Room.colyseusConnection.OnError += OnRoomError;
            Room.colyseusConnection.OnClose += OnRoomClose;

            Room.OnMessage<long>(RoomSpaceAction.Ping, (serverTime) =>
            {
                _lastPong = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (_lastPong > _lastPing)
                    Ping = _lastPong - _lastPing;
            });

            Room.OnMessage<RoomSpaceNetworkEntity>(RoomSpaceAction.Join, (entity) =>
            {
                CurrentNetworkEntity = entity;
            });

            Room.OnMessage<string>(RoomSpaceAction.Shoot, (id) =>
            {
                OnShooting?.Invoke(id);
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
            Room.State.Asteroids.OnAdd -= OnAsteroidAdd;
            Room.State.Asteroids.OnRemove -= OnAsteroidRemove;
            Room.colyseusConnection.OnError -= OnRoomError;
            Room.colyseusConnection.OnClose -= OnRoomClose;
        }

        private void OnAsteroidRemove(string key, RoomSpaceAsteroidEntity value)
        {
            OnRemoveAsteroidEntity?.Invoke(key, value);
        }

        private void OnAsteroidAdd(string key, RoomSpaceAsteroidEntity value)
        {
            OnAddAsteroidEntity?.Invoke(key, value);
        }

        private void OnLeaveRoom(int code)
        {
            WebSocketCloseCode closeCode = WebSocketHelpers.ParseCloseCodeEnum(code);
            Debug.Log($"Room: Leave reason: {closeCode} {code}");
            _pingThread.Abort();
            _pingThread = null;
            Room = null;
        }

        private void OnStateChange(RoomSpaceState state, bool isFirstState)
        {
            OnRoomStateChange?.Invoke(state);
        }

        private void OnEntityAdd(string key, RoomSpaceNetworkEntity entity)
        {
            Entities.Add(entity.Id, entity);
            OnAddNetworkEntity?.Invoke(key, entity);
        }

        private void OnEntityRemove(string key, RoomSpaceNetworkEntity entity)
        {
            if (Entities.ContainsKey(entity.Id))
            {
                OnRemoveNetworkEntity?.Invoke(key, entity);
                Entities.Remove(entity.Id);
                EntityViews.Remove(entity.Id);
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
            ColyseusRoom<RoomSpaceState> currentRoom = (ColyseusRoom<RoomSpaceState>)roomToPing;

            while (roomToPing != null)
            {
                _lastPing = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await currentRoom.Send(RoomSpaceAction.Ping);
                Thread.Sleep(5000);
            }
        }
    }
}
