using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using SH.Networking.Mining;

namespace SH.Networking.PVE
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar = default;

        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private Animator _animator = default;

        private RoomPVENetworkEnemy _previousState = default;

        private RoomPVENetworkEnemy _state = default;

        private float _stateSyncRateMs = 0.1f; // 40 frame/second

        private float _counterStateSyncRate = 0f;

        private Vector3[] _paths = new Vector3[3];

        TweenerCore<Vector3, Path, PathOptions> _tweenMove = default;

        [SerializeField] private int _timeMoveAgain = 3;

        private float _timeCountDown = 0;

        //[SerializeField] private Material _mat;

        //hidden in inspector
        private int _currentPos;
        private Vector3 _nextPos;

        

        private void Start()
        {
            RoomMiningController.OnEnemyHurt += OnEnemyHurt;
            RoomMiningController.OnEnemyRespawn += OnEnemyRespawn;
            if (_animator != null)
               _animator.SetBool("IsHurt", false);
        }

        public async void Setup(RoomPVENetworkEnemy enemy)
        {
            gameObject.name = enemy.Id;
            gameObject.SetActive(true);
            _animator.SetBool("IsDeath",false);
            _state = enemy;
            transform.position = new Vector3(_state.PosX, _state.PosY, _state.PosZ);
            _paths[0] = new Vector3(_state.PosX, _state.PosY, _state.PosZ);
            _paths[1] = new Vector3(_state.PosX + 10, _state.PosY, _state.PosZ + UnityEngine.Random.Range(-15, 15));
            _paths[2] = new Vector3(_state.PosX, _state.PosY, _state.PosZ);
            _healthBar.Setup((int)_state.Health);
            await Task.Delay(1000);
            Move();
        }

        private void OnDestroy()
        {
            RoomMiningController.OnEnemyHurt -= OnEnemyHurt;
            RoomMiningController.OnEnemyRespawn -= OnEnemyRespawn;
        }

        private void Move()
        {
            if(RoomMiningManager.Instance.CurrentNetworkEntity != null)
            {
                if (RoomMiningManager.Instance.CurrentNetworkEntity.IsHost)
                {
                    _tweenMove = transform.DOPath(_paths, 15, PathType.Linear, PathMode.Full3D).SetLoops(-1).SetEase(Ease.Linear).OnWaypointChange(waypoint => {
                        if(waypoint == _paths.Length) {
                            _nextPos =_paths[0];
                        } else _nextPos = _paths[waypoint+1];
                    });
                }
            }
        }

        private void EnemyRotDirection() {
             var rotationAngle = Quaternion.LookRotation(_nextPos - transform.position);
             transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * 5);
        }

        private void Update()
        {
            if (RoomMiningManager.Instance.CurrentNetworkEntity != null)
            {
                if (RoomMiningManager.Instance.CurrentNetworkEntity.IsHost)
                {
                    _counterStateSyncRate += Time.deltaTime;
                    if (_counterStateSyncRate > _stateSyncRateMs)
                    {
                        _counterStateSyncRate = 0f;
                        UpdateStateFromView();
                    }
                }
                else
                {
                    UpdateViewFromState();
                }
            }

            _timeCountDown += Time.deltaTime;
            if (_timeCountDown >= _timeMoveAgain)
            {
                if (_tweenMove != null)
                {
                    if (!_tweenMove.IsPlaying() && _state.Health > 0)
                    {
                        _tweenMove.Play();
                    }
                }
            }
            EnemyRotDirection();

        }

        private void UpdateStateFromView()
        {
            _previousState = _state.Clone(_state);

            _state.PosX = (float)Math.Round(transform.position.x, 3);
            _state.PosY = (float)Math.Round(transform.position.y, 3);
            _state.PosZ = (float)Math.Round(transform.position.z, 3);

            _state.RotX = (float)Math.Round(transform.rotation.x, 3);
            _state.RotY = (float)Math.Round(transform.rotation.y, 3);
            _state.RotZ = (float)Math.Round(transform.rotation.z, 3);
            _state.RotW = (float)Math.Round(transform.rotation.w, 3);

            Dictionary<string, object> changes = CompareChange(_previousState, _state);
            if (changes.Count > 1)
            {
                RoomMiningManager.Instance.SendAction(RoomPVEAction.EnemyUpdate, changes);
            }
        }

        private void UpdateViewFromState()
        {
            Vector3 pos = new Vector3(_state.PosX, _state.PosY, _state.PosZ);
            Quaternion rot = new Quaternion(_state.RotX, _state.RotY, _state.RotZ, _state.RotW);

            transform.position = Vector3.Lerp(transform.position, pos, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w), rot, 5 * Time.deltaTime);
        }

        private Dictionary<string, object> CompareChange(RoomPVENetworkEnemy oldState, RoomPVENetworkEnemy newState)
        {
            Dictionary<string, object> changes = new Dictionary<string, object>();
            changes["Id"] = oldState.Id;

            if (oldState.PosX != newState.PosX)
            {
                changes["PosX"] = newState.PosX;
            }
            if (oldState.PosY != newState.PosY)
            {
                changes["PosY"] = newState.PosY;
            }
            if (oldState.PosZ != newState.PosZ)
            {
                changes["PosZ"] = newState.PosZ;
            }

            if (oldState.RotX != newState.RotX)
            {
                changes["RotX"] = newState.RotX;
            }
            if (oldState.RotY != newState.RotY)
            {
                changes["RotY"] = newState.RotY;
            }
            if (oldState.RotZ != newState.RotZ)
            {
                changes["RotZ"] = newState.RotZ;
            }
            if (oldState.RotW != newState.RotW)
            {
                changes["RotW"] = newState.RotW;
            }

            return changes;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Weapon")
            {
                if (_healthBar.Health <= 0)
                    return;
                RoomMiningManager.Instance.SendAction(RoomPVEAction.EnemyHurt, _state.Id);
                if(_animator != null)
                    _animator.Play("GetHit",0,0);
            }   
        }

        private void OnEnemyHurt(RoomPVEController.EnemyHurtParam data)
        {
            if (_state.Id == data.Id)
            {
                _healthBar.UpdateHealth(data.Health);

                _timeCountDown = 0;
                _tweenMove.Pause();

                if (data.Health <= 0)
                {
                    _tweenMove.Kill();
                    _animator.SetBool("IsDeath", true);
                    EnemyDeath();
                }
            }

        }

        private async void EnemyDeath()
        {
            await Task.Delay(5000);
            gameObject.SetActive(false);
        }

        private void OnEnemyRespawn(RoomPVENetworkEnemy enemy)
        {
            if (_state.Id == enemy.Id)
                Setup(enemy);
        }
    }
}
