using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_TestMode : NetworkBehaviour
    {
        public Animator Anim;
        [SerializeField] private Network_PlayerMovement _playerMovement;

        [SerializeField] private List<GameObject> _comboVFXList;
        public List<GameObject> ComboVFXList => _comboVFXList;

        [SerializeField] private float _speed = 7f;

        



        [Networked(OnChanged = nameof(OnIsTestModeChanged))]
        [HideInInspector] public NetworkBool N_IsTestMode { get; set; }
        public bool L_IsTestMode;

        public int PressCount = 0;


        private NetworkButtons _lastButtonsInput { get; set; }


        public override void FixedUpdateNetwork()
        {
            if (Object.HasInputAuthority == false) return;

            if (L_IsTestMode == false)
            {
                var input = GetInput<PlayerInput>();

                if (input.HasValue == true)
                {
                    ActiveTestMode(input.Value);
                }

                if (PressCount >= 20)
                {
                    RPC_SetIsTestMode(true);
                }
            }
        }

        public void ActiveTestMode(PlayerInput input)
        {

            // bool HasPressed = input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.ActiveTestMode);

            // if (HasPressed)
            // {
            //     PressCount++;

            // }

            // _lastButtonsInput = input.Buttons;


        }


        static void OnIsTestModeChanged(Changed<Network_TestMode> changed)
        {
            changed.Behaviour.OnIsTestModeChanged();
        }
        private void OnIsTestModeChanged()
        {
            L_IsTestMode = N_IsTestMode;

            if (L_IsTestMode == true)
            {
                if (Object.HasInputAuthority)
                {
                    //UIControllerManager.Instance.ActiveTestModeController(true);

                }
                //Anim.SetLayerWeight(1, 1);

                //_playerMovement.SetSpeed(_speed);



            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetIsTestMode(bool isTestMode, RpcInfo info = default)
        {
            this.N_IsTestMode = isTestMode;
        }


    }

}
