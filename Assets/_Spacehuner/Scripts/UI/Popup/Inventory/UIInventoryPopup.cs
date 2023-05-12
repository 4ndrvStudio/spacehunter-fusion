using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH
{
    public class UIInventoryPopup : UIPopup
    {
        public static UIInventoryPopup Instance;

        private bool _init = false;

        // this just for test
        enum WeaponInUse
        {
            MineralAxe,
            Sword
        }

        [SerializeField] private Button _closeBtn;
        [SerializeField] private Button _miningBtn;
        [SerializeField] private Button _swordBtn;
        [SerializeField] private Button _useWeaponBtn;
        [SerializeField] private WeaponInUse _weaponInUse;

        [SerializeField] private Image _displayWeaponImage;
        [SerializeField] private Sprite _mineralAxeImage;
        [SerializeField] private Sprite _swordImage;


        [SerializeField] private UIItemSlot _mainSlot;

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

            //this is for test
            _miningBtn.onClick.AddListener(() =>
            {
                _weaponInUse = WeaponInUse.MineralAxe;
                _displayWeaponImage.sprite = _mineralAxeImage;

            });
            _swordBtn.onClick.AddListener(() =>
            {
                _weaponInUse = WeaponInUse.Sword;
                _displayWeaponImage.sprite = _swordImage;
            });

            _closeBtn.onClick.AddListener(() =>
            {
                Hide();
            });
            _useWeaponBtn.onClick.AddListener(() =>
            {
                Hide();
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

        public void SetMainItem(UIItemSlot uiInventoryItem)
        {
            _mainSlot = uiInventoryItem;
            _displayWeaponImage.sprite = uiInventoryItem.ItemIcon;
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

            foreach (var item in InventoryManager.Instance.Items)
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
