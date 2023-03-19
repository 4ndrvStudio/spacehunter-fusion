using Colyseus.Schema;
using SH.Networking.Mining;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;
namespace SH.Networking.Mining
{
    public class Mineral : MonoBehaviour
    {
        [Header("Health")]

        [SerializeField] private HealthBar _healthHandler = default;

        [Header("FX")]

        [SerializeField] private ParticleSystem _explosionFX;

        [SerializeField] private ParticleSystem _hitFX;

        private RoomMiningNetworkMineral _mineralNetwork = default;


        private void Awake()
        {
            RoomMiningController.OnMineralHurt += OnMineralHurt;
            RoomMiningController.OnMineralRespawn += OnMineralRespawn;

        }

        private void OnDestroy()
        {
            RoomMiningController.OnMineralHurt -= OnMineralHurt;
            RoomMiningController.OnMineralRespawn -= OnMineralRespawn;
            _mineralNetwork.OnChange -= OnMineralChange;
        }

        public void Setup(RoomMiningNetworkMineral mineralNetwork)
        {
            gameObject.SetActive(true);
            _mineralNetwork = mineralNetwork;
            gameObject.name = "Mineral_" + mineralNetwork.Id;
            transform.position = new Vector3(mineralNetwork.PosX, mineralNetwork.PosY, mineralNetwork.PosZ);
            _healthHandler.Setup((int)mineralNetwork.TotalHealth);
            _mineralNetwork.OnChange += OnMineralChange;

        }

        private void OnMineralHurt(RoomMiningNetworkMineral mineral)
        {
            if (_mineralNetwork.Id == mineral.Id)
            {
                _healthHandler.UpdateHealth((int)mineral.CurrentHealth);
                if (mineral.CurrentHealth <= 0)
                {
                    //_explosionFX.Play();
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnMineralChange(List<DataChange> changes)
        {

        }

        private void OnMineralRespawn(RoomMiningNetworkMineral mineral)
        {
            if (mineral.Id == _mineralNetwork.Id)
                Setup(mineral);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.tag == "Weapon")
            {
                if (other.gameObject.GetComponent<RoomMiningNetworkEntityView>().IsMine)
                {
                    RoomMiningManager.Instance.SendAction(RoomMiningAction.MineralHurt, _mineralNetwork.Id);
                }
                other.collider.enabled = false;
                GetHitFX(other.contacts[0].point);
                _hitFX.Play();
            }
        }

        private void GetHitFX(Vector3 point)
        {
            _hitFX.gameObject.transform.position = point;
            _hitFX.Play();
        }
    }
}