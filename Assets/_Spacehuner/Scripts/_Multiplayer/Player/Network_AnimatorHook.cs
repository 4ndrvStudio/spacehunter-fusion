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
        [SerializeField] private Transform _centerWeapon;

        public void SetWeaponCollider(Network_WeaponCollider weaponColider) {
            _weaponColider = weaponColider;
            weaponColider.SetupCenterOverlapse(_centerWeapon);
        }

        public void EnableCollider(CanHitName canHitName) {
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, true);
        }

        public void DisableCollider(CanHitName canHitName) {
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, false);
        }

        public void EnableVFX(int index) {
            GameObject targetFX = _attackVFXList[index];
            GameObject fx  = Instantiate(targetFX,targetFX.transform.position, targetFX.transform.rotation);
            fx.SetActive(true);
        }
     

        
    }

}
