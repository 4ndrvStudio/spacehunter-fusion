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

        
        private int _lastVisibleJump;
        private int _lastVisibleAttack;

        // NetworkBehaviour INTERFACE

        public override void Spawned()
        {
            _lastVisibleJump = _playerMovement.JumpCount;
            _lastVisibleAttack = _playerCombat.AttackCount;

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


            Anim.SetFloat("movement", _playerMovement.Speed);

            Anim.SetBool("onGround", _playerState.L_IsGrounded);

   
        }

    }

}
