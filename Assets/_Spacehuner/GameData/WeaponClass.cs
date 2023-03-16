using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponClass", menuName ="Game Data/Weapon")]
public class WeaponClass : ScriptableObject
{
    public string Name;
    public WeaponType type;
    public GameObject Prefab;
    public int Id;
    public int Dame;
}

// Id weapon 


// 10001 => Mine Axe 

// 20001 - 2999 => Sword

// 30001 - 3999 =>  Bow

public enum WeaponType {
    None,
    Sword,
    MineAxe,
    Bow,
    Gun,
    MagicStick
}
