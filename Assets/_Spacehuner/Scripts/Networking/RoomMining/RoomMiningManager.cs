using Colyseus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Mining
{
    public class RoomMiningManager : ColyseusManager<RoomMiningManager>
    {
        private RoomMiningController _roomController;

        private RoomMiningEntityFactory _networkEntityFactory;

        public RoomMiningNetworkEntity CurrentNetworkEntity => _roomController.CurrentNetworkEntity;

        public bool IsInRoom => _roomController.Room != null;

        public ColyseusRoom<RoomMiningState> Room => _roomController.Room;

        public long Ping => _roomController.Ping;

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            if (_roomController != null)
                _roomController.LeaveRoom(true);
        }

        public override void InitializeClient()
        {
            base.InitializeClient();
            _roomController = new RoomMiningController(client);
            _networkEntityFactory = new RoomMiningEntityFactory(_roomController.Entities, _roomController.EntityViews);
        }

        public async void JoinOrCreateRoom(Dictionary<string, object> options)
        {
            await _roomController.JoinOrCreateRoom(options);
        }

        public async void JoinRoomId(string id)
        {
            await _roomController.JoinRoomId(id);
        }

        public async void LeaveRoom(Action onLeave)
        {
            await _roomController.LeaveRoom(true, onLeave);
        }

        public bool HasEntity(string id)
        {
            return _roomController.HasEntityView(id);
        }

        public RoomMiningNetworkEntityView GetEntityView(string id)
        {
            return _roomController.GetEntityView(id);
        }

        public async void SendAction(string action, object message)
        {
            if (_roomController == null)
            {
                Debug.LogError("Current room is null");
                return;
            }
            await _roomController.Room.Send(action, message);
        }

        public void RegisterNetworkEntityView(RoomMiningNetworkEntity entity, RoomMiningNetworkEntityView view, bool isMine)
        {
            _networkEntityFactory.RegisterNetworkEntityView(entity, view, isMine);

        }

    }
}
