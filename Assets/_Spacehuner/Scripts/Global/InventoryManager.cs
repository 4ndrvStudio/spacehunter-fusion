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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SH.UI
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

        public WeaponConfig CurrentWeaponInUse = null;

        [HideInInspector] public int MineralCollectedCount;
        [HideInInspector] public int ExpCollectedCount;

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
                async res =>
                {
                    this._items = res.Inventory;

                    _items.Add(CreateItemToTest("weapon_swordtest", "weapon", "Normal Sword"));
                    _items.Add(CreateItemToTest("weapon_mineral_axe","weapon","Mineral Axe"));
                    _items.Add(CreateItemToTest("spaceshiptest", "spaceship", "Spaceship E7x"));
                    
                    //Get NFT
                    var allNft = await SuiWalletManager.GetAllNFT();
                  
                    allNft.Result.Data.ForEach(nft => {
                      
                        string jsonNft = JsonConvert.SerializeObject(nft.Data.Content,Formatting.Indented);
                       
                        JObject nftJsonObject = JObject.Parse(jsonNft);

                        if(nftJsonObject.SelectToken("type").ToString().Contains("stone")) {
                            ItemInstance item= new ItemInstance();
                            item.ItemId = "mineral";
                            item.ItemClass = "mineral";
                            item.DisplayName = nftJsonObject.SelectToken("fields.name").ToString();
                            Dictionary<string, string> itemCustomData = new Dictionary<string, string>() {
                                {"Level", "3"}
                            };
                            item.CustomData = itemCustomData;
                            this._items.Add(item);
                        } 
                    });

                    OnInventoryDataChange?.Invoke();
                },
                err =>
                {
                    Debug.LogError("Get inventory Error : " + err.ErrorMessage);
                }
            );


        }

        public void ConsumeItem(string itemInstanceId, int count)
        {

            ConsumeItemRequest request = new ConsumeItemRequest()
            {
                ConsumeCount = 1,
                ItemInstanceId = itemInstanceId
            };

            PlayFabClientAPI.ConsumeItem(request,
                res =>
               {
                   GetInventoryData();
               },
                err =>
                {
                    UIManager.Instance.HideWaiting();
                    Debug.Log(err.ErrorMessage);
                    UIManager.Instance.ShowAlert("Something is error! Please re-use this box", AlertType.Error);
                    GetInventoryData();
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
                                       UIManager.Instance.ShowAlert("Mineral Reward has been claim!", AlertType.Normal);
                                   }
                                   GetInventoryData();
                               }
                               else
                               {
                                   Debug.LogError(res.Error);
                               }
                           },
                           false
            );

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

