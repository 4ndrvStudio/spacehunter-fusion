using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_WeaponManager : NetworkBehaviour
    {
            
        [SerializeField] private List<WeaponConfig> _weaponConfigList = new List<WeaponConfig>();

        [SerializeField] private Network_PlayerState _playerState;

        [Networked(OnChanged = nameof(OnWeaponInUseIdChanged))]
        public byte N_WeaponInUseId { get; set; }
        public int L_WeaponInUseId;

        [Networked(OnChanged = nameof(OnHasEquipedWeaponChanged))]
        public NetworkBool N_HasEquipWeapon {get; set;}
        public bool L_HasEquipWeapon;


        public override void Spawned() {

            if(Object.HasInputAuthority == false) return;
            
            RPC_SetEquippedWeapon(true);

        }

        public override void FixedUpdateNetwork() {

            if(Object.HasInputAuthority == false) return;
            

        }


        //weapon in use change
        static void OnWeaponInUseIdChanged(Changed<Network_WeaponManager> changed)
        {
            changed.Behaviour.OnWeaponInUseIdChanged();
        }
        private void OnWeaponInUseIdChanged()
        {
            L_WeaponInUseId = N_WeaponInUseId;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetWeaponInUse(int weaponInUseID, RpcInfo info = default)
        {
            this.N_WeaponInUseId = (byte)weaponInUseID;
        }
        
        //dectect equiped
        static void OnHasEquipedWeaponChanged(Changed<Network_WeaponManager> changed)
        {
            changed.Behaviour.OnHasEquipedWeaponChanged();
        }
        private void OnHasEquipedWeaponChanged()
        {
            L_HasEquipWeapon = N_HasEquipWeapon;

            if(L_HasEquipWeapon == false) RPC_SetWeaponInUse(0);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetEquippedWeapon(bool hasEquip, RpcInfo info = default)
        {
            this.N_HasEquipWeapon =  hasEquip;
        }

        

        

        
    }

}
