using SH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.Networking.Mining;
using SH.Define;

public class UISceneMining : MonoBehaviour
{
    [SerializeField] private Canvas _canvas = null;

    private void Start()
    {
        if (UIManager.Instance != null)
        {
            _canvas.worldCamera = UIManager.Instance.UICamera;
        }
    }

    private void Update()
    {
        if (RoomMiningManager.Instance != null)
            UIManager.Instance.SetPing(RoomMiningManager.Instance.Ping);
    }

    public void OnBackClick()
    {
        RoomMiningManager.Instance.LeaveRoom(async() =>
        {
            UIManager.Instance.LoadScene(SceneName.SceneStation);
        });
    }

}
