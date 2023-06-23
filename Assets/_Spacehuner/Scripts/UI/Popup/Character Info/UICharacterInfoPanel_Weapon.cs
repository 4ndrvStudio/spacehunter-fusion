using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using SH;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine.UI;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using UnityEngine.Events;
using TMPro;
using Suinet.Rpc.Types.MoveTypes;

namespace SH.UI
{
    public class UICharacterInfoPanel_Weapon : UICharacterInfoPanel
    {
        [Header("Weapon Panel")]
        [SerializeField] private UICharacterInfoPopup _uiCharacterInfoPopup;
        [SerializeField] private UICharacterRenderTexture _uiCharacterRenderTexture;

        [SerializeField] private GameObject _noneWeaponOb;
        [SerializeField] private GameObject _hasWeaponOb;
        [SerializeField] private GameObject _weaponEquipOb;
        [SerializeField] private GameObject _loaderIcon;

        [Header("HasWeapon")]
        [SerializeField] private GameObject _weaponSlotOb;
        [SerializeField] private GameObject _weaponContentHolder;
        [SerializeField] private List<GameObject> _weaponItemList = new List<GameObject>();
        [SerializeField] private GameObject _weaponHolderLoaderIcon;

        [SerializeField] private string _selectedWeaponAddress;

        [SerializeField] private Button _btnUseWeapon;

        [Header("WeaponEquiped")]
        [SerializeField] private string _currentWeaponEquipedAddress;
        [SerializeField] private TextMeshProUGUI _weaponNameText;
        [SerializeField] private TextMeshProUGUI _weaponIdText;
        [SerializeField] private TextMeshProUGUI _weaponLevelText;
        [SerializeField] private TextMeshProUGUI _attackText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _unEquipButton;


        private RpcResult<TransactionBlockBytes> _currentTx;

        public static UnityAction ConfirmGasFeesEquipAction;
        public static UnityAction ConfirmGasFeesUnEquipAction;

        public string targetItemName = "sui_weapon";



        void OnEnable() {
            ConfirmGasFeesEquipAction += WeaponUsedStep;
            ConfirmGasFeesUnEquipAction += UnWeaponUsedStep;
            
        }
        void OnDisable() {
            ConfirmGasFeesEquipAction -= WeaponUsedStep;
            ConfirmGasFeesUnEquipAction -= UnWeaponUsedStep;

        }

        void Start() {
            _btnUseWeapon.onClick.AddListener(() => UseWeaponClick());
            _unEquipButton.onClick.AddListener(() => UnEquipWeaponClick());
        }

        public override void Display()
        {
            ProcessState();
        }
        public override void Hide()
        {
            HideWeaponPanel();
        }

        private void HideWeaponPanel()
        {
            _noneWeaponOb.SetActive(false);
            _hasWeaponOb.SetActive(false);
            _weaponEquipOb.SetActive(false);
        }

        public void SelectWeapon(string address) {
            _weaponItemList.ForEach(weaponItem => {
                weaponItem.GetComponent<UICharacterWeaponSlot>().DeSelect();
            });
            _selectedWeaponAddress = address;
        }




        private async void ProcessState()
        {
            _currentTx = null;
            _selectedWeaponAddress = null;
            _loaderIcon.gameObject.SetActive(true);
            HideWeaponPanel();

            var data = await SuiWalletManager.GetHunterWeaponEquipment();

            var listWeapon = data.Result.Data.ToList();

            if(_uiCharacterInfoPopup.CurrentTab != UICharacterTabName.Weapon) {
                  _loaderIcon.gameObject.SetActive(false);
                  return;
            }   


            if (listWeapon.Count > 0)
            {
                _currentWeaponEquipedAddress = listWeapon[0].ObjectId;
                ShowWeaponEquipPanel();
                _uiCharacterInfoPopup.HasWeapon = true;
            }
            else
            {
                _uiCharacterInfoPopup.HasWeapon = false;

                if(_uiCharacterInfoPopup.HasWeapon == true ) {
                     _uiCharacterRenderTexture.SetToIdle(false);
                } else {
                    _uiCharacterRenderTexture.SetToIdle(true);
                }

                List<ItemInstance> itemList = new List<ItemInstance>();

                itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemId == targetItemName);

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
            _noneWeaponOb.SetActive(true);
        }
        private void ShowHasWeaponPanel()
        {
         
           

            _hasWeaponOb.SetActive(true);
            GetWeapons();

        }

        private async void ShowWeaponEquipPanel()
        {
            _weaponEquipOb.SetActive(true);
            _uiCharacterRenderTexture.SetToWeapon();

            if(_currentWeaponEquipedAddress != string.Empty) {
                var weaponResult = await SuiWalletManager.GetWeaponInfo(_currentWeaponEquipedAddress);
    
                if(weaponResult.IsSuccess == true) {
                string nameNft = weaponResult.Result.Data.Display.Data["name"];
                int indexSharp = nameNft.IndexOf("#");

                _weaponNameText.text = nameNft.Substring(0,nameNft.Length - (nameNft.Length - indexSharp));

                _weaponIdText.text = nameNft.Substring(indexSharp);
                
                string jsonNft = JsonConvert.SerializeObject(weaponResult.Result.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);

                 _weaponLevelText.text =  "Lv " + nftJsonObject.SelectToken("fields.level").ToString();
                 _attackText.text = nftJsonObject.SelectToken("fields.damage").ToString();

                 _descriptionText.text = weaponResult.Result.Data.Display.Data["description"];
                }
               
                
            }
          
        }

   

        private void ClearUI()
        {
            // Clear UI
            foreach (var item in _weaponItemList)
            {
                Destroy(item);
            }
            _weaponItemList.Clear();
        }

        private void GetWeapons()
        {

            _weaponHolderLoaderIcon.SetActive(true);

            ClearUI();

            List<ItemInstance> itemList = new List<ItemInstance>();
            InventoryManager.Instance.GetInventoryData();
            itemList = InventoryManager.Instance.Items.FindAll(item => item.ItemId == "sui_weapon");

            bool isSelected = false;
            foreach (var item in itemList)
            {
                
                string address = item.CustomData["Address"].ToString();

                int level = int.Parse(item.CustomData["Level"]);

                GameObject inventoryItemEl = Instantiate(_weaponSlotOb, _weaponContentHolder.transform);
                UICharacterWeaponSlot inventoryElScript = inventoryItemEl.GetComponent<UICharacterWeaponSlot>();

                ItemConfig itemConfig = InventoryManager.Instance.ItemConfigs.Find(itemConfig => itemConfig.ItemId == item.ItemId);
               
                if (item.ItemInstanceId != null)
                {
                    itemConfig.ItemInstanceId = item.ItemInstanceId;
                }
                inventoryElScript.Setup(level, itemConfig, address, this);
               
                _weaponItemList.Add(inventoryItemEl);

                if(isSelected == false) {
                    inventoryElScript.Select();
                    isSelected = true;
                }

            }

            _weaponHolderLoaderIcon.SetActive(false);
        }

        private async void UseWeaponClick() {

            UIManager.Instance.ShowWaiting();

            var rpcResult =  await SuiWalletManager.EquipWeapon(_selectedWeaponAddress);
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
