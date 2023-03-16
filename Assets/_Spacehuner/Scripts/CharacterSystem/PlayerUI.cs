using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.PlayerData;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _camPos;
    [SerializeField] private TextMeshProUGUI _playerNameUI;
    // Update is called once per frame
    void Start() {
        //update Name
        SetPlayerName();
    }
    void LateUpdate()
    {
        FaceToCamera();
    }

    private void SetPlayerName(){
        string _playerName = PlayerDataManager.DisplayName;
        _playerNameUI.text = _playerName;
        
    }

    private void FaceToCamera() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0,180f,0);
    }

}
