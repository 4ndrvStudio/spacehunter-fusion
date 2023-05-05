using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_EnemyAnimation : NetworkBehaviour
    {
        [SerializeField] private Network_EnemyBrain _enemyBrain;
        [SerializeField] private Network_EnemyCombatBrain _enemyCombatBrain;
        [SerializeField] private Animator _anim;
        [SerializeField] private SkinnedMeshRenderer _bodyMesh;
        private Material _enemyMat;

        [SerializeField] private bool IsAttack;


        private int _lastVisibleAttack;

        public override void Spawned()
        {

            Material[] materials = _bodyMesh.materials;
            _bodyMesh.material = new Material(materials[0]);
            _enemyMat = _bodyMesh.materials[0];
        

            _lastVisibleAttack = _enemyCombatBrain.AttackCount;



        }


        // Update is called once per frame
        public override void Render()
        {
            Movement();

            //Render Attack
            if (_lastVisibleAttack < _enemyCombatBrain.AttackCount)
            {

                _anim.SetTrigger(_enemyCombatBrain.Attack_List[_enemyCombatBrain.L_IndexAttack]);

            }
            else if (_lastVisibleAttack > _enemyCombatBrain.AttackCount)
            {
                //cancel attack
            }

            _lastVisibleAttack = _enemyCombatBrain.AttackCount;

        }

        void Movement()
        {
            switch (_enemyBrain.E_AI_STATE)
            {
                case E_AI_STATE.Dead:
                    DeathAnimation();
                    break;
                case E_AI_STATE.Combat:
                case E_AI_STATE.Idle:
                    MovementAnimation(0);
                    break;
                case E_AI_STATE.Patrolling:
                    MovementAnimation(0.5f);
                    break;
                case E_AI_STATE.Chasing:
                    MovementAnimation(1f);
                    break;
            }
        }
        public void SetJump(bool target)
        {
            _anim.SetBool("isJump", target);
        }

        public void SetBool(string name, bool target) => _anim.SetBool(name, target);


        void DeathAnimation()
        {
            _anim.SetBool("isDeath", true);
            
        }

        void MovementAnimation(float speed)
        {
            _anim.SetFloat("movement", speed, 0.5f, Time.deltaTime);
        }

        public void GetDame()
        {
            _anim.Play("GetDame", 1, 0);

            
            //Effect
            _enemyMat.DOColor(Color.white, "_EmissionColor", 0f).OnComplete(() =>
            {
                _enemyMat.DOColor(Color.red, "_EmissionColor", 0.1f).OnComplete(() =>
                {
                    _enemyMat.DOColor(Color.white, "_EmissionColor", 0.01f).OnComplete(() =>
                    {
                        _enemyMat.DOColor(Color.red, "_EmissionColor", 0.1f).OnComplete(() =>
                        {
                            _enemyMat.DOColor(Color.black, "_EmissionColor", 0);
                        });
                    });
                });

            });
        }
        public void PlayAttack(string targetAttack)
        {
            _anim.Play(targetAttack, 1, 0);
            //_anim.SetTrigger(targetAttack);
        }

        public void PlayShoot(string targetShoot)
        {
            //_anim.Play(targetShoot, 1, 0);
            _anim.SetTrigger(targetShoot);
        }

    }

}
