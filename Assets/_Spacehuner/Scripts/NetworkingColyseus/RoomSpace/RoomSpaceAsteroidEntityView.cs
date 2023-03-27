using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace SH.Networking.Space
{
    public class RoomSpaceAsteroidEntityView : MonoBehaviour
    {
        [SerializeField] private string _id = default;

        private RoomSpaceAsteroidEntity _previousState = default;

        private RoomSpaceAsteroidEntity _currentState = default;

        private float _stateSyncRateMs = 0.1f;

        private float _counterStateSyncRate = 0f;

        public void Setup(RoomSpaceAsteroidEntity entity)
        {
            _currentState = entity;
            _id = entity.Id;
            _currentState.OnChange += OnChange;
            transform.position = new Vector3(entity.PosX, entity.PosY, entity.PosZ);
        }

        private void OnDestroy()
        {
            _currentState.OnChange -= OnChange;
        }

        private void OnChange(List<DataChange> changes)
        {
            
        }

        private void Update()
        {
            //if (RoomSpaceManager.Instance.CurrentNetworkEntity.IsHost)
            //{
            //    _counterStateSyncRate += Time.deltaTime;
            //    if (_counterStateSyncRate > _stateSyncRateMs)
            //    {
            //        _counterStateSyncRate = 0f;
            //        SendStateForOtherPlayer();
            //    }
            //}
            //else
            //{
            //    UpdateState();
            //}
        }

        private void SendStateForOtherPlayer()
        {
            _currentState.PosX = transform.position.x;
            _currentState.PosY = transform.position.y;
            _currentState.PosZ = transform.position.z;
            Quaternion quater = transform.rotation;
            _currentState.RotX = quater.x;
            _currentState.RotY = quater.y;
            _currentState.RotZ = quater.z;
            _currentState.RotW = quater.w;
            Debug.LogError("send to server");
        }

        private void UpdateState()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_currentState.PosX, _currentState.PosY, _currentState.PosZ), 5 * Time.deltaTime);
        }
    }
}
