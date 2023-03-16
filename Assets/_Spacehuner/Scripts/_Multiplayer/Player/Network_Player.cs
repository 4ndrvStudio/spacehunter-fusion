using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;
using TMPro;
using SH.PlayerData;

namespace SH.Multiplayer
{
    public class Network_Player : NetworkBehaviour, IPlayerLeft
    {
        public static Network_Player Local { get; set; }

        [SerializeField] private Transform _body;
        [SerializeField] private Transform _lookPoint;

        [Networked(OnChanged = nameof(OnNickNameChanged))]
        public NetworkString<_16> nickName { get; set; }

        //Display Data 
        [SerializeField] private TextMeshPro _nameUI;

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Local = this;
                Network_CameraManager.Instance.SetAimTarget(_body, _lookPoint);
                RPC_SetNickName(PlayerDataManager.DisplayName);
            }

        }



        public void PlayerLeft(PlayerRef player)
        {
            if (player == Object.InputAuthority)
                Runner.Despawn(Object);
        }

        static void OnNickNameChanged(Changed<Network_Player> changed)
        {
            Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickName}");

            changed.Behaviour.OnNickNameChanged();
        }

        private void OnNickNameChanged()
        {
            Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
            _nameUI.text = nickName.ToString();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetNickName(string nickName, RpcInfo info = default)
        {
            Debug.Log($"[RPC] SetNickName {nickName}");
            this.nickName = nickName;
        }
    }

}
