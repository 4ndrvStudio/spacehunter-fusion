using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;

namespace SH.Multiplayer
{
    public class Network_PlayerStats : NetworkBehaviour, IDespawned
    {

        [SerializeField] private Network_PlayerDamageable _playerDamageable;
        [SerializeField] private Network_PlayerState _playerState;
        [SerializeField] private Network_PlayerAnimation _playerAnimation;
        private int _lastVisibleGetHit;

        public int HP = 20;
        private bool WasSetup;

        public static UnityAction PlayerDeath;

        public override void Spawned()
        {
            _lastVisibleGetHit = _playerDamageable.HitCount;
            UIControllerManager.Instance.SetHP(HP);
            WasSetup = true;

            //event
            PlayerDeath += OnClaimed;
        }

        void IDespawned.Despawned(Fusion.NetworkRunner runner, bool hasState) {
             PlayerDeath -= OnClaimed;
        }
        

        public override void Render()
        {

            if (WasSetup != true)
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
            if (HP <= 0)
            {
                Death();
            }
            UIControllerManager.Instance.SetHP(HP);
        }
        
        public void Death()
        {
            _playerAnimation.PlayDeathAnimation();
            UIManager.Instance.HidePopup(PopupName.Inventory);
            UIControllerManager.Instance.HideAllController();
            _playerState.RPC_SetIsDeath(true);

            //Death in Mining Room
            if((int)Runner.CurrentScene == 3) {
                //UIManager.Instance.ShowPopupWithCallback()
            }

        }

        public void OnClaimed() {

        }

    }

}