using SH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.Define;
using SH.Multiplayer;
using UnityEngine.UI;
using System;

public class UISceneMining : MonoBehaviour
{
    [SerializeField] private Canvas _canvas = null;
    [SerializeField] private Button _buttonBack;

    private void Start()
    {
        _buttonBack.onClick.AddListener(() => OnBackClick());
    }
    public void OnBackClick()
    {

        ulong exp = Convert.ToUInt64(Network_RoomPVE.Instance.ExpCollectedCount);
        List<ulong> amountStone = new List<ulong> { Convert.ToUInt64(Network_RoomMining.Instance.MineralCollectedCount)};
        List<string> symbolStone = new List<string> { "dst_stone" };

        Debug.Log("Exiting Mining Room");
        Network_ClientManager.ExitRoomMining(exp, amountStone, symbolStone);
        gameObject.SetActive(false);



    }

}


