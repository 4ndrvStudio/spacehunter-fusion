using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SH.Define;
using SH.Multiplayer;
using TMPro;
using UnityEngine.Events;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using Newtonsoft.Json.Linq;

namespace SH
{
    public enum ControllerType {
        Hide,
        Combat,
        Action
    }

    public class UIControllerManager : MonoBehaviour
    {

        public static UIControllerManager Instance;

        [Header("Controller")]
        //combat 
        [SerializeField] private UITouchField _touchfield;
        [SerializeField] private Image _touchPanel;
        [SerializeField] private Joystick _movementJoy;
        [SerializeField] private UIButtonCustom _attackBtn;
        [SerializeField] private UIButtonCustom _jumpBtn;
        [SerializeField] private UIButtonCustom _combo1Btn;
        [SerializeField] private UIButtonCustom _dashAttackBtn;
        [SerializeField] private UIButtonCustom _activeTestModeBtn;

        
        //dance
        [SerializeField] private UIButtonCustom _danceBtn;

        [SerializeField] private GameObject _combatGroup;
        [SerializeField] private GameObject _actionGroup;

        [Header("UI")]
        [SerializeField] private Button _inventoryBtn;
        [SerializeField] private Button _gotoMiningBtn;

        [Header("Interact")]
        [SerializeField] private GameObject _interactBtnPanel;
        [SerializeField] private List<UIInteractButton> _interactBtnList = new List<UIInteractButton>();

        [SerializeField] private ControllerType _currentController;
        [SerializeField] private Network_PlayerState _playerState;
        
        public bool IsActive = false;

        [Header("SUI Properties")]
        [SerializeField] private TextMeshProUGUI _suiAddressText;
        [SerializeField] private TextMeshProUGUI _suiBalanceText;
        [SerializeField] private Button _suiAddressCoppyButton;
        [SerializeField] private Button _suiBalanceRefreshButton;

        public static UnityAction<bool> UIControllerEvent;
        
        [Header("Mining Property")]
        [SerializeField] private GameObject _uiSceneMining;
        [SerializeField] private TextMeshProUGUI _hpText;

        //sui 
        private RpcResult<TransactionBlockBytes> _currentTx;
        public static UnityAction ConfirmGasFeesAction {get ; set;}
        
        private void OnEnable() {
            ConfirmGasFeesAction += GotoMining;
        }
        private void OnDisble() {
            ConfirmGasFeesAction -= GotoMining;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            _inventoryBtn.onClick.AddListener(() => OpenInventory());
            _gotoMiningBtn.onClick.AddListener(() => PrepareToGotoMining());
        }

        private void SetupSUI() {
            
            SetupBalance();

            string suiAddress = SuiWallet.GetActiveAddress();
            Debug.Log(suiAddress);
            _suiAddressText.text = suiAddress.Substring(0, 11) + "..." + suiAddress.Substring(suiAddress.Length - 5);

            _suiAddressCoppyButton.onClick.AddListener(()=> {
                UniClipboard.SetText(SuiWallet.GetActiveAddress());
                UIManager.Instance.ShowAlert("Your address has been coppied!",AlertType.Normal);
            });

            _suiBalanceRefreshButton.onClick.AddListener(() => {
                SetupBalance();
            });
        }
        
        private async void SetupBalance() {
            if(SuiWallet.GetActiveAddress() == null) return;
            string balance = await SuiWalletManager.GetSuiWalletBalance();
            _suiBalanceText.text = $"{balance} SUI";
        }


        public void OpenInventory() {
            UIControllerEvent?.Invoke(false);
            HideAllController();
            UIManager.Instance.ShowPopup(PopupName.Inventory);
        }

        public void DisplayController() {
            
            _playerState  = Network_Player.Local.PlayerState;

            SetupSUI();

            if(_playerState.L_IsInsideBuilding) {
                ActiveActionControlller();
            }  else { 
                ActiveCombatController();
            }

            _movementJoy.gameObject.SetActive(true);

            _touchPanel.enabled = true;

            UIControllerEvent?.Invoke(true);
            IsActive = true;
            
            //sceneMining Test
            _uiSceneMining.gameObject.SetActive(true);
        }

        private void ActiveCombatController() 
        {

            _combatGroup.SetActive(true);

            _actionGroup.SetActive(false);

        }

        private void ActiveActionControlller() 
        {
            _combatGroup.SetActive(false);

            _actionGroup.SetActive(true);
          
        }

        public void HideAllController() 
        {
            _combatGroup.SetActive(false);

            _actionGroup.SetActive(false);

            _movementJoy.gameObject.SetActive(false);

            IsActive = false;
     

        }


        public Joystick GetMovementJoystick() => _movementJoy;

        public bool GetAttackBtn() => _attackBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetCombo1Btn() => _combo1Btn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetJumpBtn() => _jumpBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetDashAttackBtn() => _dashAttackBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetActiveTestModeBtn() => _activeTestModeBtn.GetComponent<UIButtonCustom>().IsPressed;

        public void ShowGotoMiningBtn(bool isActive) => _gotoMiningBtn.gameObject.SetActive(isActive);

        public UITouchField GetTouchField() => _touchfield;

        private async void PrepareToGotoMining() {
             
            UIManager.Instance.ShowWaiting();

            var rpcResult = await SuiWalletManager.StartFarming();

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
            UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas,gasFeesModel, ConfirmGasFeesAction);
        }

        public async void GotoMining()
        {
            if(_currentTx == null) return;

            UIManager.Instance.ShowWaiting();
            var rpcResult = await SuiWalletManager.Execute(_currentTx);
            UIManager.Instance.HideWaiting();
            
            if(rpcResult.IsSuccess == true) {
                Network_ClientManager.MoveToRoom(SceneDefs.scene_mining);
            } else {
                UIManager.Instance.ShowAlert("Some thing wrong. Please recheck", AlertType.Warning);
            }
        }

        public void AddInteractButton(int id, InteractButtonType type,Dictionary<string, object> customProperties)
        {

            int hasIndex = _interactBtnList.FindIndex(interactBtn => interactBtn.Id == id);
            
            if(hasIndex != -1) return;

            int indexToInstance = _interactBtnList.FindIndex(interactBtn => interactBtn.IsSet == false);

            if(indexToInstance == -1) return;
            
            _interactBtnList[indexToInstance].SetContentOfButton(type, customProperties);

            _interactBtnList[indexToInstance].Id = id;


        }
        public void RemoveInteractionButton(int id)
        {
            int index = _interactBtnList.FindIndex(interactBtn => interactBtn.Id == id);

            if(index == -1) return;

            _interactBtnList[index].DisableBtn();

        }

        //Mining;
        public void SetHP(int hp) {
            _hpText.text = $"HP : {hp}";
        }




    }

}
