using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerStats : NetworkBehaviour
    {

        [SerializeField] private Network_PlayerDamageable _playerDamageable;
        [SerializeField] private Network_PlayerState _playerState;
        [SerializeField] private Network_PlayerAnimation   _playerAnimation;
        private int _lastVisibleGetHit;

        public int HP = 20;
        private bool WasSetup;

        public override void Spawned()
        {

            _lastVisibleGetHit = _playerDamageable.HitCount;

            UIControllerManager.Instance.SetHP(HP);
            WasSetup = true;
        }
        public override void Render()
        {

            if(WasSetup != true) 
                return;

            RenderCombatInteract();
        }


        private void RenderCombatInteract()
        {
            
            //gethit
            if (_lastVisibleGetHit < _playerDamageable.HitCount)
            {
                GetDame();
            }
            else if (_lastVisibleGetHit > _playerDamageable.HitCount)
            {

            }
            _lastVisibleGetHit = _playerDamageable.HitCount;


        }

        public void GetDame()
        {
            
            HP -= 1;
            if(HP <= 0) {
              
                _playerAnimation.PlayDeathAnimation();
                UIControllerManager.Instance.HideAllController();
                UIManager.Instance.HidePopup(PopupName.Inventory);
                _playerState.RPC_SetIsDeath(true);
            }
            UIControllerManager.Instance.SetHP(HP);
           
        }

    }

}
