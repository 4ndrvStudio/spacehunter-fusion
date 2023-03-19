using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_AnimatorHook : NetworkBehaviour
    {
        [SerializeField] private Network_WeaponCollider _weaponColider;
        

        public void EnableCollider(CanHitName canHitName) {
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, true);
        }

        public void DisableCollider(CanHitName canHitName) {
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, false);

        }
    }

}
