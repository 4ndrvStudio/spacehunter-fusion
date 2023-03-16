using UnityEngine;

namespace SH
{
    public class AnimationEvent : MonoBehaviour
    {
        [SerializeField] private WeaponHook _hook = default;

        [SerializeField] private PlayerMovement _movement = default;

        public void EnableBox()
        {
            _hook.EnableBoxCol();
        }

        public void DisableBox()
        {
            _hook.DisableBoxCol();
        }

        public void PlayEffect()
        {
            _movement.PlayEffectAttack();
        }
    }
}
