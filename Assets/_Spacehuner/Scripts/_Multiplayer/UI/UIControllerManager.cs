using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SH.Multiplayer
{
    public class UIControllerManager : MonoBehaviour
    {
        public static UIControllerManager Instance;

        [SerializeField] private Joystick _movementJoy;
        [SerializeField] private UIButtonCustom _attackBtn;
        [SerializeField] private UIButtonCustom _jumpBtn;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public void ActiveController(bool isActive)
        {
            _movementJoy.gameObject.SetActive(isActive);
            _attackBtn.gameObject.SetActive(isActive);
            _jumpBtn.gameObject.SetActive(isActive);
        }

        public Joystick GetMovementJoystick() => _movementJoy;

        public bool GetAttackBtn() => _attackBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetJumpBtn() => _jumpBtn.GetComponent<UIButtonCustom>().IsPressed;

    }

}
