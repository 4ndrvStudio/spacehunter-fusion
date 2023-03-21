using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_AnimatorHook : NetworkBehaviour
    {
        [SerializeField] private Network_WeaponCollider _weaponColider;
        [SerializeField] private List<GameObject> _attackVFXList;



        public void EnableCollider(CanHitName canHitName) {
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, true);
        }

        public void DisableCollider(CanHitName canHitName) {
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, false);
        }

        public void EnableVFX(int index) {
            _attackVFXList[index].SetActive(true);
        }
        public void DisableVFX(int index) {
            _attackVFXList[index].SetActive(false);
        }

        
    }

}
