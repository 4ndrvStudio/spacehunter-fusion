using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH;
using SH.Define;
using SH.PlayerData;

public class MiningManager : MonoBehaviour
{
    [SerializeField] private  int _seconds;
    [SerializeField] private int _minutes = 59;
    [SerializeField] private TextMeshProUGUI _uiTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeCalculate());
    }   

    public IEnumerator TimeCalculate()
    {
       bool _timeCal = true;
       while (_timeCal)
       {
           yield return new WaitForSeconds(1);
               _seconds--;
               _uiTime.text= $"{_minutes.ToString("D2")}:{_seconds.ToString("D2")}";
           if(_seconds <= 0) {
                _minutes--;
                _seconds = 60;
                if(_minutes < 0) {
                    _uiTime.text= "Mining End";
                    BackToStation();
                    break;
                }
           }
       }
    }
    public void BackToStation() {
        var properties = new Dictionary<NotifyProperty, string>() {
                    {NotifyProperty.Title, "Mining"},
                    {NotifyProperty.OkBtn, "true"},
                    {NotifyProperty.CloseBtn, "false"},
                    {NotifyProperty.Content, "Time running out."},
                    {NotifyProperty.TargetConfirm, SceneName.SceneStation}
                };
        UIManager.Instance.ShowPopup(PopupName.Notification, properties);
    }
}
