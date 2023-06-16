using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using PlayFab.ClientModels;

namespace SH.UI
{
   public class UIMiningRewardPopup : UIPopup
    {
        [SerializeField] private Button _confirmBtn;

        [SerializeField] private GameObject _itemHolder;

        [SerializeField] private GameObject _itemSlotPrefab;

        [SerializeField] private List<GameObject> _inventoryItemList = new List<GameObject>();

    
        private UnityAction _callback;
        private string _currentTx;
        private bool IsMinted;

        public override void ShowWithCallback(object customProperties, UnityAction callback = null)
        {
            base.ShowWithCallback(customProperties, callback);
             _callback = callback;

            List<ItemInstance> items = customProperties as List<ItemInstance>;
           
            foreach (var item in items)
            {
                int level = int.Parse(item.CustomData["Level"]);
                string itemKey = item.ItemId + level;

                Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

                if (!itemDictionary.ContainsKey(itemKey))
                {
                    GameObject inventoryItemEl = Instantiate(_itemSlotPrefab, _itemHolder.transform);
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

        // Start is called before the first frame update
        void Start()
        {
            _confirmBtn.onClick.AddListener(() => ConfirmClick());
        }
        void ConfirmClick()
        {
      
            if (_callback != null)
                if (IsMinted) _callback?.Invoke();

            Hide();
        }

    
    }
        public class SuiMiningRewardModel
        {   
            public List<ItemConfig> itemConfigs;
        }

}
