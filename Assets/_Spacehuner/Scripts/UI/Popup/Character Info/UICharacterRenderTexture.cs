using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SH.UI
{
    public class UICharacterRenderTexture : MonoBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private Camera _cam;
        [SerializeField] private WeaponDissolve _weaponDissovle;
        

        public void SetHideWeapon() {
            _weaponDissovle.HideWeapon();
        }

        public void SetToWeapon() {
            _anim.SetBool("isWeaponEquipped", true);
            DOTween.To(() =>_cam.orthographicSize , x => _cam.orthographicSize = x, 1f,0.8f)
                .OnComplete(()=> {
                    _weaponDissovle.UnDissolveWeapon();
                });
        }
        public void SetToIdle(bool required) {
            if(required == true) {
                _weaponDissovle.HideWeapon();
            } else {
                _weaponDissovle.DissolveWeapon();
            }

            _anim.SetBool("isWeaponEquipped", false);
            DOTween.To(() =>_cam.orthographicSize , x => _cam.orthographicSize = x, 1.2f,0.8f);
        }
        
      
    }

}
