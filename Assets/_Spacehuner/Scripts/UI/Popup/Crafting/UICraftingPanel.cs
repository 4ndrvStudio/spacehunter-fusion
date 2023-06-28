using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using SH.Multiplayer;
using DG.Tweening;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine.Events;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SH.UI
{
    public class UICraftingPanel : MonoBehaviour
    {
        [SerializeField] private Button _closeBtn;
        [SerializeField] private UICraftingPopup _craftingPopup;
        //item
        [SerializeField] private Image _targetCraftingIcon;
        [SerializeField] private Sprite _swordSprite;
        [SerializeField] private Sprite _glassSprite;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _inventoryContentHolder;
        [SerializeField] private List<GameObject> _craftingItemList = new List<GameObject>();
        [SerializeField] private GameObject _loaderIcon;
        //Crafting Setup 
        [SerializeField] private List<UICraftingPiece> _craftingItemPieceList = new List<UICraftingPiece>();
        [SerializeField] private List<string> _stoneAddress = new List<string>();
        [SerializeField] private List<string> _stoneAddressToCraft = new List<string>(3);
        [SerializeField] private UICraftingButton _craftingButton;
        [SerializeField] private Image _processBarImage;
        [SerializeField] private bool _isCrafting;

        private RpcResult<TransactionBlockBytes> _currentTx;

        public static UnityAction ConfirmSwordGasFeesAction;
        public static UnityAction ConfirmGlassGasFeesAction;

        private bool IsExecutingCrafting;


        private void Start()
        {
            _closeBtn.onClick.AddListener(() =>
            {
                if (_isCrafting == true) return;
                ResetPanel();
                _craftingPopup.CloseCrafting();
            });
        }

        public void SetupPanel (ECraftingType craftingType) {
            switch(craftingType) {
                case ECraftingType.Weapon : _targetCraftingIcon.sprite = _swordSprite;
                    break;
                case ECraftingType.Glass : _targetCraftingIcon.sprite = _glassSprite;
                    break;
            }
        }

        private async void OnEnable()
        {
            _loaderIcon.SetActive(true);
            await InventoryManager.Instance.GetInventoryData();
            InventoryManager.OnInventoryDataChange += UpdateView;
            ConfirmSwordGasFeesAction +=  OnConfirmCraftSword;
            ConfirmGlassGasFeesAction += OnConfirmCraftGlass;
        }
        private void OnDisable()
        {
            InventoryManager.OnInventoryDataChange += UpdateView;
             ConfirmSwordGasFeesAction -=  OnConfirmCraftSword;
            ConfirmGlassGasFeesAction -= OnConfirmCraftGlass;
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
            _stoneAddress.Clear();
            _stoneAddressToCraft.Clear();
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
                string address = item.CustomData["Address"].ToString();
                _stoneAddress.Add(address);

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

        public void Craft()
        {
           switch(_craftingPopup._currentCraftType) {
                case ECraftingType.Weapon : CraftSword();
                    break;
                case ECraftingType.Glass : CraftGlass();
                    break;
           }
          
        }

        public async void CraftSword() {
            
            if(IsExecutingCrafting == true) 
                return;
            IsExecutingCrafting = true;

            if(_stoneAddress.Count < 3) return;
            
            _isCrafting = true;

            _craftingButton.ProcessState(ECraftingButtonState.Processing);
            
            List<string> itemToCraftList = _stoneAddress.GetRange(0,3); 
            
            var rpcResult = await SuiWalletManager.CraftSword(itemToCraftList);

            if(rpcResult.IsSuccess) {
                
                _currentTx = rpcResult;
                
                var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(rpcResult.Result.TxBytes.ToString());
                JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);
                JArray balanceChangesArray =(JArray) jsonObject["result"]["balanceChanges"];

                SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();

                gasFeesModel.CanExcute = true;
                if (balanceChangesArray[0]["amount"].ToString() != null)
                    gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
                else
                    gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas, gasFeesModel, ConfirmSwordGasFeesAction);
            }
            else {
                UIManager.Instance.ShowAlert(rpcResult.ErrorMessage, AlertType.Error);
                UIManager.Instance.HideWaiting();
                IsExecutingCrafting = false;
            }
            
            
        }

        public void OnConfirmCraftSword() {
            DOTween.To(() => _processBarImage.fillAmount, x => _processBarImage.fillAmount = x, 0.82f, 5f)
                .OnComplete(async () =>
                {
                    var rpcResult2 = await SuiWalletManager.Execute(_currentTx);

                    DOTween.To(() => _processBarImage.fillAmount, x => _processBarImage.fillAmount = x, 1f, 1.5f)
                        .OnComplete(() =>
                        {
                            if(rpcResult2.IsSuccess == true) {
                                Debug.Log(rpcResult2.RawRpcResponse);
                                ResetPanel();
                                _craftingPopup.ProcessNextStep(ECraftingState.Complete, ECraftingType.Weapon);
                            } else {
                                UIManager.Instance.ShowAlert(rpcResult2.ErrorMessage, AlertType.Error);
                                ResetPanel();
                            }
                            IsExecutingCrafting = false;
                              _currentTx = null;
                        });
                       
            });
           
        }

        
         public async void CraftGlass() {
            
            if(_stoneAddress.Count < 3) return;
            
            _isCrafting = true;

            _craftingButton.ProcessState(ECraftingButtonState.Processing);
            
            List<string> itemToCraftList = _stoneAddress.GetRange(0,3); 
            
            var rpcResult = await SuiWalletManager.CraftGlass(itemToCraftList);

                if(rpcResult.IsSuccess) {
                _currentTx = rpcResult;
                
                var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(rpcResult.Result.TxBytes.ToString());
                JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);
                JArray balanceChangesArray =(JArray) jsonObject["result"]["balanceChanges"];

                SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();

                gasFeesModel.CanExcute = true;
                if (balanceChangesArray[0]["amount"].ToString() != null)
                    gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
                else
                    gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas, gasFeesModel, ConfirmGlassGasFeesAction);
            }
            else {
                UIManager.Instance.ShowAlert(rpcResult.ErrorMessage, AlertType.Error);
                UIManager.Instance.HideWaiting();
                IsExecutingCrafting = false;
            }
            
         
        }

        public void OnConfirmCraftGlass() {
               
               DOTween.To(() => _processBarImage.fillAmount, x => _processBarImage.fillAmount = x, 0.82f, 5f)
                .OnComplete(async () =>
                {
                    var rpcResult2 = await SuiWalletManager.Execute(_currentTx);

                    DOTween.To(() => _processBarImage.fillAmount, x => _processBarImage.fillAmount = x, 1f, 1.5f)
                        .OnComplete(() =>
                        {
                            if(rpcResult2.IsSuccess == true) {
                                Debug.Log(rpcResult2.RawRpcResponse);
                                ResetPanel();
                                _craftingPopup.ProcessNextStep(ECraftingState.Complete, ECraftingType.Glass);
                            } else {
                                UIManager.Instance.ShowAlert(rpcResult2.ErrorMessage, AlertType.Error);
                                ResetPanel();
                            }
                            IsExecutingCrafting = false;
                                  _currentTx = null;
                        });
            });
      
        }   



    }

}
