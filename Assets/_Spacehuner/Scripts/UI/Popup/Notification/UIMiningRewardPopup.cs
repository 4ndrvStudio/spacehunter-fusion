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

        public override void ShowWithCallback(object customProperties, UnityAction callback = null)
        {
            base.ShowWithCallback(customProperties, callback);
             _callback = callback;

            List<ItemInstance> items = customProperties as List<ItemInstance>;
            
            Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

            foreach (var item in items)
            {
                int level = int.Parse(item.CustomData["Level"]);
                
                string itemKey = item.ItemId + level;

                if (!itemDictionary.ContainsKey(itemKey))
                {
                    GameObject inventoryItemEl = Instantiate(_itemSlotPrefab, _itemHolder.transform);
                    
                    UIItemSlotReward inventoryElScript = inventoryItemEl.GetComponent<UIItemSlotReward>();

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
                    stackedItem.GetComponent<UIItemSlotReward>().StackItem();
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
            _inventoryItemList.ForEach(item => {
                Destroy(item);
            });     
            _inventoryItemList.Clear();

            if (_callback != null)
                 _callback?.Invoke();

            Hide();
        }

    
    }
        public class SuiMiningRewardModel
        {   
            public List<ItemConfig> itemConfigs;
        }

}
