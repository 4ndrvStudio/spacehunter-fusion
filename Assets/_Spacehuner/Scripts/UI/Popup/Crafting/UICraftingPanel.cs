using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using SH.Multiplayer;
using DG.Tweening;

namespace SH.UI
{
    public class UICraftingPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeBtn;
        [SerializeField] private UICraftingPopup _craftingPopup;
        //item
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _inventoryContentHolder;
        [SerializeField] private List<GameObject> _craftingItemList = new List<GameObject>();
        [SerializeField] private GameObject _loaderIcon;
        //Crafting Setup 
        [SerializeField] private List<UICraftingPiece> _craftingItemPieceList = new List<UICraftingPiece>();
        [SerializeField] private UICraftingButton _craftingButton;
        [SerializeField] private Image _processBarImage;
        [SerializeField] private bool _isCrafting;

        private void Start()
        {
            _closeBtn.onClick.AddListener(() =>
            {
                if (_isCrafting == true) return;
                ResetPanel();
                _craftingPopup.CloseCrafting();
            });
        }


        private void OnEnable()
        {
            _loaderIcon.SetActive(true);
            InventoryManager.Instance.GetInventoryData();
            InventoryManager.OnInventoryDataChange += UpdateView;
        }
        private void OnDisable()
        {
            InventoryManager.OnInventoryDataChange += UpdateView;
        }

        private void ResetPanel()
        {
            _isCrafting = false;
            _processBarImage.fillAmount = 0;
            _craftingItemPieceList.ForEach(item =>
            {
                item.Reset();
            });
            _craftingButton.ProcessState(ECraftingButtonState.Disable);
           
            ClearUI();
        }

        private void ClearUI() {
                // Clear UI
            foreach (var item in _craftingItemList)
            {
                Destroy(item);
            }
            _craftingItemList.Clear();
        }
        public void UpdateView()
        {
            _loaderIcon.SetActive(true);

            ClearUI();

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
            _loaderIcon.SetActive(false);

        }

        public void AddItemToCrafting(UICraftItemSlot craftItemSlot)
        {
            int indexToPlace = _craftingItemPieceList.FindIndex(piece => piece.HasPiece == false);
            if (indexToPlace == -1) return;
            _craftingItemPieceList[indexToPlace].Setup(craftItemSlot);
            craftItemSlot.GetItemToCraft();
            CheckCanCraft();
        }
        
        public bool CheckCanCraft()
        {
            int indexToPlace = _craftingItemPieceList.FindIndex(piece => piece.HasPiece == false);

            bool canCraft = indexToPlace == -1;

            _craftingButton.ProcessState(canCraft ? ECraftingButtonState.Enable : ECraftingButtonState.Disable);

            return canCraft;
        }

        public void CraftItem()
        {
            _isCrafting = true;
            _craftingButton.ProcessState(ECraftingButtonState.Processing);

            DOTween.To(() => _processBarImage.fillAmount, x => _processBarImage.fillAmount = x, 0.82f, 5f)
                .OnComplete(() =>
                {
                    DOTween.To(() => _processBarImage.fillAmount, x => _processBarImage.fillAmount = x, 1f, 1.5f)
                        .OnComplete(() =>
                        {
                            Debug.Log("Display Claim");
                            ResetPanel();
                            _craftingPopup.ProcessNextStep(ECraftingState.Complete, ECraftingType.None);

                        });
                });
        }

    }

}
