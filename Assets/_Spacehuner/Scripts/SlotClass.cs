using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    private MineralClass item;
    private int quanlity;

    public SlotClass (MineralClass _item, int _quanlity)
    {
        item = _item;
        quanlity = _quanlity;
    }
    public MineralClass GetItem() { return item; }
    public int GetQuanlity() { return quanlity; }
}
