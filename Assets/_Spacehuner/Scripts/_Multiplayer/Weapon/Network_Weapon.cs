using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class Network_Weapon : MonoBehaviour
    {
        [SerializeField] private Transform _centerOverlapse;

        public Transform GetCenter() => _centerOverlapse;
    }

}
