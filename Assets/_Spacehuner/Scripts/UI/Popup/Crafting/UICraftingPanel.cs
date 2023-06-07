using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
namespace SH.Multiplayer
{
    public class UICraftingPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeBtn;

        //item
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _inventoryContentHolder;
        [SerializeField] private List<GameObject> _craftingItemList = new List<GameObject>();
        
        [SerializeField] private List<GameObject> _targetItemList = new List<GameObject>();
        

        private void OnEnable()
        {
            InventoryManager.Instance.GetInventoryData();
            InventoryManager.OnInventoryDataChange += UpdateView;
        }
        private void OnDisable()
        {
            InventoryManager.OnInventoryDataChange += UpdateView;
        }
        public void UpdateView()
        {
            // Clear UI
            foreach (var item in _craftingItemList)
            {
                Destroy(item);
            }
            _craftingItemList.Clear();
            // Display items to UI
            Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

            List<ItemInstance> itemList = new List<ItemInstance>();

            itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemClass == "mineral");

            foreach (var item in itemList)
            {
                int level = int.Parse(item.CustomData["Level"]);
                string itemKey = item.ItemId + level;

                if (!itemDictionary.ContainsKey(itemKey))
                {
                    GameObject inventoryItemEl = Instantiate(_itemPrefab, _inventoryContentHolder.transform);
                    UICraftItemSlot inventoryElScript = inventoryItemEl.GetComponent<UICraftItemSlot>();

                    ItemConfig itemConfig = InventoryManager.Instance.ItemConfigs.Find(itemConfig => itemConfig.ItemId == item.ItemId);
                    if (item.ItemInstanceId != null)
                    {
                        itemConfig.ItemInstanceId = item.ItemInstanceId;
                    }
                    inventoryElScript.Setup(level, itemConfig, this);

                    _craftingItemList.Add(inventoryItemEl);
                    itemDictionary.Add(itemKey, inventoryItemEl);
                }
                else
                {
                    // Stack item
                    GameObject stackedItem = itemDictionary[itemKey];
                    stackedItem.GetComponent<UICraftItemSlot>().StackItem();
                }
            }

        }
    }

}
