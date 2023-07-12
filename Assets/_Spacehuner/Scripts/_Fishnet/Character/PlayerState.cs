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

    public class PlayerState : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Animator _anim;
 
        [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
        public bool IsOnGround { get; [ServerRpc(RunLocally = true)] set; }
        
        [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
        public bool IsAttack { get; [ServerRpc(RunLocally = true)] set; }

        [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
        public bool IsAbility { get; [ServerRpc(RunLocally = true)] set; }

        [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
        public bool IsTakeDamage { get; [ServerRpc(RunLocally = true)] set; }


        private void Awake()
        {
            InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;

        }

        private void OnDestroy()
        {
            if (InstanceFinder.TimeManager != null)
            {
                InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
            }
        }

        private void TimeManager_OnTick()
        {
            if (base.IsOwner == false)
                    return;
            
            IsOnGround = _playerMovement.Grounded;
            IsAttack = _anim.GetCurrentAnimatorStateInfo(3).IsTag("Action");
            IsTakeDamage = _anim.GetCurrentAnimatorStateInfo(3).IsTag("TakeDamage");
            IsAbility = _anim.GetCurrentAnimatorStateInfo(3).IsTag("Ability");

        }



    }


  

}
