using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SH;
using SH.Define;


public class UINotificationPopup : UIPopup
{
    [SerializeField] private TextMeshProUGUI _title; 
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private GameObject _okBtn; 
    [SerializeField] private GameObject _yesBtn;
    [SerializeField] private GameObject _noBtn;
    [SerializeField] private GameObject _closeBtn;

    [SerializeField] private Color _completeColor;
        [SerializeField] private Color _failColor;

    private void Start()
    {
        Show();

        _okBtn.GetComponent<Button>().onClick.AddListener(() => Hide());

    }

    public override void Show(object customProperties = null)
    {
        base.Show(customProperties);

        if(customProperties == null) return;
       
        foreach(KeyValuePair<NotifyProperty, string> property in customProperties as Dictionary<NotifyProperty, string> ) {
             switch(property.Key) {
                case NotifyProperty.Title : _title.text = property.Value;
                        break;
                case NotifyProperty.Content : _content.text = property.Value;
                        break;
                case NotifyProperty.OkBtn : _okBtn.SetActive(bool.Parse(property.Value));
                        break;
                case NotifyProperty.YesBtn : _yesBtn.SetActive(bool.Parse(property.Value));
                        break;
                case NotifyProperty.NoBtn : _noBtn.SetActive(bool.Parse(property.Value));
                        break;
                case NotifyProperty.CloseBtn : _closeBtn.SetActive(bool.Parse(property.Value));
                        break;
                case NotifyProperty.State : 
                        _title.color =bool.Parse(property.Value) ?  _completeColor : _failColor;
                        break;
                case NotifyProperty.TargetConfirm:   
                        _okBtn.GetComponent<Button>().onClick.AddListener(()=> { ReturnToStation(property.Value);});
                        _yesBtn.GetComponent<Button>().onClick.AddListener(()=> { ReturnToStation(property.Value);});
                        break;
             }
        }
        
    }


    public override void Hide()
    {
        base.Hide();
    }

    public void ReturnToStation(string sceneName) 
    {
        Debug.Log(sceneName);
        
    }
}

public enum NotifyProperty {
        Title,
        Content,
        OkBtn,
        YesBtn,
        NoBtn,
        CloseBtn,
        TargetConfirm,
        State
}

