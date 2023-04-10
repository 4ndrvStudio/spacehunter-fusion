using SH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.Define;
using SH.Multiplayer;

public class UISceneMining : MonoBehaviour
{
    [SerializeField] private Canvas _canvas = null;

    private void Start()
    {
        if (UIManager.Instance != null)
        {
           // _canvas.worldCamera = UIManager.Instance.UICamera;
        }
    }

    private void Update()
    {
        
    }

    public void OnBackClick()
    {
        Network_ClientManager.MoveToRoom(SceneDefs.scene_stationFusion);
    }

}


