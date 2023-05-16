using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

namespace SH
{
    public enum UIInventoryTab
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
        [SerializeField] private Button _miningBtn;
        [SerializeField] private Button _swordBtn;
        [SerializeField] private Button _useWeaponBtn;

        //pref
        [SerializeField] private Image _displayWeaponImage;
        [SerializeField] private Sprite _mineralAxeImage;
        [SerializeField] private Sprite _swordImage;
        [SerializeField] private UIItemSlot _mainSlot;

        //Tab
        [SerializeField] private UIInventoryTab _currentTab;
        [SerializeField] private Button _weaponTab;
        [SerializeField] private Button _spaceshipTab;
        [SerializeField] private Button _mineralTab;

        //item
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _inventoryContentHolder;
        [SerializeField] private List<GameObject> _inventoryItemList = new List<GameObject>();

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
            if (Instance == null)
                Instance = this;

            Show();

            _closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
            _useWeaponBtn.onClick.AddListener(() =>
            {
                Hide();
            });

            _weaponTab.onClick.AddListener(() => ChangeTab(UIInventoryTab.Weapon));
            _mineralTab.onClick.AddListener(() => ChangeTab(UIInventoryTab.Mineral));
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


        public void SetMainItem(UIItemSlot uiInventoryItem)
        {
            _mainSlot = uiInventoryItem;
            _displayWeaponImage.sprite = uiInventoryItem.ItemIcon;
        }

        public void ChangeTab(UIInventoryTab tab)
        {
            _currentTab = tab;
            InventoryManager.Instance.GetInventoryData();
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


            if (_currentTab != UIInventoryTab.All)
            {
                switch (_currentTab)
                {
                    case UIInventoryTab.Weapon:
                        itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemId == "weapon");
                        break;
                    case UIInventoryTab.Mineral:
                        itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemId == "mineral");
                        break;
                }
            }
            else
            {

                itemList = InventoryManager.Instance.Items;

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
