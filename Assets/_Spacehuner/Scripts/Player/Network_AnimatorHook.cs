using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_AnimatorHook : NetworkBehaviour
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private Network_WeaponCollider _weaponColider;
        [SerializeField] private List<GameObject> _attackVFXList;
        [SerializeField] private List<GameObject> _comboVFXList;


        private WeaponDissolve _weaponDissolve;


        //time active weapon
        [SerializeField] private float _weaponExistTime = 3f;

        [SerializeField] private float m_weaponExistTime;

        void Awake()
        {

            m_weaponExistTime = 0;
            _anim = gameObject.GetComponent<Animator>();

        }

        void FixedUpdate()
        {
            if (_weaponDissolve != null)
            {
                if (_weaponDissolve.IsDissolved == false)
                {
                    _anim.SetLayerWeight(2, 0.75f);
                    m_weaponExistTime -= Time.fixedDeltaTime;

                    if (m_weaponExistTime <= 0)
                    {
                        m_weaponExistTime = _weaponExistTime;

                        _weaponDissolve.DissolveWeapon();

                    }
                }
                else
                {
                    _anim.SetLayerWeight(2, 0f);
                }
            }




        }

        public void SetWeaponCollider(Network_WeaponCollider weaponColider)
        {
            _weaponColider = weaponColider;
        }

        public void SetDissolve(Weapon weapon) => _weaponDissolve = weapon.WeaponDissolve;

        public void SetComboVFXList(List<GameObject> comboVFX)
        {
            _comboVFXList = comboVFX;
        }

        public void EnableCollider(CanHitName canHitName)
        {
            _weaponDissolve.ActiveWeapon();

            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, true);
        }

        public void DisableCollider(CanHitName canHitName)
        {
            m_weaponExistTime = _weaponExistTime;
            _weaponColider.ToggleActiveCollider(CanHitName.Mineral, false);
        }

        public void EnableVFX(int index)
        {
            GameObject targetFX = _attackVFXList[index];
            GameObject fx = Instantiate(targetFX, targetFX.transform.position, targetFX.transform.rotation);
            fx.SetActive(true);
        }

        public void EnableComboVFX(int index)
        {
            GameObject targetFX = _comboVFXList[index];
            GameObject fx = Instantiate(targetFX, targetFX.transform.position, targetFX.transform.rotation);
            fx.SetActive(true);
        }



    }

}
