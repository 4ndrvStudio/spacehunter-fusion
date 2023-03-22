using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{

    public class Network_PlayerState : NetworkBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;


        [Networked(OnChanged = nameof(OnIsActionChanged))]
        [HideInInspector] public NetworkBool N_IsAction { get; set; }
        public bool L_IsAction;

        [Networked(OnChanged = nameof(OnIsGroundedChanged))]
        [HideInInspector] public NetworkBool N_IsGrounded { get; set; }
        public bool L_IsGrounded;


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
            RPC_SetIsAction(_anim.GetBool("isAction"));
        }


        private bool GroundCheck() => Physics.CheckSphere(_groundCheck.position, 0.15f, _groundMask);


    }

}
