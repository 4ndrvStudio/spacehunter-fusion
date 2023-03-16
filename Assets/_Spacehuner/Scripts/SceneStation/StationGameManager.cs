
using System.Collections.Generic;
using UnityEngine;
using SH.PlayerData;
using SH.Networking.Mining;
using SH.Networking.PVE;

namespace SH.Networking.Station
{
    public class StationGameManager : MonoBehaviour
    {
        [SerializeField] private Transform _entityContainer = default;

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

        private PlayerMovement _playerMovement = default;

        public PlayerMovement PlayerMovement => _playerMovement;

        private void OnEnable()
        {
            RoomStationController.OnAddNetworkEntity += OnAddNetworkEntity;
            RoomStationController.OnRemoveNetworkEntity += OnRemoveNetworkEntity;
        }
        private void OnDisable()
        {
            RoomStationController.OnAddNetworkEntity -= OnAddNetworkEntity;
            RoomStationController.OnRemoveNetworkEntity -= OnRemoveNetworkEntity;
        }

        private void Start()
        {
            RoomStationManager.Instance.InitializeClient();
            Dictionary<string, object> options = new Dictionary<string, object>();
            options["DisplayName"] = PlayerDataManager.DisplayName;
            options["CharacterId"] = ((int)PlayerDataManager.Character.Data.CharacterInUse.CharacterType).ToString();
            RoomStationManager.Instance.JoinOrCreateRoom(options);
        }

        private void OnAddNetworkEntity(string sessionId, RoomStationNetworkEntity entity)
        {
            GameObject player = null;
            switch(entity.IdCharacter)
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

            if(player != null)
            {
                player.transform.SetParent(_entityContainer);
                Destroy(player.GetComponent<RoomMiningNetworkEntityView>());
                Destroy(player.GetComponent<RoomPVENetworkEntityView>());
                RoomStationNetworkEntityView entityView = player.GetComponent<RoomStationNetworkEntityView>();
                if (entity == null || entityView == null)
                {
                    Debug.LogError("Invalid entity");
                    return;
                }
                RoomStationManager.Instance.RegisterNetworkEntityView(entity, entityView, RoomStationManager.Instance.Room.SessionId == sessionId);
                if (RoomStationManager.Instance.Room.SessionId == sessionId)
                    _playerMovement = player.GetComponent<PlayerMovement>();
            }
        }

        private void OnRemoveNetworkEntity(string sessionId, RoomStationNetworkEntity entity)
        {
            var entityView = RoomStationManager.Instance.GetEntityView(entity.Id);
            if (entity != null)
                Destroy(entityView.gameObject);
        }

        public void OnJump()
        {
            _playerMovement?.OnJumpClick();
        }

        public void OnDashClick()
        {
            _playerMovement?.OnDashClick();
        }

    }
}
