using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer.Player
{
    
    public class PlayerAnimatorHook : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _normalAttackVfxList = new();
        [SerializeField] private WeaponCollider _weaponCollider;
        public void OnEnableVFX(int index) {
            var targetObject = _normalAttackVfxList[index];
            GameObject vfx = Instantiate( targetObject, targetObject.transform.position,targetObject.transform.rotation);
            vfx.SetActive(true);
            Destroy(vfx,0.14f);
            _weaponCollider.CanDamge = true;
        }

        public void OnDisableVFX(int index) {
            //_normalAttackVfxList[index].SetActive(false);

            _weaponCollider.CanDamge = false;
        }
    }

}
