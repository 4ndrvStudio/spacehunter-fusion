using SH.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _anim = default;

        public void SetMove(float currentSpeed)
        {
            _anim.SetFloat(AnimationParam.Vertical, currentSpeed, 0.2f, Time.deltaTime);
        }

        public void SetMining()
        {
            _anim.CrossFade(AnimationParam.Mining, 0.2f);
        }

        public void SetJump(bool isJump)
        {
            _anim.SetBool(AnimationParam.OnGround, isJump);
        }

        public void SetAttack()
        {
            _anim.Play("attack_onehand_1");
        }

        public void SetDash()
        {
            _anim.Play("dash");
        }
    }
}
