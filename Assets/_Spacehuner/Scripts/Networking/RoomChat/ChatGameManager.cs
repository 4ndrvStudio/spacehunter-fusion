using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.PlayerData;

namespace SH.Networking.Chat
{
    public class ChatGameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpContent = default;

        [SerializeField] private TMP_InputField _inputChat = default;

        private string _content = default;

        private void Start()
        {
            RoomChatManager.Instance.InitializeClient();
            RoomChatManager.Instance.JoinOrCreateRoom(null);
            RoomChatController.OnReceiveChat += OnReceiveChat;
        }

        private void OnDestroy()
        {
            RoomChatController.OnReceiveChat -= OnReceiveChat;
        }

        private void OnReceiveChat(RoomChatController.ChatContent chatContent)
        {
            _content += $"{chatContent.Name}: {chatContent.Content} \n";
            _tmpContent.SetText(_content);
        }

        public void OnEndEditChat()
        {
            if (!string.IsNullOrEmpty(_inputChat.text))
            {
                RoomChatController.ChatContent content = new RoomChatController.ChatContent();
                content.Name = PlayerDataManager.DisplayName;
                content.Content = _inputChat.text;
                RoomChatManager.Instance.SendAction("Chat", content);
                _inputChat.text = null;
            }
        }
    }
}
