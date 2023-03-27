using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SH.Networking.PVE
{
    public class RoomPVENetworkEntityView : MonoBehaviour
    {
        private float _stateSyncRateMs = 0.1f; // 40 frame/second
        private float _counterStateSyncRate = 0f;

        [SerializeField] private TextMeshPro _tmpName = default;

        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private PlayerMovement _playerMovement = default;

        [SerializeField] private PlayerAnimation _playerAnimation = default;

        [field: SerializeField] public string OwnerId { get; private set; }

        [field: SerializeField] public bool IsMine { get; private set; }


        private RoomPVENetworkEntity _previousState = default;

        private RoomPVENetworkEntity _state = default;



        public void InitView(RoomPVENetworkEntity entity, bool isMine)
        {
            _state = entity;
            IsMine = isMine;
            transform.position = new Vector3(_state.PosX, _state.PosY, _state.PosZ);
            OwnerId = _state.Id;
            _state.Attributes.OnChange += OnAttributeChange;
            _state.OnChange += OnStateChange;
            _tmpName.SetText(_state.Name);
            _tmpName.color = IsMine ? Color.yellow : Color.white;
            gameObject.name = _state.Name;
        }

        private void OnStateChange(List<DataChange> changes)
        {
            if (!IsMine)
            {
                //UpdateViewFromState();
            }
        }

        // Update state for remote player
        private void UpdateViewFromState()
        {
            Vector3 pos = new Vector3(_state.PosX, _state.PosY, _state.PosZ);
            Quaternion rot = new Quaternion(_state.RotX, _state.RotY, _state.RotZ, _state.RotW);
            Vector3 vel = new Vector3(_state.VelX, _state.VelY, _state.VelZ);

            transform.position = Vector3.Lerp(transform.position, pos, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w), rot, 5 * Time.deltaTime);
            _rb.velocity = vel;
            _playerAnimation.SetMove(_state.Speed);
        }



        private void OnAttributeChange(string key, string value)
        {

        }




        private void Update()
        {
            _tmpName.gameObject.transform.LookAt(Camera.main.transform);

            if (IsMine)
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

        // send state to other players
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

            _state.VelX = _rb.velocity.x;
            _state.VelY = _rb.velocity.y;
            _state.VelZ = _rb.velocity.z;

            _state.Speed = _playerMovement.Speed;

            Dictionary<string, object> changes = CompareChange(_previousState, _state);
            if (changes.Count > 1)
            {
                RoomPVEManager.Instance.SendAction(RoomPVEAction.EntityUpdate, changes);
            }
        }

        private Dictionary<string, object> CompareChange(RoomPVENetworkEntity oldState, RoomPVENetworkEntity newState)
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

            if (oldState.VelX != newState.VelX)
            {
                changes["VelX"] = newState.VelX;
            }
            if (oldState.VelY != newState.VelY)
            {
                changes["VelY"] = newState.VelY;
            }
            if (oldState.VelZ != newState.VelZ)
            {
                changes["VelZ"] = newState.VelZ;
            }

            if (oldState.Speed != newState.Speed)
            {
                changes["Speed"] = newState.Speed;
            }

            if (oldState.Name != newState.Name)
            {
                changes["Name"] = newState.Name;
            }

            return changes;
        }
    }
}