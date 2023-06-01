using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using SH.PlayerData;
using SH.AzureFunction;
using SH.Models.Azure;
using System.Linq;
using SH.Multiplayer;

namespace SH
{
    public class InventoryManager : MonoBehaviour
    {

        public static InventoryManager Instance;

        [SerializeField] private List<ItemInstance> _items = new List<ItemInstance>();
        [HideInInspector] public List<ItemInstance> Items => _items;
        
        

        public List<Sprite> ItemFrame = new List<Sprite>();

        [HideInInspector]  
        public List<ItemConfig> ItemConfigs = new List<ItemConfig>();

        
        public List<WeaponConfig> WeaponConfigs = new List<WeaponConfig>();

        public static UnityAction OnInventoryDataChange;

        void Awake()
        {

            if (Instance == null)
                Instance = this;

            //Load Item Configs
            ItemConfigs = Resources.LoadAll<ItemConfig>("Configs/Items").ToList<ItemConfig>();

            //Load all Weapon configs;
            WeaponConfigs = Resources.LoadAll<WeaponConfig>("Configs/Weapons").ToList<WeaponConfig>();
            

        }

        public void GetInventoryData()
        {
            PlayFabManager.Instance.GetInventoryData(
                res =>
                {
                    this._items = res.Inventory;

                    _items.Add(CreateItemToTest("weapon_swordtest","weapon","Normal Sword"));
                    _items.Add(CreateItemToTest("weapon_mineral_axe","weapon","Mineral Axe"));
                    _items.Add(CreateItemToTest("spaceshiptest","spaceship","Spaceship E7x"));

                    OnInventoryDataChange?.Invoke();
                },
                err =>
                {
                    Debug.LogError("Get inventory Error : " + err.ErrorMessage);
                }
            );


        }

        public void AddInventoryItem(ClaimItemRequestModel[] requestModels)
        {
            ClaimItemsRequest claimItemsRequest = new ClaimItemsRequest(requestModels);
            PlayerDataManager.CallFunction<ClaimItemsRespone>(claimItemsRequest,
                    (res) =>
                           {
                               if (string.IsNullOrEmpty(res.Error))
                               {
                                   foreach (ClaimItemsResponeModel item in res.Items)
                                   {
                                       Debug.Log($"You claim {item.DisplayName} !");
                                   }
                                   GetInventoryData();
                               }
                               else
                               {
                                   Debug.LogError(res.Error);
                               }
                           });
        }

        private ItemInstance CreateItemToTest(string id, string itemClass, string name)
        {
            ItemInstance item = new ItemInstance();
            item.ItemId = id;
            item.ItemClass = itemClass;
            item.CustomData = new Dictionary<string, string> {
                        {"Level" , "1"}
                    };
            item.DisplayName = name;
            return item;
        }






    }
}

