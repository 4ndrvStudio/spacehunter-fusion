using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public enum CanHitName
    {
        Mineral = 0,
        Enemy = 1
    }

    public class Network_WeaponCollider : NetworkBehaviour
    {

        [SerializeField] private Network_PlayerState _playerState;

        [Networked]
        public NetworkBool CanHitMineral { get; set; }

        //private 
        [SerializeField] private LayerMask _mineralCollisionLayer;
        [SerializeField] private LayerMask _enemyCollisionLayer;
        [SerializeField] private LayerMask _playerCollisionLayer;

        [SerializeField] private Weapon _weaponInUse;

        private List<LagCompensatedHit> _lagCompensatedHits = new List<LagCompensatedHit>();

        public void SetupWeaponInUse(Weapon weapon) => _weaponInUse = weapon;

        public override void FixedUpdateNetwork()
        {
            if (Object.IsProxy == true)
                return;

            if (_playerState.L_IsInsideBuilding) return;

            HasHitMineral();

            HasHitEnemy();

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


        public bool HasHitMineral()
        {
            if (CanHitMineral == false) return false;

            if (_weaponInUse == null) return false;

            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation
                                .OverlapBox(_weaponInUse.CenterOverlapse.transform.position,
                                _weaponInUse.OverlapseExtends, Quaternion.LookRotation(_weaponInUse.CenterOverlapse.transform.up),
                                Object.InputAuthority, _lagCompensatedHits, _mineralCollisionLayer.value);

            if (count <= 0) return false;

            _lagCompensatedHits.SortDistance();

            var networkMineral = _lagCompensatedHits[0].GameObject.GetComponentInParent<Network_Mineral>();

            networkMineral.HitMineral(Object.InputAuthority);

            return true;
        }

        public bool HasHitEnemy()
        {
            if (CanHitMineral == false) return false;

            if (_weaponInUse == null) return false;

            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation
                                .OverlapBox(_weaponInUse.CenterOverlapse.transform.position,
                                            _weaponInUse.OverlapseExtends, Quaternion.LookRotation(_weaponInUse.CenterOverlapse.transform.up),
                                            Object.InputAuthority, _lagCompensatedHits, _enemyCollisionLayer.value);

            if (count <= 0) return false;

            var networkEnemyDamageable = _lagCompensatedHits[0].GameObject.GetComponentInParent<Network_EnemyDamageable>();

            networkEnemyDamageable.HitEnemy(Object.InputAuthority, Object.transform);

            return true;
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

            networkPlayerDamageable.HitPlayer(Object.InputAuthority);

            return true;

        }





    }

}
