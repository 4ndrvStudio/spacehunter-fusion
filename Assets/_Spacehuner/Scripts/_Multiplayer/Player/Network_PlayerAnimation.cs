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

        
        private int _lastVisibleJump;
        private int _lastVisibleAttack;
        private int _lastCombo1Attack;
        private int _lastVisibleGetHit;

        // NetworkBehaviour INTERFACE

        public override void Spawned()
        {
            _lastVisibleJump = _playerMovement.JumpCount;
            _lastVisibleAttack = _playerCombat.AttackCount;
            _lastCombo1Attack = _playerCombat.Combo1Count;
            _lastVisibleGetHit = _playerDamageable.HitCount;


        }

        public override void Render()
        {
            if(Anim == null) return;
             UpdateAnimations();
        }


        // PRIVATE METHODS

        private void UpdateAnimations()
        {
          

            //Render Attack
            if (_lastVisibleAttack < _playerCombat.AttackCount)
			{
                if(_playerState.L_IsGrounded) {
                    Anim.SetTrigger(_playerCombat.AttackName[_playerCombat.L_IndexAttack]);
                } else {
                    
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
                if(_playerState.L_IsGrounded) {
                    Anim.Play(_playerCombat.Combo1Name,3,0);
                } else {
                    
                }
				
			}
			else if (_lastCombo1Attack > _playerCombat.Combo1Count)
			{

			}


            _lastCombo1Attack = _playerCombat.Combo1Count;

            //gethit
            if (_lastVisibleGetHit < _playerDamageable.HitCount)
			{
                 Anim.Play("GetHit1",3,0);
			}
			else if (_lastVisibleGetHit > _playerDamageable.HitCount)
			{

			}


            _lastVisibleGetHit = _playerDamageable.HitCount;




            Anim.SetFloat("movement", _playerMovement.Speed);

            Anim.SetBool("onGround", _playerState.L_IsGrounded);

   
        }

    }

}
