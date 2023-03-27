using Colyseus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SH.Networking.Space
{
    public class RoomSpaceManager : ColyseusManager<RoomSpaceManager>
    {
        private RoomSpaceController _roomController;

        private RoomSpaceEntityFactory _networkEntityFactory;

        public RoomSpaceNetworkEntity CurrentNetworkEntity => _roomController.CurrentNetworkEntity;

        public bool IsInRoom => _roomController.Room != null;

        public ColyseusRoom<RoomSpaceState> Room => _roomController.Room;

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
            _roomController = new RoomSpaceController(client);
            _networkEntityFactory = new RoomSpaceEntityFactory(_roomController.Entities, _roomController.EntityViews);
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

        public RoomSpaceNetworkEntityView GetEntityView(string id)
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

        public void RegisterNetworkEntityView(RoomSpaceNetworkEntity entity, RoomSpaceNetworkEntityView view, bool isMine)
        {
            _networkEntityFactory.RegisterNetworkEntityView(entity, view, isMine);

        }
    }
}
