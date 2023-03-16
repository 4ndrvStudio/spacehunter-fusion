using Colyseus;
using Colyseus.Schema;
using NativeWebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SH.Networking.Station
{
    public class RoomStationController
    {
        public const string RoomName = "RoomStation";

        public static event Action<string, RoomStationNetworkEntity> OnAddNetworkEntity;

        public static event Action<string, RoomStationNetworkEntity> OnRemoveNetworkEntity;

        public event Action<RoomStationState> OnRoomStateChange;

        public Dictionary<string, RoomStationNetworkEntity> Entities { get; private set; } = new Dictionary<string, RoomStationNetworkEntity>();

        public Dictionary<string, RoomStationNetworkEntityView> EntityViews { get; private set; } = new Dictionary<string, RoomStationNetworkEntityView>();

        public ColyseusRoom<RoomStationState> Room { get; private set; }

        public RoomStationNetworkEntity CurrentNetworkEntity { get; private set; }

        public long Ping { set; get; }

        private RoomStationEntityFactory _entityFactory;

        private ColyseusClient _client;

        private Thread _pingThread;

        private long _lastPing;

        private long _lastPong;

        public RoomStationController(ColyseusClient client)
        {
            this._client = client;
        }

        public bool HasEntityView(string entityId)
        {
            return EntityViews.ContainsKey(entityId);
        }

        public RoomStationNetworkEntityView GetEntityView(string entityId)
        {
            if (EntityViews.ContainsKey(entityId))
                return EntityViews[entityId];
            return null;
        }

        public async Task JoinOrCreateRoom(Dictionary<string, object> options)
        {
            try
            {
                Room = await _client.JoinOrCreate<RoomStationState>(RoomName, options);
                RegisterRoomHandlers();
                Debug.Log($"Joined room {RoomName} success!");
            }
            catch(Exception e)
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
                    Room = await _client.JoinById<RoomStationState>(roomId);
                    RegisterRoomHandlers();
                    Debug.Log($"Joined room {roomId} success!");
                }
                catch(Exception e)
                {
                    Debug.LogError($"Join room fail with {e.Message}");
                }
            }
        }

        public async Task LeaveRoom(bool consented, Action onLeave = null)
        {
            if(Room != null)
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
            if(_pingThread != null)
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
            Room.State.TriggerAll();
            Room.colyseusConnection.OnError += OnRoomError;
            Room.colyseusConnection.OnClose += OnRoomClose;

            Room.OnMessage<long>(RoomStationAction.Ping, (serverTime) => 
            {
                _lastPong = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (_lastPong > _lastPing)
                    Ping = _lastPong - _lastPing;
            });

            Room.OnMessage<RoomStationNetworkEntity>(RoomStationAction.Join, (entity) =>
            {
                CurrentNetworkEntity = entity;
            });
        }

        private void ClearRoomHandlers()
        {
            if(_pingThread != null)
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

        private void OnStateChange(RoomStationState state, bool isFirstState)
        {
            OnRoomStateChange?.Invoke(state);
        }

        private void OnEntityAdd(string key, RoomStationNetworkEntity entity)
        {
            Entities.Add(entity.Id, entity);
            OnAddNetworkEntity?.Invoke(key, entity);
        }

        private void OnEntityRemove(string key, RoomStationNetworkEntity entity)
        {
            if(Entities.ContainsKey(entity.Id))
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
            ColyseusRoom<RoomStationState> currentRoom = (ColyseusRoom<RoomStationState>)roomToPing;

            while(roomToPing != null)
            {
                _lastPing = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await currentRoom.Send(RoomStationAction.Ping);
                Thread.Sleep(5000);
            }
        }
    }
}
