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

    public class PlayerStats : NetworkBehaviour
    {
        
        [SyncVar(WritePermissions = WritePermission.ServerOnly, OnChange = nameof(OnHpChange))]
        public int HP;

        public override void OnStartServer()
        {
            base.OnStartServer();
            HP = 100;
        }

        private void OnHpChange(int prev, int next, bool asServer) {
            if(IsOwner) {
                HpUiTest.Instance.SetTest(next.ToString());
            }
        }
        
        public void TakeDamage(int damage) {
            if(IsServer == false)
                return;
                
            HP-= damage;
        }
        
    }

}
