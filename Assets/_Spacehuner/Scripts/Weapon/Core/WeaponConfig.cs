using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    [CreateAssetMenu(fileName = "Weapon Config" , menuName = "Game Data/Weapon/Config")]
    public class WeaponConfig : ScriptableObject
    {
        public string ItemId;
        public GameObject Prefab;
        public WeaponType WeaponType;
    }

    public enum WeaponType {
        MineralAxe,
        Sword
    }

}
