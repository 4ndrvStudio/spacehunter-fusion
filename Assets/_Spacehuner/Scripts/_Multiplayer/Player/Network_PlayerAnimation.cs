using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerAnimation : NetworkBehaviour
    {
        [SerializeField] private Animator _anim;
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
            UpdateAnimations();
        }


        // PRIVATE METHODS

        private void UpdateAnimations()
        {
          

            //Render Attack
            if (_lastVisibleAttack < _playerCombat.AttackCount)
			{
                if(_playerState.L_IsGrounded) {
                    _anim.SetTrigger(_playerCombat.AttackName[_playerCombat.L_IndexAttack]);
                } else {
                    
                }
				
			}
			else if (_lastVisibleAttack > _playerCombat.AttackCount)
			{
				//cancel attack
			}

            _lastVisibleAttack = _playerCombat.AttackCount;


            _anim.SetFloat("movement", _playerMovement.Speed);

            _anim.SetBool("onGround", _playerState.L_IsGrounded);

   
        }

    }

}
