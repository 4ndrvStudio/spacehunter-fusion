using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SH.Multiplayer.Player
{
    using FishNet;
    using FishNet.Object;
    using FishNet.Object.Prediction;
    using FishNet.Transporting;
    using FishNet.Object.Synchronizing;
    public class PlayerCombat : NetworkBehaviour
    {
        
        public struct AttackData : IReplicateData
        {
            public bool Attack1Button;
            public bool Skill1Button;

            private uint _tick;
            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }
        public struct AttackReconcileData : IReconcileData
        {
            public int Attack1Count;
            public int Skill1Count;
            public AttackReconcileData(int attack1Count, int skill1Count)
            {
                Attack1Count = attack1Count;
                Skill1Count = skill1Count;
                _tick = 0;
            }

            private uint _tick;
            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }

        //client state
        [SerializeField] private AttackData _clientAttackData;
        [SerializeField] private PlayerState _playerState;

        [SyncVar(WritePermissions = WritePermission.ClientUnsynchronized)]
        public int Attack1Count;

        [SyncVar(WritePermissions = WritePermission.ClientUnsynchronized)]
        public int Skill1Count;

        private void Awake()
        {
            InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
        }

        private void OnDestroy()
        {
            if (InstanceFinder.TimeManager != null)
            {
                InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
            }
        }

        private void TimeManager_OnTick()
        {

            if (base.IsOwner)
            {
                Reconciliation(default, false);
                CheckInput(out AttackData attackData);
                Move(attackData, false);
            }
            if (base.IsServer)
            {
                Move(default, true);
                AttackReconcileData attackReconcileData = new AttackReconcileData();
                Reconciliation(attackReconcileData, true);
            }
        }

        private void CheckInput(out AttackData attackData)
        {
            attackData = default;

            bool attack1Button = _playerState.IsOnGround == false ? false : Input.GetMouseButton(0);
            bool skill1Button = Input.GetMouseButton(1);

            if ((attack1Button == false &&  skill1Button == false) || _playerState.IsAttack == true || _playerState.IsAbility == true)
                return;

            attackData = new AttackData()
            {
                Attack1Button = attack1Button,
                Skill1Button = skill1Button
            };
        }

        [Replicate]
        private void Move(AttackData attackData, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
        {

            if (attackData.Attack1Button == true)
            {
                Attack1Count += 1;
            }
             if (attackData.Skill1Button == true)
            {
                Skill1Count += 1;
            }

        }

        [Reconcile]
        private void Reconciliation(AttackReconcileData attackReconcileData, bool asServer, Channel channel = Channel.Unreliable)
        {
            Attack1Count = attackReconcileData.Attack1Count;
            Skill1Count = attackReconcileData.Skill1Count;

        }


    }
}