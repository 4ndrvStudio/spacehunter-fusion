using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SH.PlayerData;
using System;

namespace SH.Chat
{
    public class ChatManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpMsg = default;

        [SerializeField] private TMP_InputField _input = default;

        private string _msg = default;

        private void Start()
        {
            //RoomStationHandler.Instance.OnMsgReceived += OnMsgReceived;
        }

        private void OnDestroy()
        {
            //RoomStationHandler.Instance.OnMsgReceived -= OnMsgReceived;
        }

        private void OnMsgReceived(string msg)
        {
            Debug.Log("msg: " + msg);
            _msg += msg + "\n";
            _tmpMsg.SetText(_msg);
        }



        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                OnChatClick();
            }
        }
        public void OnChatClick()
        {
            if(!string.IsNullOrEmpty(_input.text))
            {
                Debug.Log(_input.text);
                //RoomStationHandler.Instance.SendMessage(StationAction.Chat, _input.text);
            }
        }



    }
}
