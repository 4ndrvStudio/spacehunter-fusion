using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using SH.Multiplayer;
using PlayFab;
using Newtonsoft.Json;
using Suinet.Rpc.Types;

namespace SH.UI
{
    public enum UIInventoryTabName
    {
        All,
        Weapon,
        Spaceship,
        Mineral,
        Reward
    }

    public class UIInventoryPopup : UIPopup
    {
        public static UIInventoryPopup Instance;

        private bool _init = false;

        //View
        [SerializeField] private Button _closeBtn;
        [SerializeField] private Button _useWeaponBtn;

        //pref
        [SerializeField] private Image _displayPreUseSlotImage;
        [SerializeField] private Sprite _mineralAxeImage;
        [SerializeField] private Sprite _swordImage;

        private UIItemSlot _preUseSlot;

        //Tab
        [SerializeField] private UIInventoryTabName _currentTab;
        [SerializeField] private List<UIInventoryTabButton> _tabButtonList = new List<UIInventoryTabButton>();


        //item
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _inventoryContentHolder;
        [SerializeField] private List<GameObject> _inventoryItemList = new List<GameObject>();

        //component
        [SerializeField] private UIInventoryItemInfo _uiInventoryItemInfo;

        private void OnEnable()
        {
            InventoryManager.Instance.GetInventoryData();

            InventoryManager.OnInventoryDataChange += UpdateView;

        }
        private void OnDisable()
        {
            InventoryManager.OnInventoryDataChange -= UpdateView;

        }

        private void Start()
        {
            _currentTab = UIInventoryTabName.Weapon;
            if (Instance == null)
                Instance = this;

            Show();

            _closeBtn.onClick.AddListener(() => Hide());
            //_useWeaponBtn.onClick.AddListener(() => Hide());

            _tabButtonList.ForEach(tabButton =>
            {
                tabButton.Button.onClick.AddListener(() =>
                {
                    ChangeTab(tabButton.TabName);
                    _tabButtonList.ForEach(tab => tab.SetDeactive());
                    tabButton.SetActive();
                });
            });
        }

        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);
            Setup();

        }

        public override void Hide()
        {
            base.Hide();
            UIControllerManager.Instance.DisplayController();
        }

        private void Setup()
        {
            if (!_init)
            {
                _init = true;
            }
        }


        public void SetPreUseItem(UIItemSlot uiInventoryItem)
        {
            _preUseSlot = uiInventoryItem;

            _uiInventoryItemInfo.DisplayItemInfo(uiInventoryItem);

            _inventoryItemList.ForEach(item =>
            {
                item.GetComponent<UIItemSlot>().IsPreUse(false);
            });

        }

        public void UseItem()
        {
            if (_preUseSlot.ItemConfig.TypeTab == UIInventoryTabName.Weapon)
            {
                Network_Player.Local.WeaponManager.RPC_SetWeaponInUse(_preUseSlot.ItemConfig.ItemId);
                Hide();
            }
            if (_preUseSlot.ItemConfig.TypeTab == UIInventoryTabName.Reward)
            {
                UseRewardBox(_preUseSlot.ItemConfig.ItemInstanceId);

            }

        }

        private async void UseRewardBox(string itemInstanceId)
        {
            UIManager.Instance.ShowWaiting();
            var mintResult = await SuiWalletManager.MintMineral();
            UIManager.Instance.HideWaiting();
            
            SuiNotificationModel modelPopup = new SuiNotificationModel();

            if (mintResult.IsSuccess == false)
            {
                modelPopup.IsSuccess = false;
                modelPopup.ErrorDescription = mintResult.ErrorMessage;

                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiNotification, modelPopup);
            }
            else
            {
                var nftObject = await SuiApi.Client.GetObjectAsync(mintResult.Result.Effects.SharedObjects[0].ObjectId, ObjectDataOptions.ShowAll());

                //serialize object
                string nFTModel = JsonConvert.SerializeObject(nftObject.Result.Data.Content, Formatting.Indented);

                //Cast to model
                MineralNFTModel model = JsonConvert.DeserializeObject<MineralNFTModel>(nFTModel);
                modelPopup.IsSuccess = true;
                modelPopup.Name = model.Fields.Name;
                modelPopup.Description = model.Fields.Description;
                modelPopup.ImageURL = model.Fields.ImageURL;
                modelPopup.ObjectId = mintResult.Result.Effects.SharedObjects[0].ObjectId;

                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiNotification, modelPopup);

                InventoryManager.Instance.ConsumeItem(itemInstanceId,1);
               
            }
             _uiInventoryItemInfo.ClearDisplay();


        }

        public void ChangeTab(UIInventoryTabName tab)
        {
            _currentTab = tab;
            _preUseSlot = null;
            Debug.Log("Tab change to " + _currentTab);
            _uiInventoryItemInfo.ClearDisplay();

            UpdateView();
        }

        private void UpdateView()
        {

            // Clear UI
            foreach (var item in _inventoryItemList)
            {
                Destroy(item);
            }
            _inventoryItemList.Clear();

            // Display items to UI
            Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

            List<ItemInstance> itemList = new List<ItemInstance>();

            switch (_currentTab)
            {
                case UIInventoryTabName.Weapon:
                    itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemClass == "weapon");
                    break;
                case UIInventoryTabName.Mineral:
                    itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemClass == "mineral");
                    break;
                case UIInventoryTabName.Spaceship:
                    itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemClass == "spaceship");
                    break;
                case UIInventoryTabName.Reward:
                    itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemClass == "mineral_ticket");
                    break;
                default:
                    itemList = InventoryManager.Instance.Items;
                    break;
            }


            foreach (var item in itemList)
            {
                int level = int.Parse(item.CustomData["Level"]);
                string itemKey = item.ItemId + level;

                if (!itemDictionary.ContainsKey(itemKey))
                {
                    GameObject inventoryItemEl = Instantiate(_itemPrefab, _inventoryContentHolder.transform);
                    UIItemSlot inventoryElScript = inventoryItemEl.GetComponent<UIItemSlot>();

                    ItemConfig itemConfig = InventoryManager.Instance.ItemConfigs.Find(itemConfig => itemConfig.ItemId == item.ItemId);
                    if (item.ItemInstanceId != null)
                    {
                        itemConfig.ItemInstanceId = item.ItemInstanceId;
                    }
                    inventoryElScript.Setup(level, itemConfig);

                    _inventoryItemList.Add(inventoryItemEl);
                    itemDictionary.Add(itemKey, inventoryItemEl);
                }
                else
                {
                    // Stack item
                    GameObject stackedItem = itemDictionary[itemKey];
                    stackedItem.GetComponent<UIItemSlot>().StackItem();
                }
            }

        }


    }

}
