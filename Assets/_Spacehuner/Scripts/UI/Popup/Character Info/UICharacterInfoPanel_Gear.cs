using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using System.Linq;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SH.UI
{
    public class UICharacterInfoPanel_Gear : UICharacterInfoPanel
    {
        [SerializeField] private UICharacterInfoPopup _uiCharacterInfoPopup;
        [SerializeField] private UICharacterRenderTexture _uiCharacterRenderTexture;

        
        [SerializeField] private GameObject _noneGearOb;
        [SerializeField] private GameObject _hasGearOb;
        [SerializeField] private GameObject _gearEquipOb;
        [SerializeField] private GameObject _loaderIcon;

           [Header("HasWeapon")]
        [SerializeField] private GameObject _gearSlotOb;
        [SerializeField] private GameObject _gearContentHolder;
        [SerializeField] private List<GameObject> _gearItemList = new List<GameObject>();
        [SerializeField] private GameObject _gearHolderLoaderIcon;

        [SerializeField] private string _selectedGearAddress;

        [SerializeField] private Button _btnUseGear;

        
        [Header("WeaponEquiped")]
        [SerializeField] private string _currentWeaponEquipedAddress;
        [SerializeField] private TextMeshProUGUI _weaponNameText;
        [SerializeField] private TextMeshProUGUI _weaponIdText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _unEquipButton;


         private RpcResult<TransactionBlockBytes> _currentTx;

        public static UnityAction ConfirmGasFeesEquipAction;
        public static UnityAction ConfirmGasFeesUnEquipAction;
        

        void OnEnable() {
            ConfirmGasFeesEquipAction += WeaponUsedStep;
            ConfirmGasFeesUnEquipAction += UnWeaponUsedStep;
            
        }
        void OnDisable() {
            ConfirmGasFeesEquipAction -= WeaponUsedStep;
            ConfirmGasFeesUnEquipAction -= UnWeaponUsedStep;

        }

        void Start() {
            _btnUseGear.onClick.AddListener(() => UseWeaponClick());
            _unEquipButton.onClick.AddListener(() => UnEquipWeaponClick());
        }

        public override void Display()
        {
           ProcessState();
            _uiCharacterRenderTexture.SetToIdle(true);
        }
        public override void Hide()
        {
           HideWeaponPanel();
        }

        private void HideWeaponPanel()
        {
            _noneGearOb.SetActive(false);
            _hasGearOb.SetActive(false);
            _gearEquipOb.SetActive(false);
        }

        public void SelectWeapon(string address) {
            _gearItemList.ForEach(weaponItem => {
                weaponItem.GetComponent<UICharacterGearSlot>().DeSelect();
            });
            _selectedGearAddress = address;
        }


        private async void ProcessState()
        {
            _currentTx = null;
            _selectedGearAddress = null;
            _loaderIcon.gameObject.SetActive(true);
            HideWeaponPanel();
           

            var data = await SuiWalletManager.GetHunterWeaponEquipment();
            
            var listItem = data.Result.Data.ToList().FindAll(data => data.ObjectType.Type.Contains("item::Item"));
            await InventoryManager.Instance.GetInventoryData();
           
           
            if(_uiCharacterInfoPopup.CurrentTab != UICharacterTabName.Gear) {
                  _loaderIcon.gameObject.SetActive(false);
                  return;
            }       

           

            if (listItem.Count > 0)
            {
                _currentWeaponEquipedAddress = listItem[0].ObjectId;
                ShowWeaponEquipPanel();
               _uiCharacterRenderTexture.EquipGear(true);
            }
            else
            {
                _uiCharacterRenderTexture.EquipGear(false);

                List<ItemInstance> itemList = new List<ItemInstance>();

                itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemId == "glasses");

                if (itemList.Count > 0)
                {
                    ShowHasWeaponPanel();
                }
                else
                {
                    ShowNoneWeaponPanel();
                }
            }
             _loaderIcon.gameObject.SetActive(false);

        }
        private void ShowNoneWeaponPanel()
        {
            _noneGearOb.SetActive(true);
        }
        private void ShowHasWeaponPanel()
        {
         
            _hasGearOb.SetActive(true);
            GetWeapons();

        }

        private async void ShowWeaponEquipPanel()
        {
            _gearEquipOb.SetActive(true);
           // _uiCharacterRenderTexture.SetToWeapon();

            if(_currentWeaponEquipedAddress != string.Empty) {
                var weaponResult = await SuiWalletManager.GetWeaponInfo(_currentWeaponEquipedAddress);
    
                if(weaponResult.IsSuccess == true) {
                string nameNft = weaponResult.Result.Data.Display.Data["name"];
                int indexSharp = nameNft.IndexOf("#");

                _weaponNameText.text = nameNft.Substring(0,nameNft.Length - (nameNft.Length - indexSharp));

                _weaponIdText.text = nameNft.Substring(indexSharp);
                
                string jsonNft = JsonConvert.SerializeObject(weaponResult.Result.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);
                 _descriptionText.text = weaponResult.Result.Data.Display.Data["description"];
                }
               
                
            }
          
        }

        

        private void ClearUI()
        {
            // Clear UI
            foreach (var item in _gearItemList)
            {
                Destroy(item);
            }
            _gearItemList.Clear();
        }

        private void GetWeapons()
        {

            _gearHolderLoaderIcon.SetActive(true);

            ClearUI();

            List<ItemInstance> itemList = new List<ItemInstance>();

            itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemId == "glasses");
            bool isSelected = false;
            foreach (var item in itemList)
            {
                
                string address = item.CustomData["Address"].ToString();

                int level = int.Parse(item.CustomData["Level"]);

                GameObject inventoryItemEl = Instantiate(_gearSlotOb, _gearContentHolder.transform);
                UICharacterGearSlot inventoryElScript = inventoryItemEl.GetComponent<UICharacterGearSlot>();

                ItemConfig itemConfig = InventoryManager.Instance.ItemConfigs.Find(itemConfig => itemConfig.ItemId == item.ItemId);
               
                if (item.ItemInstanceId != null)
                {
                    itemConfig.ItemInstanceId = item.ItemInstanceId;
                }
               inventoryElScript.Setup(level, itemConfig, address, this);
               
                _gearItemList.Add(inventoryItemEl);

                if(isSelected == false) {
                    inventoryElScript.Select();
                    isSelected = true;
                }

            }

            _gearHolderLoaderIcon.SetActive(false);
        }

        private async void UseWeaponClick() {

            UIManager.Instance.ShowWaiting();

            var rpcResult =  await SuiWalletManager.EquipWeapon(_selectedGearAddress,"item");
            _currentTx = rpcResult;

            var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(rpcResult.Result.TxBytes.ToString()); 
            
            JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);

            JArray balanceChangesArray = (JArray)jsonObject["result"]["balanceChanges"];
       
            SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();
            gasFeesModel.CanExcute = true;
            if(balanceChangesArray[0]["amount"].ToString() != null)
                gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
            else 
                gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
            UIManager.Instance.HideWaiting();
            UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas,gasFeesModel, ConfirmGasFeesEquipAction);
        }

        private async void WeaponUsedStep() {
            if(_currentTx == null) return;
            
            UIManager.Instance.ShowWaiting();
            var rpcResult = await SuiWalletManager.Execute(_currentTx);
            _uiCharacterInfoPopup.GetEquipedItem();
            UIManager.Instance.HideWaiting();
            
            if(rpcResult.IsSuccess == true) {
                Debug.Log("Called Here");
                ProcessState();
                SuiTxSuccessModel txSuccessModel = new SuiTxSuccessModel();
                txSuccessModel.Message = "Your hunter has been equipped with a weapon!";
                txSuccessModel.ObjectID = rpcResult.Result.Digest;
                UIManager.Instance.ShowPopup(PopupName.SuiTxSuccess, txSuccessModel);

            } else {
                UIManager.Instance.ShowAlert("Some thing wrong. Please recheck", AlertType.Warning);
            }
            _uiCharacterInfoPopup.UpdateSuiBalance();
         


        }

        private async void UnEquipWeaponClick() {

            UIManager.Instance.ShowWaiting();

            var rpcResult =  await SuiWalletManager.UnEquipWeapon(_currentWeaponEquipedAddress);
            _currentTx = rpcResult;

            var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(rpcResult.Result.TxBytes.ToString()); 
            
            JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);

            JArray balanceChangesArray = (JArray)jsonObject["result"]["balanceChanges"];
       
            SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();
            gasFeesModel.CanExcute = true;
            if(balanceChangesArray[0]["amount"].ToString() != null)
                gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
            else 
                gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
            UIManager.Instance.HideWaiting();
            UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas, gasFeesModel, ConfirmGasFeesUnEquipAction);
            
        
        }

        private async void UnWeaponUsedStep() {
            if(_currentTx == null) return;
            
            UIManager.Instance.ShowWaiting();
            var rpcResult = await SuiWalletManager.Execute(_currentTx);
             _uiCharacterInfoPopup.GetEquipedItem();
            UIManager.Instance.HideWaiting();
            
            if(rpcResult.IsSuccess == true) {
                ProcessState();
                SuiTxSuccessModel txSuccessModel = new SuiTxSuccessModel();
                txSuccessModel.Message = "Your hunter has unequipped the weapon!";
                txSuccessModel.ObjectID = rpcResult.Result.Digest;
                UIManager.Instance.ShowPopup(PopupName.SuiTxSuccess, txSuccessModel);
            } else {
                UIManager.Instance.ShowAlert("Some thing wrong. Please recheck", AlertType.Warning);
            }
            _uiCharacterInfoPopup.UpdateSuiBalance();
           
        }

        
    }

}
