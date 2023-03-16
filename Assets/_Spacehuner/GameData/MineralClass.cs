using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Mineral", menuName ="Game Data/Mineral")]
public class MineralClass : ScriptableObject
{
    public MineralType Type;
    public int Heal;
    public GameObject Prefab;
    public string itemName;
    public Sprite icon;
}

public enum MineralType
{
    Lv1 = 1,
    Lv2 = 2,
    Lv3 = 3,
}
