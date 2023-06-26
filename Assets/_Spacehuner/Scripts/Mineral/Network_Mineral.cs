using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Threading.Tasks;
using Suinet.Rpc.Types;
using Newtonsoft.Json;
using SH.Models.Azure;
using SH.UI;

namespace SH.Multiplayer
{
    public class MineralNFTModel
    {
        public string DataType { get; set; }
        public string Type { get; set; }
        public bool HasPublicTransfer { get; set; }
        public MineralNFTFieldModel Fields { get; set; }

    }
    public class MineralNFTFieldModel
    {

        public FieldId Id;
        public string Owner;

        [JsonProperty("collection_name")]
        public string Name;

        [JsonProperty("image_url")]
        public string ImageURL;

        [JsonProperty("collection_description")]
        public string Description;
        // public string Project_url;
    }
    public class FieldId
    {
        public string Id;
    }

    public class Network_Mineral : NetworkBehaviour
    {
        [HideInInspector] public Network_RoomMining Network_RoomMining = default;

        //network 
        [Networked] private NetworkBool _wasHit { get; set; }

        [Networked(OnChanged = nameof(OnHpChanged))]
        [SerializeField] private int _hp { get; set; }
        [SerializeField] private bool _wasSetupHp;

        //local
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _destroyFX;
        [SerializeField] private GameObject _awardObject;
        
        private PlayerRef _lastHitPlayer;
        private int _currentDame=0;

        private bool isClaimed;



        public override void Spawned()
        {
            _healthBar.Setup(_hp);
        }

        void Update()
        {
            if (!_wasSetupHp)
            {
                _healthBar.Setup(_hp);
                _wasSetupHp = true;
            }
        }


        public void HitMineral(PlayerRef player, int dame = 0)
        {
            if (Object == null) return;
            if (Object.HasStateAuthority == false) return;
            if (_wasHit) return;
            _lastHitPlayer = player;
            _currentDame = dame;
            // if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            // {
            //     playerNetworkObject.GetComponentInChildren<Network_WeaponCollider>().ToggleActiveCollider(CanHitName.Mineral, false);
            // }
            _wasHit = true;
        }
        public override void FixedUpdateNetwork()
        {

            if (Object.HasStateAuthority && _wasHit)
            {
                _wasHit = false;
                _hp-= _currentDame;

                if (_hp <= 0 && isClaimed == false)
                {
                    
                    MineralDestroy();
                     isClaimed = true;
                   
                   // RPC_MineralCollected(_lastHitPlayer, "ColledItem");
                }
            }
            if (Object.HasInputAuthority)
            {
                if (_hp <= 0)
                {
                    SpawnAward();
                   
                }
            }

        }

        async void MineralDestroy()
        {
            await Task.Delay(2000);
            Network_RoomMining.MineralCollected(Object);
        }

        private void SpawnAward()
        {
            Debug.Log("Award Spawned");
            Instantiate(_awardObject, this.transform.position, Quaternion.identity);
        }


        static void OnHpChanged(Changed<Network_Mineral> changed)
        {
            changed.Behaviour.OnHpChanged();
        }
        private void OnHpChanged()
        { 

            if (_hp <= 0)
            {
                _healthBar.gameObject.SetActive(false);
                _body.SetActive(false);
                _destroyFX.SetActive(true);
            }
            _healthBar.UpdateHealth(_hp);

        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_MineralCollected([RpcTarget] PlayerRef player, string message, RpcInfo info = default)
        {
            ClaimItemRequestModel[] ls2 = new ClaimItemRequestModel[1] {
                new ClaimItemRequestModel(){
                    ItemId = "mineral_ticket",
                    Level = 1
                }
            };
            InventoryManager.Instance.AddInventoryItem(ls2);
            
          

        }









    }

}
