using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_EnemyWeaponCollider : NetworkBehaviour
    {

        [Networked]
        public NetworkBool CanHitMineral { get; set; }

        [SerializeField] private LayerMask _playerCollisionLayer;

        [SerializeField] private Weapon _weaponInUse;

        private List<LagCompensatedHit> _lagCompensatedHits = new List<LagCompensatedHit>();


        public override void FixedUpdateNetwork()
        {
         

            if (Runner.IsServer == false)
                return;

            HasHitPlayer();

        }

        public void ToggleActiveCollider(CanHitName canHitName, bool isActive)
        {

            switch (canHitName)
            {
                case CanHitName.Mineral:
                    CanHitMineral = isActive;
                    break;
            }
        }

        public bool HasHitPlayer()
        {

            if (CanHitMineral == false) return false;

            if (_weaponInUse == null) return false;
            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation
                        .OverlapBox(_weaponInUse.CenterOverlapse.transform.position,
                                    _weaponInUse.OverlapseExtends, Quaternion.LookRotation(_weaponInUse.CenterOverlapse.transform.up),
                                    Object.InputAuthority, _lagCompensatedHits, _playerCollisionLayer.value);

            if (count <= 0) return false;

            var networkPlayerDamageable = _lagCompensatedHits[0].GameObject.GetComponentInParent<Network_PlayerDamageable>();

            networkPlayerDamageable.HitPlayer(this);

            return true;

        }
    }

}
