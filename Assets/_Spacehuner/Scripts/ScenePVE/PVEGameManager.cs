using SH.Networking.Mining;
using SH.Networking.Station;
using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.PVE
{
    public class PVEGameManager : MonoBehaviour
    {
        [SerializeField] private Transform _entityContainer = default;

        [SerializeField] private Transform _enemiesContainer = default;

        [SerializeField] private GameObject _prefabDisry = default;

        [SerializeField] private GameObject _prefabEnemy = default;

        private PlayerMovement _playerMovement = default;

        public PlayerMovement PlayerMovement => _playerMovement;

        private void OnEnable()
        {
            RoomPVEController.OnAddNetworkEntity += OnAddNetworkEntity;
            RoomPVEController.OnRemoveNetworkEntity += OnRemoveNetworkEntity;
            RoomPVEController.OnAddNetworkEnemy += OnAddNetworkEnemy;
            RoomPVEController.OnRemoveNetworkEnemy += OnRemoveNetworkEnemy;
        }

        private void OnDisable()
        {
            RoomPVEController.OnAddNetworkEntity -= OnAddNetworkEntity;
            RoomPVEController.OnRemoveNetworkEntity -= OnRemoveNetworkEntity;
            RoomPVEController.OnAddNetworkEnemy -= OnAddNetworkEnemy;
            RoomPVEController.OnRemoveNetworkEnemy -= OnRemoveNetworkEnemy;
        }

        private void Start()
        {
            RoomPVEManager.Instance.InitializeClient();
            Dictionary<string, object> options = new Dictionary<string, object>();
            options["PlayFabId"] = PlayerDataManager.PlayFabId;
            options["DisplayName"] = PlayerDataManager.DisplayName;
            options["CharacterId"] = 1.ToString();
            RoomPVEManager.Instance.JoinOrCreateRoom(options);
        }

        private void OnAddNetworkEntity(string sessionId, RoomPVENetworkEntity entity)
        {
            GameObject player = Instantiate(_prefabDisry);
            player.transform.SetParent(_entityContainer);
            Destroy(player.GetComponent<RoomStationNetworkEntityView>());
            Destroy(player.GetComponent<RoomMiningNetworkEntityView>());
            RoomPVENetworkEntityView entityView = player.GetComponent<RoomPVENetworkEntityView>();
            if (entity == null || entityView == null)
            {
                Debug.LogError("Invalid entity");
                return;
            }
            RoomPVEManager.Instance.RegisterNetworkEntityView(entity, entityView, RoomPVEManager.Instance.Room.SessionId == sessionId);
            if (RoomPVEManager.Instance.Room.SessionId == sessionId)
                _playerMovement = player.GetComponent<PlayerMovement>();

        }

        private void OnRemoveNetworkEntity(string sessionId, RoomPVENetworkEntity entity)
        {
            var entityView = RoomPVEManager.Instance.GetEntityView(entity.Id);
            if (entity != null)
                Destroy(entityView.gameObject);
        }

        private void OnAddNetworkEnemy(string id, RoomPVENetworkEnemy networkEnemy)
        {
            GameObject enemy = Instantiate(_prefabEnemy);
            enemy.GetComponent<Enemy>()?.Setup(networkEnemy);
            enemy.transform.SetParent(_enemiesContainer);
        }

        private void OnRemoveNetworkEnemy(string arg1, RoomPVENetworkEnemy arg2)
        {
            Debug.Log("destroy game object enemy");
        }

        public void OnJump()
        {
            _playerMovement?.OnPVEJumpClick();
        }

        public void OnAttack()
        {
            _playerMovement?.OnAttackClick();
        }
    }
}