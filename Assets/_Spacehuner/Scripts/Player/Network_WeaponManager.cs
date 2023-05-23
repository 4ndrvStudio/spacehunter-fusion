using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

namespace SH.Multiplayer
{
    public class Network_WeaponManager : NetworkBehaviour
    {
        public static Network_WeaponManager Instance;

        [SerializeField] private Network_PlayerState _playerState;
        [SerializeField] private Network_PlayerCombat _playerCombat;
        [SerializeField] private Network_WeaponCollider _weaponCollider;
        
        private Network_AnimatorHook _animatorHook;

        [SerializeField] private List<WeaponConfig> _weaponConfigList = new List<WeaponConfig>();
      
        [SerializeField] private GameObject _weaponModel;
        [SerializeField]   private GameObject _weaponHolder;
        [SerializeField] private WeaponConfig _weaponConfig;

        [Networked(OnChanged = nameof(OnWeaponInUseIdChanged))]
        public NetworkString<_32> N_WeaponInUseId { get; set; }
        public string L_WeaponInUseId;


        public override void Spawned()
        {

            if (Object.HasInputAuthority == false) return;            
       
        }

        public override void FixedUpdateNetwork()
        {

            if (Object.HasInputAuthority == false) return;


        }


        //weapon in use change
        static void OnWeaponInUseIdChanged(Changed<Network_WeaponManager> changed)
        {
            changed.Behaviour.OnWeaponInUseIdChanged();
        }
        private void OnWeaponInUseIdChanged()
        {
            L_WeaponInUseId = N_WeaponInUseId.ToString();
            

            _weaponConfig = InventoryManager.Instance.WeaponConfigs.Find(config => config.ItemId == L_WeaponInUseId);
            

            if(_weaponConfig == null) return;


            if (_weaponModel != null)
                Destroy(_weaponModel.gameObject);

            _weaponModel = Instantiate(_weaponConfig.Prefab);

            _weaponModel.transform.SetParent(_weaponHolder.transform);
            _weaponModel.transform.localPosition = _weaponConfig.Prefab.transform.position;
            _weaponModel.transform.localRotation = _weaponConfig.Prefab.transform.rotation;

            if (Object.HasInputAuthority)
            {
                switch (_weaponConfig.WeaponType)
                {
                    case WeaponType.Sword:
                        _playerCombat.RPC_SetIsMiningWeapon(false);
                        break;
                    case WeaponType.MineralAxe:
                        _playerCombat.RPC_SetIsMiningWeapon(true);
                        break;
                }
            }

            _animatorHook.SetDissolve(_weaponModel.gameObject.GetComponent<Weapon>());
            _weaponCollider.SetupWeaponInUse(_weaponModel.gameObject.GetComponent<Weapon>());


        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetWeaponInUse(string weaponInUseID, RpcInfo info = default)
        {
    
            this.N_WeaponInUseId = weaponInUseID;
            
        }

        public void SetupWeapon(GameObject weaponHolder, Network_AnimatorHook animatorHook)
        {
            _weaponHolder  = weaponHolder;
            _animatorHook = animatorHook;

            //set default weapon
            RPC_SetWeaponInUse("weapon_swordtest");
        }

        public void UseWeapon(string weaponId) {

            _weaponConfig = InventoryManager.Instance.WeaponConfigs.Find(config => config.ItemId == weaponId);
            
            if(_weaponConfig == null) return;


            if (_weaponModel != null)
                Destroy(_weaponModel.gameObject);

            _weaponModel = Instantiate(_weaponConfig.Prefab);

            _weaponModel.transform.SetParent(_weaponHolder.transform);
            _weaponModel.transform.localPosition = _weaponConfig.Prefab.transform.position;
            _weaponModel.transform.localRotation = _weaponConfig.Prefab.transform.rotation;

            if (Object.HasInputAuthority)
            {
                switch (_weaponConfig.WeaponType)
                {
                    case WeaponType.Sword:
                        _playerCombat.RPC_SetIsMiningWeapon(false);
                        break;
                    case WeaponType.MineralAxe:
                        _playerCombat.RPC_SetIsMiningWeapon(true);
                        break;
                }
            }

            _animatorHook.SetDissolve(_weaponModel.gameObject.GetComponent<Weapon>());
            _weaponCollider.SetupWeaponInUse(_weaponModel.gameObject.GetComponent<Weapon>());

        }



    }

}
