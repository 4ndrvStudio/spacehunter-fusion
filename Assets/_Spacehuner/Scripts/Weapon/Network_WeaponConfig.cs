using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    [CreateAssetMenu(fileName = "Weapon Config" , menuName = "Weapon/Config")]
    public class Network_WeaponConfig : ScriptableObject
    {
        public string Id;
        public GameObject Prefabs;
        public Sprite Sprite;
        public WeaponType WeaponType;
        public Network_Weapon WeaponScript;

    }

    public enum WeaponType {
        PickAxe,
        Sword
    }

}
