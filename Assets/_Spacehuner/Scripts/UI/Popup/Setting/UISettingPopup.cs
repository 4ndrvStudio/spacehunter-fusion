using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISettingPopup : UIPopup
{
    [SerializeField] private TextMeshProUGUI _tmpSetting = null;

    public override void Show(object customProperties = null)
    {
        base.Show(customProperties);
        Setup();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void Setup()
    {
        _tmpSetting.SetText("Setting");
    }
}
