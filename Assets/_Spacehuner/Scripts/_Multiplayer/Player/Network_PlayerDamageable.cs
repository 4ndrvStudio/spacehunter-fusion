using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerDamageable : NetworkBehaviour
    {

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

        public void HitPlayer(PlayerRef player)
        {
            // if (Object == null) return;

            // if (Object.HasInputAuthority == false) return;

            // if (_wasHit) return;

            // if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            // {      
            //     playerNetworkObject.GetComponentInChildren<Network_WeaponCollider>().ToggleActiveCollider(CanHitName.Mineral, false);
            // }

            // _wasHit = true;
        }

        public override void FixedUpdateNetwork()
        {
            // if (Object.HasStateAuthority == false) return;
           
            // if (_wasHit)
            // {
            //     _wasHit = false;

            //     HitCount++;

            // }


        }
       
    }


}
