using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Fusion;
using System.Threading.Tasks;

namespace SH.Multiplayer
{
    public class Network_EnemyDamageable : NetworkBehaviour
    {
        [SerializeField] private Network_EnemyBrain _enemyBrain;
        [SerializeField] private Network_EnemyStats _enemyStats;
        [SerializeField] private Network_EnemyAnimation _enemyAnim;
        
        [HideInInspector] public Network_RoomPVE RoomPVE;

        //network 
        [Networked] private NetworkBool _wasHit { get; set; }
        [Networked] private NetworkBool _isAlive { get; set; }

        [Networked(OnChanged = nameof(OnHitFromPosChanged))]
        private Vector3 N_HitFromPosition { get; set; }
        private Vector3 L_HitFromPosition;

        [Networked, HideInInspector]
        public int HitCount { get; set; }

        private Interpolator<int> _hitCountInterpolator;
        private int _lastVisibleHit;
        private int _currentDame;

        public override void Spawned()
        {
            _hitCountInterpolator = GetInterpolator<int>(nameof(HitCount));
            _lastVisibleHit = HitCount;
            _isAlive = true;
        }


        public void HitEnemy(PlayerRef player, Transform hisFromPos, int dame = 0)
        {
            if (Object == null) return;
            if (Object.HasStateAuthority == false) return;
            if (_wasHit) return;
            
            _currentDame = dame;

            N_HitFromPosition = hisFromPos.position;

            if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            {
                playerNetworkObject.GetComponentInChildren<Network_WeaponCollider>().ToggleActiveCollider(CanHitName.Mineral, false);
            }

            _wasHit = true;
        }


        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false) return;
            if (_wasHit)
            {
                _wasHit = false;

                _enemyStats.ReduceHealth(_currentDame);

                HitCount++;


            }
            if (_isAlive && _enemyStats.HP <= 0)
            {
                EnemyDestroy();
                _isAlive = false;

            }

        }

        async void EnemyDestroy()
        {
            await Task.Delay(4000);
            RoomPVE.EnemyDefeated(Object);

        }

        private void GetHit(Vector3 hitFromPos)
        {
            if (_isAlive == false)
            {
                return;
            }
            _enemyAnim.GetDame();

            // CameraManager.Instance.ShakeCam(5, 0.1f);

            // CameraManager.Instance.ShakeCam(2,0.1f);

            PushEnemyBack(hitFromPos, 0.5f);
            _enemyBrain.RotateToPlayer(hitFromPos);
            _enemyBrain.ReSelectNearestPlayer();
            _enemyStats.ReduceHealth(1);

            if (_enemyStats.HP <= 0)
            {
                PushEnemyBack(hitFromPos, 2f);
            }

        }
        public void PushEnemyBack(Vector3 hitFromPos, float factor)
        {
            Vector3 direction = transform.position - hitFromPos;
            direction = direction.normalized * factor;
            direction.y = transform.position.y;
            Vector3 targetPos = transform.position + direction;
            targetPos.y = transform.position.y;
           // transform.DOMove(targetPos, 1f);
        }

        public override void Render()
        {
            if (_lastVisibleHit < HitCount)
            {
                GetHit(L_HitFromPosition);

            }
            else if (_lastVisibleHit > HitCount)
            {
                //cancel attack
            }
            _lastVisibleHit = HitCount;


        }

        static void OnHitFromPosChanged(Changed<Network_EnemyDamageable> changed)
        {
            changed.Behaviour.OnHitFromPosChanged();
        }
        private void OnHitFromPosChanged()
        {
            this.L_HitFromPosition = N_HitFromPosition;
        }





    }
}
