using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerDamageable : NetworkBehaviour
    {
        [SerializeField] private Network_PlayerStats _playerStats;
        [SerializeField] private Network_PlayerState _playerState;

        [Networked] private NetworkBool _wasHit { get; set; }
        [Networked] private NetworkBool _isAlive { get; set; }

        [Networked, HideInInspector]
        public int HitCount { get; set; }

        private Interpolator<int> _hitCountInterpolator;

        public override void Spawned()
        {
            _hitCountInterpolator = GetInterpolator<int>(nameof(HitCount));
            _isAlive = true;
        }

        public void HitPlayer(Network_EnemyWeaponCollider _enemyWeaponCollider)
        {
            if (Object == null) return;

            if (Object.HasStateAuthority == false || _playerState.L_IsDeath == true) return;

            if (_wasHit) return;
            _enemyWeaponCollider.ToggleActiveCollider(CanHitName.Mineral, false);


            // if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            // {
             
            //     Network_EnemyWeaponCollider s = playerNetworkObject.GetComponent<Network_EnemyWeaponCollider>();
            //     s.ToggleActiveCollider(CanHitName.Mineral, false);
            //     Debug.Log(s.gameObject.name);
            // }

            _wasHit = true;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false) return;
           
            if (_wasHit)
            {
                _wasHit = false;

                Debug.Log("Was Hitted");

                HitCount++;

            }


        }
       
    }


}
