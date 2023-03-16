using SH.Networking.PVE;
using SH.Networking.Station;
using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Mining
{
    public class MiningGameManager : MonoBehaviour
    {
        [Header("Players")]

        [SerializeField] private GameObject _prefabDisryMale = default;
        [SerializeField] private GameObject _prefabDisryFemale = default;

        [SerializeField] private GameObject _prefabMutasMale = default;
        [SerializeField] private GameObject _prefabMutasFemale = default;

        [SerializeField] private GameObject _prefabHumesMale = default;
        [SerializeField] private GameObject _prefabHumesFemale = default;

        [SerializeField] private GameObject _prefabVasinMale = default;
        [SerializeField] private GameObject _prefabVasinFemale = default;

        [SerializeField] private GameObject _prefabMabitMale = default;
        [SerializeField] private GameObject _prefabMabitFemale = default;

        [SerializeField] private Transform _entityContainer = default;

        [Header("Minerals")]

        [SerializeField] private GameObject _prefabMineralLv1 = default;

        [SerializeField] private GameObject _prefabMineralLv2 = default;

        [SerializeField] private GameObject _prefabMineralLv3 = default;

        [SerializeField] private Transform _mineralsContainer = default;

        [Header("Enemies")]

        [SerializeField] private GameObject _prefabEnemy1 = default;

        [SerializeField] private GameObject _prefabEnemy2 = default;

        [SerializeField] private GameObject _prefabEnemy3 = default;

        [SerializeField] private Transform _enemiesContainer = default;

        private PlayerMovement _playerMovement = default;

        public PlayerMovement PlayerMovement => _playerMovement;

        private void OnEnable()
        {
            RoomMiningController.OnAddNetworkEntity += OnAddNetworkEntity;
            RoomMiningController.OnRemoveNetworkEntity += OnRemoveNetworkEntity;
            RoomMiningController.OnAddMineralEntity += OnAddMineralEntity;
            RoomMiningController.OnRemoveMineralEntity += OnRemoveMineralEntity;
            RoomMiningController.OnAddNetworkEnemy += OnAddNetworkEnemy;
            RoomMiningController.OnRemoveNetworkEnemy += OnRemoveNetworkEnemy;
        }

        private void OnDisable()
        {
            RoomMiningController.OnAddNetworkEntity -= OnAddNetworkEntity;
            RoomMiningController.OnRemoveNetworkEntity -= OnRemoveNetworkEntity;
            RoomMiningController.OnAddMineralEntity -= OnAddMineralEntity;
            RoomMiningController.OnRemoveMineralEntity -= OnRemoveMineralEntity;
            RoomMiningController.OnAddNetworkEnemy -= OnAddNetworkEnemy;
            RoomMiningController.OnRemoveNetworkEnemy -= OnRemoveNetworkEnemy;
        }

        private void Start()
        {
            RoomMiningManager.Instance.InitializeClient();
            Dictionary<string, object> options = new Dictionary<string, object>();
            options["PlayFabId"] = PlayerDataManager.PlayFabId;
            options["DisplayName"] = PlayerDataManager.DisplayName;
            options["CharacterId"] = ((int)PlayerDataManager.Character.Data.CharacterInUse.CharacterType).ToString();
            RoomMiningManager.Instance.JoinOrCreateRoom(options);
        }

        private void OnAddNetworkEntity(string sessionId, RoomMiningNetworkEntity entity)
        {
            GameObject player = null;
            switch (entity.IdCharacter)
            {
                 case "1":
                    player = Instantiate(_prefabDisryMale);
                    break;
                case "2":
                    player = Instantiate(_prefabHumesMale);
                    break;
                case "3":
                    player = Instantiate(_prefabMabitMale);
                    break;
                case "4":
                    player = Instantiate(_prefabMutasMale);
                    break;
                case "5":
                    player = Instantiate(_prefabVasinMale);
                    break;
                case "6":
                    player = Instantiate(_prefabDisryFemale);
                    break;
                case "7":
                    player = Instantiate(_prefabHumesFemale);
                    break;
                case "8":
                    player = Instantiate(_prefabMabitFemale);
                    break;
                case "9":
                    player = Instantiate(_prefabMutasFemale);
                    break;
                case "10":
                    player = Instantiate(_prefabVasinFemale);
                    break;
                default:
                    Debug.LogError("Invalid character type!");
                    break;
            }

            if (player != null)
            {
                player.transform.SetParent(_entityContainer);
                Destroy(player.GetComponent<RoomStationNetworkEntityView>());
                Destroy(player.GetComponent<RoomPVENetworkEntityView>());
                RoomMiningNetworkEntityView entityView = player.GetComponent<RoomMiningNetworkEntityView>();
                if (entity == null || entityView == null)
                {
                    Debug.LogError("Invalid entity");
                    return;
                }
                RoomMiningManager.Instance.RegisterNetworkEntityView(entity, entityView, RoomMiningManager.Instance.Room.SessionId == sessionId);
                if (RoomMiningManager.Instance.Room.SessionId == sessionId)
                    _playerMovement = player.GetComponent<PlayerMovement>();
            }


        }

        private void OnRemoveNetworkEntity(string sessionId, RoomMiningNetworkEntity entity)
        {
            var entityView = RoomMiningManager.Instance.GetEntityView(entity.Id);
            if (entity != null)
                Destroy(entityView.gameObject);
        }

        private void OnRemoveMineralEntity(string id, RoomMiningNetworkMineral mineral)
        {
           
        }

        private void OnAddMineralEntity(string id, RoomMiningNetworkMineral mineral)
        {
            CreateMineral(id, mineral);
        }

        private void CreateMineral(string id, RoomMiningNetworkMineral mineral)
        {
            GameObject prefab = default;
            if (mineral.Type == "Lv1")
                prefab = _prefabMineralLv1;
            if (mineral.Type == "Lv2")
                prefab = _prefabMineralLv2;
            if (mineral.Type == "Lv3")
                prefab = _prefabMineralLv3;

            GameObject obj = Instantiate(prefab, _mineralsContainer.transform);
            obj.GetComponent<Mineral>().Setup(mineral);
        }

        private void OnAddNetworkEnemy(string id, RoomPVENetworkEnemy enemyNetwork)
        {
            GameObject enemy = null;
            switch (enemyNetwork.Type)
            {
                case "1":
                    enemy = Instantiate(_prefabEnemy1);
                    break;
                case "2":
                    enemy = Instantiate(_prefabEnemy2);
                    break;
                case "3":
                    enemy = Instantiate(_prefabEnemy3);
                    break;
            }
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>()?.Setup(enemyNetwork);
                enemy.transform.SetParent(_enemiesContainer);
            }
        }


        private void OnRemoveNetworkEnemy(string arg1, RoomPVENetworkEnemy arg2)
        {
           
        }




        public void OnJump()
        {
            _playerMovement?.OnJumpMiningClick();
        }

        public void Mint()
        {
            _playerMovement.GetComponent<PlayerMovement>().OnMiningClick();
        }
    }
}
