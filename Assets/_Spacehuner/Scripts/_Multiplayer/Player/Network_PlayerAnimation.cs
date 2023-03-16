using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerAnimation : NetworkBehaviour
    {
        [SerializeField] private Network_PlayerMovement _playerMovement;
        [SerializeField] private Animator _anim;
        private int _lastVisibleJump;

        // NetworkBehaviour INTERFACE

        public override void Spawned()
        {
            _lastVisibleJump = _playerMovement.JumpCount;

        }

        public override void Render()
        {
            UpdateAnimations();
        }


        // PRIVATE METHODS

        private void UpdateAnimations()
        {
            if (_lastVisibleJump < _playerMovement.JumpCount)
			{
				_anim.SetBool("onGround", _playerMovement.HasJumped);
                
			}
			else if (_lastVisibleJump > _playerMovement.JumpCount)
			{
				_anim.SetBool("onGround", _playerMovement.HasJumped);
			}

			_lastVisibleJump = _playerMovement.JumpCount;

            _anim.SetFloat("movement", _playerMovement.Speed);
   
        }

    }

}
