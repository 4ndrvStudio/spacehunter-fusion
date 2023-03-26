using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class Network_SlashFX : MonoBehaviour
    {
       // [SerializeField] private float _destroyTime = 2f;
        void Start()
        {
            Destroy(this.gameObject, 2f);
        }
    }

}
