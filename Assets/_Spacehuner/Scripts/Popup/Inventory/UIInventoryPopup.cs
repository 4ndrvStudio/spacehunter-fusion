using SH.PlayerData;
using SuperScrollView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPopup : UIPopup
{
    [SerializeField] private LoopGridView _scroll = null;
    [SerializeField] private GameObject _prefabSlot = null;
    private bool _init = false;

    private void Start()
    {
        Show();
    }

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
        if(!_init)
        {
            _init = true;
            _scroll.InitGridView(30, OnGetItemByRowColumn);
        }
    }

    private LoopGridViewItem OnGetItemByRowColumn(LoopGridView scroll, int index, int row, int column)
    {

        var item = _scroll.NewListViewItem(_prefabSlot.name);
        return item;

    }
}
