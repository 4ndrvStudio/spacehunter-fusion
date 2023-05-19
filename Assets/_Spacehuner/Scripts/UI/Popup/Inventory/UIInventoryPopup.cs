using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using SH.Multiplayer;

namespace SH
{
    public enum UIInventoryTabName
    {
        All,
        Weapon,
        Spaceship,
        Mineral,
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
        [SerializeField] private List<UIInventoryTabButton> _tabButtonList  = new List<UIInventoryTabButton>();
      

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
            _useWeaponBtn.onClick.AddListener(() => Hide());
            
            _tabButtonList.ForEach(tabButton => {
                tabButton.Button.onClick.AddListener(() => {
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

            _inventoryItemList.ForEach(item=> {
                item.GetComponent<UIItemSlot>().IsPreUse(false);
            });

        }

        public void UseItem() {
            Network_Player.Local.WeaponManager.UseWeapon(_preUseSlot.ItemConfig.ItemId);
        }

        public void ChangeTab(UIInventoryTabName tab)
        {
            _currentTab = tab;
            _preUseSlot = null;

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
