using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer.Player
{
    using FishNet;
    using FishNet.Object;
    using FishNet.Object.Prediction;
    using FishNet.Transporting;
    using FishNet.Object.Synchronizing;

    public class PlayerDamageable : NetworkBehaviour
    {   
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private PlayerStats _playerStats;
        
        [SyncVar(WritePermissions = WritePermission.ServerOnly)]
        public int TakeDameCount = 0;
        
        public void TakeDamage() {
            
            TakeDameCount++;
             _playerStats.TakeDamage(1);
        }

    }
}