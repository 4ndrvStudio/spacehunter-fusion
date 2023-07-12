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

    public class PlayerAnimation : NetworkBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private PlayerState _playerState;
        [SerializeField] private PlayerCombat _playerCombat;
        [SerializeField] private PlayerDamageable _playerDamageable;

        //Attack
        [SyncVar(WritePermissions = WritePermission.ServerOnly)]
        private int _normalAttackIndex;
        [SerializeField] private List<string> _normalAttackList;
        private int _lastAttack1Count;
        private int _lastSkill1Count;

        //TakeDame
        private int _lastTakeDamageCount;


        private void Awake()
        {
            InstanceFinder.TimeManager.OnUpdate += TimeManager_OnTick;

        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            _lastAttack1Count = _playerCombat.Attack1Count;
            _lastTakeDamageCount = _playerDamageable.TakeDameCount;
            _lastSkill1Count = _playerCombat.Skill1Count;
        }

        private void OnDestroy()
        {
            if (InstanceFinder.TimeManager != null)
            {
                InstanceFinder.TimeManager.OnUpdate -= TimeManager_OnTick;
            }
        }

        private void TimeManager_OnTick()
        {
            RenderCombat();
  

        }

        public void PlayAttack()
        {
            _normalAttackIndex++;

            if (_normalAttackIndex >= _normalAttackList.Count)
                _normalAttackIndex = 0;

            _anim.Play(_normalAttackList[_normalAttackIndex], 3, 0);
        }

        public void PlaySkill()
        {
            _anim.Play("skill1", 3, 0);

        }

        public void PlayGetHit()
        {
            //_anim.Play("gethit", 3, 0);
        }

        public void RenderCombat()
        {
            //render attack 
            if (_playerCombat.Attack1Count > _lastAttack1Count)
            {
                PlayAttack();
            }
            _lastAttack1Count = _playerCombat.Attack1Count;

            if (_playerCombat.Skill1Count > _lastSkill1Count)
            {
                PlaySkill();
            }
            _lastSkill1Count = _playerCombat.Skill1Count;

            //Render TakeDamage 
            if (_playerDamageable.TakeDameCount > _lastTakeDamageCount)
            {

                PlayGetHit();
            }
            _lastTakeDamageCount = _playerDamageable.TakeDameCount;

        }






    }

}

