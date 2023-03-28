using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using Fusion;
namespace SH.Multiplayer
{
    public class Network_EnemyCombatBrain : NetworkBehaviour
    {

        [SerializeField] private Network_EnemyBrain _enemyBrain;
        [SerializeField] private Network_EnemyAnimation _enemyAnim;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [Networked, SerializeField] private E_COMBAT_STATE E_COMBAT_STATE { get; set; }

  
        [SerializeField] public string[] Attack_List;
        [SerializeField] private string[] _shoot_List;

        [SerializeField] private GameObject _shootBullet;
        [SerializeField] private Transform _shootPosition;

        [Networked] private NetworkBool _canAttack { get; set; }


        [Networked]
        public int AttackCount { get; set; }

        private Interpolator<int> _attackCountInterpolator;
        private int _lastVisibleAttack;

        [Networked(OnChanged = nameof(OnIndexAttackChanged))]
        public byte N_IndexAttack { get; set; }
        public int L_IndexAttack;

        [Networked] public float TimeToReloadAttack { get; set; }
        [Networked] private TickTimer _attackReloadTime { get; set; }
        [Networked] private NetworkBool _timeHasCal { get; set; }




        public override void Spawned()
        {
            if (Runner.IsServer == false) return;

            E_COMBAT_STATE = E_COMBAT_STATE.None;
            _canAttack = true;

            _attackCountInterpolator = GetInterpolator<int>(nameof(AttackCount));
            _lastVisibleAttack = AttackCount;



        }

        public override void FixedUpdateNetwork()
        {
            if (Runner.IsServer == false) return;

            if (_enemyBrain.E_AI_STATE == E_AI_STATE.Combat)
            {
                //if player player not in combat state, return to chasing
                if (_enemyBrain.DisToPlayer >= _enemyBrain.DisToCombat) _enemyBrain.E_AI_STATE = E_AI_STATE.Chasing;
                else E_COMBAT_STATE = E_COMBAT_STATE.NormalAttack;

                //select combat state. 
                switch (E_COMBAT_STATE)
                {
                    case E_COMBAT_STATE.NormalAttack:
                        NormalAttack();
                        break;
                    case E_COMBAT_STATE.GetHit: // do something;
                        break;
                    case E_COMBAT_STATE.Turned: // Do something;
                        break;
                    default: return;
                }
            }

        }

        private void NormalAttack()
        {

            if (_timeHasCal == false)
            {
                _attackReloadTime = TickTimer.CreateFromSeconds(Runner, TimeToReloadAttack);

                _timeHasCal = true;

                if (_enemyBrain.E_TYPE == E_TYPE.ShortRange)
                {
                    if (_canAttack) ShortRangeAttack();
                }
                else
                {
                    if (_canAttack) LongRangeAttack();
                };
            }
            else
            {
                if (_attackReloadTime.Expired(Runner))
                {
                    _timeHasCal = false;
                    _canAttack = true;
                }
            }



        }

        private void LongRangeAttack()
        {
            int targetAttack = GetRandomAttack(); // Define logic Here.
            _enemyBrain.RotateToPlayer();
            _enemyAnim.PlayShoot(_shoot_List[targetAttack]);
            StartCoroutine(ShootBullet());

            _canAttack = false;
        }


        private void ShortRangeAttack()
        {
            if(Runner.IsServer == false) return;
            
            int targetAttack = GetRandomAttack(); // Define logic Here.
            //int targetAttack = 0;
            N_IndexAttack = (byte) targetAttack;

            _enemyBrain.RotateToPlayer();
            
           // _enemyBrain.AimSupport();
            AttackCount++;
            // _enemyAnim.PlayAttack(Attack_List[0]);
            _canAttack = false;
        }

        IEnumerator ShootBullet()
        {
            yield return new WaitForSeconds(0.3f);
            _enemyBrain.RotateToPlayer();
            GameObject bullet = Instantiate(_shootBullet, _shootPosition.position + _shootPosition.forward * 0.5f, transform.rotation);
            Rigidbody rigid = bullet.GetComponent<Rigidbody>();
            rigid.AddForce(bullet.transform.forward * 10f, ForceMode.Impulse);
        }

        private int GetRandomAttack()
        {
            bool isShortRange = _enemyBrain.E_TYPE == E_TYPE.ShortRange;
            string[] targetList = isShortRange ? Attack_List : _shoot_List;
            int targetIndex = targetList.Length == 1 ? 0 : Random.Range(0, targetList.Length);

            return targetIndex;
        }

        static void OnIndexAttackChanged(Changed<Network_EnemyCombatBrain> changed)
        {
            changed.Behaviour.OnIndexAttackChanged();
        }
        private void OnIndexAttackChanged()
        {
            L_IndexAttack = N_IndexAttack;
        }

     

        public override void Render()
        {
            if (_lastVisibleAttack < AttackCount)
            {
                _enemyAnim.PlayAttack(Attack_List[L_IndexAttack]);

            }
            else if (_lastVisibleAttack > AttackCount)
            {
                //cancel attack
            }
            _lastVisibleAttack = AttackCount;


        }


    }
    public enum E_COMBAT_STATE
    {
        None,
        NormalAttack,
        GetHit,
        Turned,
        PreAttack,
        DecideAttack,
        EndAttack

    }
    public enum ACTION_STATE
    {
        State1,
        State2,
        State3,
        State4,
        State5
    }



}
