using Colyseus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Station
{
    public class RoomStationManager : ColyseusManager<RoomStationManager>
    {
        private RoomStationController _roomController;

        private RoomStationEntityFactory _networkEntityFactory;

        public RoomStationNetworkEntity CurrentNetworkEntity => _roomController.CurrentNetworkEntity;

        public bool IsInRoom => _roomController.Room != null;

        public ColyseusRoom<RoomStationState> Room => _roomController.Room;

        public long Ping => _roomController.Ping;

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            if(_roomController != null)
                _roomController.LeaveRoom(true);
        }

        public override void InitializeClient()
        {
            base.InitializeClient();
            _roomController = new RoomStationController(client);
            _networkEntityFactory = new RoomStationEntityFactory(_roomController.Entities, _roomController.EntityViews);
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

        public RoomStationNetworkEntityView GetEntityView(string id)
        {
            return _roomController.GetEntityView(id);
        }

        public async void SendAction(string action, object message)
        {
            if(_roomController == null)
            {
                Debug.LogError("Current room is null");
                return;
            }
           await _roomController.Room.Send(action, message);
        }

        public void RegisterNetworkEntityView(RoomStationNetworkEntity entity, RoomStationNetworkEntityView view, bool isMine)
        {
            _networkEntityFactory.RegisterNetworkEntityView(entity, view, isMine);

        }
    }
}
