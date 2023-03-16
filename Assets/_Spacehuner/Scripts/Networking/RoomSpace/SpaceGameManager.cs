using SH.Networking.Mining;
using SH.Networking.PVE;
using SH.Networking.Station;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Networking.Space
{
    public class SpaceGameManager : MonoBehaviour
    {
        [SerializeField] private Transform _entityContainer = default;

        [SerializeField] private GameObject _spaceShipPrefab = default;

        [SerializeField] private Transform _asteroidContainer = default;

        [SerializeField] private GameObject _asteroidPrefab = default;

        [SerializeField] private List<RoomSpaceAsteroidEntityView> _lstAsteroid = new List<RoomSpaceAsteroidEntityView>();

        private  SpaceshipController _currentSpaceship = default;

        private void OnEnable()
        {
            RoomSpaceController.OnAddNetworkEntity += OnAddNetworkEntity;
            RoomSpaceController.OnRemoveNetworkEntity += OnRemoveNetworkEntity;
            RoomSpaceController.OnAddAsteroidEntity += OnAddAsteroidEntity;
            RoomSpaceController.OnRemoveAsteroidEntity += OnRemoveAsteroidEntity;

            RoomSpaceController.OnShooting += OnShooting;
        }

        private void OnShooting(string id)
        {
            Debug.Log("aa");
            var entity = RoomSpaceManager.Instance.GetEntityView(id);
            if(entity != null)
            {
                Debug.Log("Receive shooting");
                entity.GetComponent<Shooting>().ShootPointerDown();
            }
        }

        private void OnDisable()
        {
            RoomSpaceController.OnAddNetworkEntity -= OnAddNetworkEntity;
            RoomSpaceController.OnRemoveNetworkEntity -= OnRemoveNetworkEntity;
            RoomSpaceController.OnAddAsteroidEntity -= OnAddAsteroidEntity;
            RoomSpaceController.OnRemoveAsteroidEntity -= OnRemoveAsteroidEntity;

            RoomSpaceController.OnShooting -= OnShooting;
        }

        private void Start()
        {
            RoomSpaceManager.Instance.InitializeClient();
            Dictionary<string, object> options = new Dictionary<string, object>();
            RoomSpaceManager.Instance.JoinOrCreateRoom(options);
            _lstAsteroid.Clear();
        }

        private void OnAddNetworkEntity(string sessionId, RoomSpaceNetworkEntity entity)
        {
            GameObject player = Instantiate(_spaceShipPrefab);
            if (player != null)
            {
                player.transform.SetParent(_entityContainer);
                Destroy(player.GetComponent<RoomMiningNetworkEntityView>());
                Destroy(player.GetComponent<RoomPVENetworkEntityView>());
                Destroy(player.GetComponent<RoomStationNetworkEntityView>());
                RoomSpaceNetworkEntityView entityView = player.GetComponent<RoomSpaceNetworkEntityView>();
                if (entity == null || entityView == null)
                {
                    Debug.LogError("Invalid entity");
                    return;
                }
                player.GetComponent<SpaceshipController>().Setup(true);
                RoomSpaceManager.Instance.RegisterNetworkEntityView(entity, entityView, RoomSpaceManager.Instance.Room.SessionId == sessionId);
                if (RoomSpaceManager.Instance.Room.SessionId == sessionId)
                    _currentSpaceship = player.GetComponent<SpaceshipController>();
            }
        }

        private void OnRemoveNetworkEntity(string sessionId, RoomSpaceNetworkEntity entity)
        {
            var entityView = RoomSpaceManager.Instance.GetEntityView(entity.Id);
            if (entity != null)
                Destroy(entityView.gameObject);
        }

        private void OnAddAsteroidEntity(string arg1, RoomSpaceAsteroidEntity arg2)
        {
            GameObject asteroid = Instantiate(_asteroidPrefab);
            if(asteroid !=  null)
            {
                asteroid.transform.SetParent(_asteroidContainer);
                var view = asteroid.GetComponent<RoomSpaceAsteroidEntityView>();
                if(view != null)
                {
                    view.Setup(arg2);
                    _lstAsteroid.Add(view);
                }
            }
        }

        private void OnRemoveAsteroidEntity(string arg1, RoomSpaceAsteroidEntity arg2)
        {
            
        }

        private void Update()
        {
            if (RoomSpaceManager.Instance != null)
                UIManager.Instance.SetPing(RoomSpaceManager.Instance.Ping);
        }

        public void PointerDownShootBtn()
        { 
            if(_currentSpaceship.IsMine)
            {
                _currentSpaceship.GetComponent<Shooting>()?.ShootPointerDown();
                RoomSpaceManager.Instance.SendAction(RoomSpaceAction.Shoot, _currentSpaceship.Id);
                Debug.Log("Shooting");
            }
        }

    }
}
