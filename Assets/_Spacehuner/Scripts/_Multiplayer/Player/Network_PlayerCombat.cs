using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerCombat : NetworkBehaviour, ISpawned
    {
        [Networked, HideInInspector]
        public int AttackCount { get; set; }

        public bool HasAttack { get; private set; }

        [Networked]
        private NetworkButtons _lastButtonsInput { get; set; }

        private Interpolator<int> _attackCountInterpolator;


        public string[] AttackName;

        [Networked(OnChanged = nameof(OnIndexAttackChanged))]
        public byte IndexAttack { get; set; }
        public int LocalAttack;

        
        public override void Spawned()
        {
            _attackCountInterpolator = GetInterpolator<int>(nameof(AttackCount));

        }

        public override void FixedUpdateNetwork()
        {
            if (Object.IsProxy == true)
                return;

            
            var input = GetInput<PlayerInput>();

            if (input.HasValue == true)
            {
                Attack(input.Value);
            }
        }

        public void Attack(PlayerInput input) {
           
            HasAttack = input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.Attack);
            
            if(HasAttack) {
                
                LocalAttack++;

                if(LocalAttack >= AttackName.Length) LocalAttack = 0;

                RPC_SetIndexAttack(LocalAttack);

                AttackCount++;
            }
            _lastButtonsInput = input.Buttons;

        }



        static void OnIndexAttackChanged(Changed<Network_PlayerCombat> changed)
        {
            changed.Behaviour.OnIndexAttackChanged();
        }
        private void OnIndexAttackChanged()
        {
           // Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
            LocalAttack = IndexAttack;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetIndexAttack(int indexAttack, RpcInfo info = default)
        {
            this.IndexAttack = (byte) indexAttack;
        }

    }

}
