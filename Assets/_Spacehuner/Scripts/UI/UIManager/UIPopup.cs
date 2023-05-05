using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public virtual void Hide()
    {
        _customProperties = null;
        gameObject.SetActive(false);
    }
}


public enum PopupName
{
    None,
    Setting,
    Inventory,
    Notification,
    UpdateNotification
}
