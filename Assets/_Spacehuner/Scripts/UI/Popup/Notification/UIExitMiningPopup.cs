using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SH.Multiplayer;
using System;
namespace SH.UI
{
    public class UIExitMiningPopup : UIPopup
    {
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _exitButton;

        public void Start() {
            _cancelButton.onClick.AddListener(() => Hide());
            _exitButton.onClick.AddListener(() => ExitMining());
        }

        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);
            UIControllerManager.Instance.HideAllController();
        }

        public override void Hide()
        {
            base.Hide();
            UIControllerManager.Instance.DisplayController();

        }

        public void ExitMining() {
            ulong exp = Convert.ToUInt64(Network_RoomPVE.Instance.ExpCollectedCount);
            List<ulong> amountStone = new List<ulong> { Convert.ToUInt64(Network_RoomMining.Instance.MineralCollectedCount)};
            List<string> symbolStone = new List<string> { "ST1" };
            Network_ClientManager.ExitRoomMining(exp, amountStone, symbolStone);
            Hide();
        }


    }

}
