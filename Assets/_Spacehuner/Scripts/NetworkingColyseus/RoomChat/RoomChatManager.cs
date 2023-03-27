using Colyseus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Chat
{
    public class RoomChatManager : ColyseusManager<RoomChatManager>
    {

        private RoomChatController _roomController;

        public bool IsInRoom => _roomController.Room != null;

        public ColyseusRoom<RoomChatState> Room => _roomController.Room;


        protected override void Start()
        {
            base.Start();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
        }

        public override void InitializeClient()
        {
            base.InitializeClient();
            _roomController = new RoomChatController(client);
        }

        public async void JoinOrCreateRoom(Dictionary<string, object> options)
        {
            await _roomController.JoinOrCreateRoom(options);
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
    }
}
