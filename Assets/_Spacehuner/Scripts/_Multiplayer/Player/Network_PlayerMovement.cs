using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerMovement : NetworkBehaviour, ISpawned
    {
        // PUBLIC MEMBERS

        [Networked, HideInInspector]
        public int JumpCount { get; set; }
        [Networked, HideInInspector]
        public float Speed { get; set; }

        public bool HasJumped { get; private set; }

        public float InterpolatedSpeed => _speedInterpolator.Value;

        // PRIVATE MEMBERS
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        [SerializeField] private float _turnSmoothVelocity;
        [SerializeField] private float _speedSmoothTime = 0.1f;
        [SerializeField] private Transform _lookPoint;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private NetworkRigidbody _rigid;
        [SerializeField] private bool isSetCam;
        [SerializeField] private GameObject _camera;


        // c# out
        private float _speedSmoothVelocity;
        private float _currentSpeed;

        [Networked]
        private NetworkButtons _lastButtonsInput { get; set; }

        private Interpolator<int> _jumpCountInterpolator;
        private Interpolator<float> _speedInterpolator;

        // NetworkBehaviour INTERFACE

        public override void Spawned()
        {
            _jumpCountInterpolator = GetInterpolator<int>(nameof(JumpCount));
            _speedInterpolator = GetInterpolator<float>(nameof(Speed));
            
        }
        void Update() {

        }

        public override void FixedUpdateNetwork()
        {
            if (Object.IsProxy == true)
                return;

            
            var input = GetInput<PlayerInput>();

            if (input.HasValue == true)
            {
                Movement(input.Value);
            }
        }


        private void Movement(PlayerInput input)
        {   

            Vector3 mainCamEuler = input.CameraEuler;

            Vector2 moveDir = input.MoveDirection.normalized;


            if (moveDir != Vector2.zero )
            {
              
                float targetRotation = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg + mainCamEuler.y;
                
                Quaternion targetQuaternion = Quaternion.Euler(0f, targetRotation, 0f);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, _turnSmoothTime * Runner.DeltaTime);
            }


            Speed = input.MoveDirection.magnitude * _speed;

            if (moveDir != Vector2.zero)
            {
                transform.Translate(transform.forward * Speed * Runner.DeltaTime, Space.World);
            }


            if (input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.Jump))
            {
                if (GroundCheck())
                {
                    HasJumped = true;
                    JumpCount++;
                    _rigid.Rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
                }
                else
                {
                    HasJumped = false;
                }

            }

            HasJumped = !GroundCheck();


            _lastButtonsInput = input.Buttons;

        }

        private bool GroundCheck() => Physics.CheckSphere(_groundCheck.position, 0.15f, _groundMask);


    }
}






