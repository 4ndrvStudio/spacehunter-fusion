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
using SH.UI;

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

        public string CurrentHunterAddressInUse = string.Empty;

        public static UnityAction OnInventoryDataChange;

        public WeaponConfig CurrentWeaponInUse = null;

        [HideInInspector] public int MineralCollectedCount;
        [HideInInspector] public int ExpCollectedCount;

        [SerializeField] private string SuiPackageId = "0x6c770a38a07c937998bb0249e70101e79eda3848aea907cb90e56fad6fe62d8a";

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
            SuiWalletManager.TestSignature();
            PlayFabManager.Instance.GetInventoryData(
                async res =>
                {
                    this._items = res.Inventory;

                    // _items.Add(CreateItemToTest("weapon_swordtest", "weapon", "Normal Sword"));
                    _items.Add(CreateItemToTest("weapon_mineral_axe", "weapon", "Mineral Axe"));
                    _items.Add(CreateItemToTest("spaceshiptest", "spaceship", "Spaceship E7x"));

                    //Get NFT
                    var allNft = await SuiWalletManager.GetAllNFT();

                    allNft.Result.Data.ForEach(nft =>
                    {

                        string jsonNft = JsonConvert.SerializeObject(nft.Data.Content, Formatting.Indented);
                        JObject nftJsonObject = JObject.Parse(jsonNft);
                        
                        if (nftJsonObject.SelectToken("type").ToString().Contains($"{SuiPackageId}::stone::Stone"))
                        {
                            ItemInstance item = new ItemInstance();
                            item.ItemId = "mineral";
                            item.ItemClass = "mineral";
                            item.DisplayName = nftJsonObject.SelectToken("fields.name").ToString();
                            Dictionary<string, string> itemCustomData = new Dictionary<string, string>() {
                                {"Level", "3"},
                                {"Address", nftJsonObject.SelectToken("fields.id.Id").ToString()}
                            };
                            item.CustomData = itemCustomData;
                            this._items.Add(item);
                        }
                     
                        if (nftJsonObject.SelectToken("type").ToString().Contains($"{SuiPackageId}::sword::Sword"))
                        {
                            ItemInstance item = new ItemInstance();
                            item.ItemId = "sui_weapon";
                            item.ItemClass = "sui_weapon";
                            item.DisplayName = nftJsonObject.SelectToken("fields.name").ToString();
                            Dictionary<string, string> itemCustomData = new Dictionary<string, string>() {
                                {"Level", "1"},
                                {"Address", nftJsonObject.SelectToken("fields.id.Id").ToString()}
                            };
                            item.CustomData = itemCustomData;
                            this._items.Add(item);
                        }


                        if (nftJsonObject.SelectToken("type").ToString().Contains($"{SuiPackageId}::hunter::Hunter"))
                        {
                            CurrentHunterAddressInUse = nftJsonObject.SelectToken("fields.id.Id").ToString();
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

        public List<ItemInstance> GetFakeStoneItems(int amount)
        {
            List<ItemInstance> items = new List<ItemInstance>();
            
            for (int i =0 ; i< amount; i++) {
                     items.Add(CreateItemToTest("mineral", "mineral", "Mineral","3"));
            }

            return items;
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

        private ItemInstance CreateItemToTest(string id, string itemClass, string name, string level = "1")
        {
            ItemInstance item = new ItemInstance();
            item.ItemId = id;
            item.ItemClass = itemClass;
            item.CustomData = new Dictionary<string, string> {
                        {"Level" , level}
                    };
            item.DisplayName = name;
            return item;
        }






    }
}

