using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SH
{
    public class UIControllerManager : MonoBehaviour
    {
        public static UIControllerManager Instance;

        [SerializeField] private Joystick _movementJoy;
        [SerializeField] private Button _attackBtn;
        [SerializeField] private Button _jumpBtn;
        [SerializeField] private Button _dashBtn;

        void Awake() {
           if(Instance == null) {
                Instance = this;
           } 
        }


        public Joystick GetMovementJoystick() => _movementJoy;


    }

}
