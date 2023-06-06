using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerAnimation : NetworkBehaviour
    {
        public Animator Anim;
        [SerializeField] private Network_PlayerState _playerState;
        [SerializeField] private Network_PlayerMovement _playerMovement;
        [SerializeField] private Network_PlayerCombat _playerCombat;
        [SerializeField] private Network_PlayerDamageable _playerDamageable;
        [SerializeField] private Network_WeaponManager _weaponManager;

        private int _lastVisibleJump;
        private int _lastVisibleAttack;
        private int _lastCombo1Attack;
        private int _lastDashAttack;
        private int _lastVisibleGetHit;

        // NetworkBehaviour INTERFACE

        public override void Spawned()
        {
            _lastVisibleJump = _playerMovement.JumpCount;
            _lastVisibleAttack = _playerCombat.AttackCount;
            _lastCombo1Attack = _playerCombat.Combo1Count;
            _lastDashAttack = _playerCombat.DashAttackCount;
            _lastVisibleGetHit = _playerDamageable.HitCount;
        }

        public override void Render()
        {
            if (Anim == null) return;
            UpdateAnimations();
        }


        // PRIVATE METHODS

        private void UpdateAnimations()
        {
            if(_playerState.L_IsDeath == true) return;

            RenderAttack();
            RenderCombatInteract();
            RenderMovement();
            //RenderWeaponManager();

        }

        private void RenderAttack()
        {

            //Render Attack
            if (_lastVisibleAttack < _playerCombat.AttackCount)
            {
                if (_playerState.L_IsGrounded)
                {
                    Anim.SetTrigger(_playerCombat.AttackName[_playerCombat.L_IndexAttack]);
                }
                else
                {

                }

            }
            else if (_lastVisibleAttack > _playerCombat.AttackCount)
            {
                //cancel attack
            }


            _lastVisibleAttack = _playerCombat.AttackCount;

            //combo
            if (_lastCombo1Attack < _playerCombat.Combo1Count)
            {
                if (_playerState.L_IsGrounded)
                {
                    Anim.Play(_playerCombat.Combo1Name, 3, 0);
                }
                else
                {

                }

            }
            else if (_lastCombo1Attack > _playerCombat.Combo1Count)
            {

            }


            _lastCombo1Attack = _playerCombat.Combo1Count;

            //dash attack
            if (_lastDashAttack < _playerCombat.DashAttackCount)
            {
                if (_playerState.L_IsGrounded)
                {
                    Anim.Play(_playerCombat.DashAttackName, 3, 0);
                }
                else
                {

                }

            }
            else if (_lastDashAttack > _playerCombat.DashAttackCount)
            {

            }


            _lastDashAttack = _playerCombat.DashAttackCount;

        }

        private void RenderCombatInteract()
        {

            //gethit
            if (_lastVisibleGetHit < _playerDamageable.HitCount)
            {
                if (!Anim.GetCurrentAnimatorStateInfo(3).IsName("GetHit1"))
                {
                    Anim.Play("GetHit1", 3, 0);
                }


            }
            else if (_lastVisibleGetHit > _playerDamageable.HitCount)
            {

            }
            _lastVisibleGetHit = _playerDamageable.HitCount;

        }

        public void PlayDeathAnimation() {
            Anim.Play("IsDeath", 3, 0);
        }

        private void RenderMovement()
        {
            Anim.SetFloat("movement", _playerMovement.Speed);

            Anim.SetBool("onGround", _playerState.L_IsGrounded);

            Anim.applyRootMotion = _playerState.L_IsCombo || _playerState.L_IsAction;

            if ((_playerState.L_IsCombo || _playerState.L_IsAction || _playerState.L_IsDash) && !_playerState.L_IsMining)
            {
                float offset = _playerState.L_IsDash ? 1.5f : 1f;
                transform.position += Anim.deltaPosition * offset;
            }
        }

        private void RenderWeaponManager()
        {

            // float targetWeightRightHandLayer = _weaponManager.L_HasEquipWeapon == true ? 0.75f : 0f;

            // Anim.SetLayerWeight(2,targetWeightRightHandLayer);
        }




    }

}
