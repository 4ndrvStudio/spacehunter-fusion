using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class SlashFX : MonoBehaviour
    {
       // [SerializeField] private float _destroyTime = 2f;
        void Start()
        {
            Destroy(this.gameObject, 2f);
        }
    }

}
