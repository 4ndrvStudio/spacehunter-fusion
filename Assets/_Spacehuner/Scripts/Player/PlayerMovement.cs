using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Cinemachine;
using SH.Networking.Station;
using SH.Networking.Mining;
using SH.Networking.PVE;
using System.Threading.Tasks;

namespace SH
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private RoomStationNetworkEntityView _view = default;

        [SerializeField] private RoomMiningNetworkEntityView _viewMining = default;

        [SerializeField] private RoomPVENetworkEntityView _viewPVE = default;

        [SerializeField] private FloatingJoystick _fixedJoystick = default;

        [SerializeField] private PlayerAnimation _playerAnimation = default;

        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private float _maxSpeed = default;

        [SerializeField] private float _jumpHigh = default;

        [SerializeField] private float _distToGround = 0.9f;

        [SerializeField] private Transform _lookAt = default;

        [SerializeField] private bool _isGrounded = true;

        [SerializeField] private int _dashPower = default;

        [SerializeField] private bool _canDash = true;

        [SerializeField] private WeaponHook _weaponHook = default;

        [SerializeField] private ParticleSystem _vfxAttack = default;

        public Transform LookAt => _lookAt;

        public float Speed = default;

        private float _turnSmoothVelocity = default;

        private float _speedSmoothVelocity = default;

        private bool _isLock = false;

        //groundCheck
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundMask;


        private void Awake()
        {
            _fixedJoystick = FindObjectOfType<FloatingJoystick>();
        }

        private void Start()
        {
            RoomMiningController.OnMining += OnMining;
            RoomMiningController.OnAttackNormal += OnAttackNormal;
        }

        private void OnDestroy()
        {
            RoomMiningController.OnMining -= OnMining;
            RoomMiningController.OnAttackNormal -= OnAttackNormal;
        }

        private void OnMining(string id)
        {
            if (_viewMining.OwnerId == id)
            {
                _playerAnimation.SetMining();
            }
        }

        private void OnAttackNormal(string id)
        {
            if (_viewMining.OwnerId == id)
            {
                _playerAnimation.SetAttack();
            }
        }

        private void Update()
        {
            GroundCheck();
        }


        private void FixedUpdate()
        {
            if (_view != null && _view.IsMine)
            {
                float targetSpeed = _maxSpeed * _fixedJoystick.Direction.magnitude;
                Speed = Mathf.SmoothDamp(Speed, targetSpeed, ref _speedSmoothVelocity, 0.1f);
                Vector3 dir = new Vector3(transform.forward.x, _rb.velocity.y, transform.forward.z);
                //_rb.velocity = new Vector3(transform.forward.x * Speed,0, transform.forward.z * Speed);
                transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
                if (_fixedJoystick.Direction != Vector2.zero)
                {
                    float targetRotation = Mathf.Atan2(_fixedJoystick.Direction.x, _fixedJoystick.Direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, 0.2f);
                }

                _playerAnimation.SetMove(Speed);
            }
            if (_viewMining != null && _viewMining.IsMine)
            {
                float targetSpeed = _maxSpeed * _fixedJoystick.Direction.magnitude;
                Speed = Mathf.SmoothDamp(Speed, targetSpeed, ref _speedSmoothVelocity, 0.1f);
                //_rb.velocity = transform.forward * Speed;
                transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
                if (_fixedJoystick.Direction != Vector2.zero)
                {
                    float targetRotation = Mathf.Atan2(_fixedJoystick.Direction.x, _fixedJoystick.Direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, 0.2f);
                }

                _playerAnimation.SetMove(Speed);
            }

            if (_viewPVE != null && _viewPVE.IsMine)
            {
                float targetSpeed = _maxSpeed * _fixedJoystick.Direction.magnitude;
                Speed = Mathf.SmoothDamp(Speed, targetSpeed, ref _speedSmoothVelocity, 0.1f);
                //_rb.velocity = transform.forward * Speed;
                transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
                if (_fixedJoystick.Direction != Vector2.zero)
                {
                    float targetRotation = Mathf.Atan2(_fixedJoystick.Direction.x, _fixedJoystick.Direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, 0.2f);
                }

                _playerAnimation.SetMove(Speed);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_groundCheck.position, 0.15f);
        }
        private void GroundCheck()
        {
            if (Physics.CheckSphere(_groundCheck.position, 0.15f, _groundMask))
            {
                _playerAnimation.SetJump(false);
                _isGrounded = true;
            }
            else
            {
                _playerAnimation.SetJump(true);
                _isGrounded = false;
            }
        }

        public void LockControl()
        {
            _isLock = true;
        }

        public void OnJumpClick()
        {
            if (_view.IsMine && _isGrounded && !_isLock)
            {
                _rb.AddForce(Vector3.up * _jumpHigh);
            }
        }

        public void OnAttackClick()
        {
            if (_viewPVE.IsMine)
            {
                _playerAnimation.SetAttack();
                RoomPVEManager.Instance.SendAction(RoomPVEAction.AttackNormal, null);
            }
        }

        public void OnPVEJumpClick()
        {
            if (_viewPVE.IsMine && _isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpHigh);
            }
        }

        public void OnJumpMiningClick()
        {
            if (_viewMining.IsMine && _isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpHigh);
            }
        }

        private int _timeCooldown = 500; // miliseconds;
        private bool _canAttack = true;

        public async void OnMiningClick()
        {
            if (_viewMining.IsMine)
            {
                if (_weaponHook.IsWeaponMining)
                {
                    if (_canAttack)
                    {
                        _playerAnimation.SetMining();
                        RoomMiningManager.Instance.SendAction(RoomMiningAction.Mining, null);
                        _canAttack = false;
                        await Task.Delay(_timeCooldown);
                        _canAttack = true;
                    }
                }
                else
                {
                    if (_canAttack)
                    {
                        _playerAnimation.SetAttack();
                        RoomMiningManager.Instance.SendAction(RoomPVEAction.AttackNormal, null);
                        _canAttack = false;
                        await Task.Delay(_timeCooldown);
                        _canAttack = true;
                    }
                }
            }
        }

        public void OnDashClick()
        {
            if (_view.IsMine && _canDash)
            {
                Dash();
            }
        }

        private void Dash()
        {
            var targetPos = transform.position + transform.forward * 10f;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 10f * Time.deltaTime);
            transform.LookAt(targetPos);
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;
            transform.rotation = Quaternion.Euler(eulerAngles);
        }

        public void PlayEffectAttack()
        {
            _vfxAttack.Play();
        }
    }

}

