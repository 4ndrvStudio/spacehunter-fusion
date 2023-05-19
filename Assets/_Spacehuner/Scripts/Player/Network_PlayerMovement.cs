using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_PlayerMovement : NetworkBehaviour, ISpawned
    {

        //Component
        [SerializeField] private Network_PlayerState _playerState;
        [SerializeField] private NetworkRigidbody _rigid;

        //Network
        [Networked, HideInInspector]
        public int JumpCount { get; set; }
        [Networked, HideInInspector]
        public float Speed { get; set; }

        public bool HasJumped { get; private set; }

        public float InterpolatedSpeed => _speedInterpolator.Value;


        // config
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpHeight = 5f;
        [SerializeField] private float _turnSmoothTime = 0.1f;
        [SerializeField] private float _turnSmoothVelocity;
        // [SerializeField] private float _speedSmoothTime = 0.1f;
        [SerializeField] private Transform _lookPoint;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;



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

            Speed = input.MoveDirection.magnitude;

            if (Speed > 0.15f)
            {
                if (!_playerState.L_IsCombo && !_playerState.L_IsMining)
                {
                    float targetRotation = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg + mainCamEuler.y;
                    Quaternion targetQuaternion = Quaternion.Euler(0f, targetRotation, 0f);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, _turnSmoothTime * Runner.DeltaTime);
                }
            }


            float targetspeed = input.MoveDirection.magnitude * _speed;

            if (Speed > 0.15f && !_playerState.L_IsAction && !_playerState.L_IsMining)
            {
                //transform.Translate(transform.forward * targetspeed * Runner.DeltaTime, Space.World);
                //_rigid.Rigidbody.MovePosition(transform.position + transform.forward * targetspeed * Runner.DeltaTime);
                MoveWithCollisionCheck(transform.forward * targetspeed * Runner.DeltaTime, targetspeed * Runner.DeltaTime);
           
            }


            if (input.Buttons.WasPressed(_lastButtonsInput, EInputButtons.Jump))
            {
                if (_playerState.L_IsGrounded && !_playerState.L_IsAction && !_playerState.L_IsMining)
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

            HasJumped = !_playerState.L_IsGrounded;


            _lastButtonsInput = input.Buttons;

        }

        private void MoveWithCollisionCheck(Vector3 moveDirection, float moveDistance)
        {
            // Kiểm tra va chạm trước khi di chuyển
            RaycastHit hit;
            if (_rigid.Rigidbody.SweepTest(moveDirection, out hit, moveDistance))
            {
                Debug.Log(hit.collider.name);
                if(hit.collider.tag == "WallCollider") {
                    Debug.Log("isWall");
                    moveDistance = hit.distance - 0.1f; 
                }
                
            }

            // Di chuyển với khoảng cách đã được điều chỉnh
            _rigid.Rigidbody.MovePosition(_rigid.Rigidbody.position + moveDirection.normalized * moveDistance);
        }


        // For test Mode
        public void SetSpeed(float speed) => this._speed = speed;

        public void SetPosition(Vector3 postion) => RPC_SetPosition(postion);

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetPosition(Vector3 pos, RpcInfo info = default)
        {
            this.transform.position = pos;
        }




    }
}






