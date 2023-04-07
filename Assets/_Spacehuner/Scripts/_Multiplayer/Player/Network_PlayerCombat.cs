using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using DG.Tweening;

namespace SH.Multiplayer
{
    public class Network_PlayerCombat : NetworkBehaviour, ISpawned
    {
        [SerializeField] private Network_PlayerState _playerState;
        [SerializeField] private Network_PlayerAimingAssistant _playerAim;



        [Networked, HideInInspector]
        public int AttackCount { get; set; }

        public bool HasAttack { get; private set; }

        [Networked, HideInInspector]
        public int Combo1Count { get; set; }

        public bool HasCombo1 { get; private set; }

        [Networked, HideInInspector]
        public int DashAttackCount {get; set; }

        public bool HasDashAttack { get; private set; }



        [Networked]
        private NetworkButtons _lastButtonsInput { get; set; }

        private Interpolator<int> _attackCountInterpolator;

        private Interpolator<int> _combo1CountInterpolator;

        private Interpolator<int> _dashCountInterpolator;


        public string[] AttackName;
        public string Combo1Name;
        public string DashAttackName;


        [Networked(OnChanged = nameof(OnIndexAttackChanged))]
        public byte N_IndexAttack { get; set; }
        public int L_IndexAttack;


        [Networked] private TickTimer _avoidAttackTime { get; set; }

        //aiming assistant   
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _attackMoveDuration;
        [SerializeField] private float _attackMoveDis;


        public override void Spawned()
        {
            _attackCountInterpolator = GetInterpolator<int>(nameof(AttackCount));
            _combo1CountInterpolator = GetInterpolator<int>(nameof(Combo1Count));
            _dashCountInterpolator = GetInterpolator<int>(nameof(DashAttackCount));

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

                    if(Runner.IsServer == false) {

                        RPC_SetIndexAttack(L_IndexAttack);

                    }

                    //AimSupport();
                    AttackCount++;
                    
                    _avoidAttackTime = TickTimer.CreateFromSeconds(Runner, 0.2f);
                }

            }

            HasCombo1 = input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.Combo1);
            HasDashAttack = input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.DashAttack);

            if(HasCombo1) {

                Combo1Count++;

            }

            if(HasDashAttack) {
                DashAttackCount++;
            }

            _lastButtonsInput = input.Buttons;

        }

        
        void AimSupport()
        {
            if (_playerAim.SelectedNearest != null)
            {

                //Rotate support
                Vector3 from = _playerAim.SelectedNearest.transform.position - transform.position;
                Vector2 to = transform.forward;
                from.y = 0;
                var rotation = Quaternion.LookRotation(from);
                transform.DORotateQuaternion(rotation, 0f);

                //translate support
                Vector3 targetPos = _playerAim.SelectedNearest.transform.position + ((transform.position - _playerAim.SelectedNearest.transform.position).normalized * 1.2f);
                targetPos.y = transform.position.y;
                transform.DOMove(targetPos, _attackMoveDuration);
            }
            else
            {

                // RaycastHit hit;
                // if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _attackMoveDis, _obstacleMask))
                // {
                //     transform.DOMove(transform.position + transform.forward * _attackMoveDis, _attackMoveDuration);
                // }
               
            }
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
