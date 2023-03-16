using Colyseus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SH.Networking.Chat
{
    public class RoomChatController
    {
        public const string RoomName = "RoomChat";

        public ColyseusRoom<RoomChatState> Room { get; private set; }

        public static Action<ChatContent> OnReceiveChat;

        private ColyseusClient _client;

        public RoomChatController(ColyseusClient client)
        {
            this._client = client;
        }

        public async Task JoinOrCreateRoom(Dictionary<string, object> options)
        {
            try
            {
                Room = await _client.JoinOrCreate<RoomChatState>(RoomName, options);
                Room.OnMessage<ChatContent>("Chat", data =>
                {
                    OnReceiveChat?.Invoke(data);
                });
                Debug.Log($"Joined room {RoomName} success!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Join room {RoomName} failed with reason: {e.Message}");
                UIManager.Instance.ShowAlert(e.Message, AlertType.Error);
            }
        }


        public class ChatContent
        {
            public string Timestamp;
            public string Name;
            public string Content;

            public ChatContent()
            {

            }
        }
    }
}