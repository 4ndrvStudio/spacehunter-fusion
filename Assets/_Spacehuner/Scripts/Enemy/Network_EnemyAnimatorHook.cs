using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
namespace SH.Multiplayer
{
    public class Network_EnemyAnimatorHook : NetworkBehaviour
    {
        [SerializeField] private Network_EnemyWeaponCollider _enemyWeaponCollider;

        public void OnEnableWeaponCollider()
        {
            if (Object.IsProxy == true) return;
            _enemyWeaponCollider.ToggleActiveCollider(CanHitName.Mineral, true);
        }

        public void OnDisableWeaponCollider()
        {
            if (Object.IsProxy == true) return;
            _enemyWeaponCollider.ToggleActiveCollider(CanHitName.Mineral, false);
        }

    }

}
