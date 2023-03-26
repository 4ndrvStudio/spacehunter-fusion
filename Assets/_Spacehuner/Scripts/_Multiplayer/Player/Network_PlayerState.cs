using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{

    public class Network_PlayerState : NetworkBehaviour
    {
        public Animator Anim;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;


        [Networked(OnChanged = nameof(OnIsActionChanged))]
        [HideInInspector] public NetworkBool N_IsAction { get; set; }
        public bool L_IsAction;

        [Networked(OnChanged = nameof(OnIsGroundedChanged))]
        [HideInInspector] public NetworkBool N_IsGrounded { get; set; }
        public bool L_IsGrounded;

        [Networked(OnChanged = nameof(OnIsComboChanged))]
        [HideInInspector] public NetworkBool N_IsCombo { get; set; }
        public bool L_IsCombo;


        // Action State    

        static void OnIsActionChanged(Changed<Network_PlayerState> changed)
        {
            changed.Behaviour.OnIsActionChanged();
        }
        private void OnIsActionChanged()
        {
            L_IsAction = N_IsAction;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetIsAction(bool isAction, RpcInfo info = default)
        {
            this.N_IsAction = isAction;
        }

        // Combo State   
        static void OnIsComboChanged(Changed<Network_PlayerState> changed)
        {
            changed.Behaviour.OnIsComboChanged();
        }
        private void OnIsComboChanged()
        {
            L_IsCombo = N_IsCombo;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetIsCombo(bool isCombo, RpcInfo info = default)
        {
            this.N_IsCombo = isCombo;
        }


        // Jump State

        static void OnIsGroundedChanged(Changed<Network_PlayerState> changed)
        {
            changed.Behaviour.OnIsGroundedChanged();
        }
        private void OnIsGroundedChanged()
        {
            L_IsGrounded = N_IsGrounded;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetIsGrounded(bool isGrounded, RpcInfo info = default)
        {
            this.N_IsGrounded = isGrounded;
        }

        public override void FixedUpdateNetwork()
        {
            

        }
        public override void Render()
        {
            if (Object.HasInputAuthority == false) return;
            RPC_SetIsGrounded(GroundCheck());
            if(Anim == null) return;
            RPC_SetIsAction(Anim.GetBool("isAction"));

            RPC_SetIsCombo(Anim.GetCurrentAnimatorStateInfo(3).IsTag("Combo"));


        }


        private bool GroundCheck() => Physics.CheckSphere(_groundCheck.position, 0.15f, _groundMask);


    }

}
