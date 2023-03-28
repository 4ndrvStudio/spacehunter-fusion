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

        [Networked]
        public NetworkBool CanHitMineral { get; set; }

        //private 
        [SerializeField] private LayerMask _mineralCollisionLayer;
        [SerializeField] private LayerMask _enemyCollisionLayer;
        [SerializeField] private LayerMask _playerCollisionLayer;
        [SerializeField] private Transform _centerOverlapse;

        [SerializeField] private Vector3 _extends;


        private List<LagCompensatedHit> _lagCompensatedHits = new List<LagCompensatedHit>();

        public void SetupCenterOverlapse(Transform centerTransform) => _centerOverlapse  = centerTransform;

        public void SetupDamageZone(Transform centerTransfom, Vector3 extend) {
            _centerOverlapse = centerTransfom;
            _extends = extend;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.IsProxy == true)
                return;
            
            if (HasHitMineral()) Debug.Log("Hit Mineral");

            if (HasHitEnemy()) Debug.Log("Hit Enemy");

           // if (HasHitPlayer()) Debug.Log("Hit Player");



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

            if(_centerOverlapse == null) return false;

            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation.OverlapBox(_centerOverlapse.transform.position, _extends, Quaternion.LookRotation(_centerOverlapse.transform.up),
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

            if(_centerOverlapse == null) return false;

            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation.OverlapBox(_centerOverlapse.transform.position, _extends, Quaternion.LookRotation(_centerOverlapse.transform.up),
                Object.InputAuthority, _lagCompensatedHits, _enemyCollisionLayer.value);
            
            if (count <= 0) return false;

            var networkEnemyDamageable = _lagCompensatedHits[0].GameObject.GetComponentInParent<Network_EnemyDamageable>();

            networkEnemyDamageable.HitEnemy(Object.InputAuthority, Object.transform);

            return true;
        }

        public bool HasHitPlayer() {

            if (CanHitMineral == false) return false;

            if(_centerOverlapse == null) return false;
              _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation.OverlapBox(_centerOverlapse.transform.position, _extends, Quaternion.LookRotation(_centerOverlapse.transform.up),
                Object.InputAuthority, _lagCompensatedHits, _playerCollisionLayer.value);
            
            if (count <= 0) return false;

            var networkPlayerDamageable = _lagCompensatedHits[0].GameObject.GetComponentInParent<Network_PlayerDamageable>();

            networkPlayerDamageable.HitPlayer(Object.InputAuthority);

            return true;

        }

       



    }

}
