using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopup : MonoBehaviour
{
    [SerializeField] private PopupName _popupName = PopupName.None;
    public PopupName PopupName => _popupName;

    private object _customProperties;

    public virtual void Show(object customProperties = null)
    {
        this._customProperties = customProperties;
        gameObject.SetActive(true);
    }
    public virtual void ShowWithCallback(object customProperties = null, UnityAction callback = null) {
        this._customProperties = customProperties;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _customProperties = null;
        gameObject.SetActive(false);
    }
}


public enum PopupName
{
    None=0,
    Setting=1,
    Inventory=2,
    Notification=3,
    UpdateNotification=4,
    Crafting=5,
    CharacterInfo = 6,
    ExitMiningPopup = 7,
    SuiTxSuccess = 996,
    SuiMiningReward = 997,
    SuiEstimatedGas = 998,
    SuiNotification=999
}
