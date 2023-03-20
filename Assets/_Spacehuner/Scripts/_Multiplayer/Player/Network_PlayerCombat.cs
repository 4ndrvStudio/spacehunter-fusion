using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerCombat : NetworkBehaviour, ISpawned
    {
        [SerializeField] private Network_PlayerState _playerState;


        [Networked, HideInInspector]
        public int AttackCount { get; set; }

        public bool HasAttack { get; private set; }

        [Networked]
        private NetworkButtons _lastButtonsInput { get; set; }

        private Interpolator<int> _attackCountInterpolator;


        public string[] AttackName;

        [Networked(OnChanged = nameof(OnIndexAttackChanged))]
        public byte N_IndexAttack { get; set; }
        public int L_IndexAttack;

        [Networked] private TickTimer _avoidAttackTime { get; set; }



        public override void Spawned()
        {
            _attackCountInterpolator = GetInterpolator<int>(nameof(AttackCount));

            _avoidAttackTime = TickTimer.CreateFromSeconds(Runner, 0);

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

        public void Attack(PlayerInput input)
        {

            if (_avoidAttackTime.Expired(Runner))
            {

                HasAttack = input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.Attack);


                if (HasAttack && !_playerState.L_IsAction)
                {

                    L_IndexAttack++;

                    if (L_IndexAttack >= AttackName.Length) L_IndexAttack = 0;

                    RPC_SetIndexAttack(L_IndexAttack);

                    AttackCount++;
                    _avoidAttackTime = TickTimer.CreateFromSeconds(Runner, 0.2f);
                }

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
            L_IndexAttack = N_IndexAttack;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetIndexAttack(int indexAttack, RpcInfo info = default)
        {
            this.N_IndexAttack = (byte)indexAttack;
        }



    }

}
