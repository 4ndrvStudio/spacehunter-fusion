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
using DG.Tweening;
using Newtonsoft.Json;

namespace SH
{
    public enum ControllerType
    {
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
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _hunterButton;
        [SerializeField] private Button _gotoMiningButton;
        [SerializeField] private Button _menuButton;

        [Header("Interact")]
        [SerializeField] private GameObject _interactBtnPanel;
        [SerializeField] private List<UIInteractButton> _interactBtnList = new List<UIInteractButton>();

        [SerializeField] private ControllerType _currentController;
        [SerializeField] private Network_PlayerState _playerState;

        public bool IsActive = false;

        [Header("SUI Properties")]
        [SerializeField] private TextMeshProUGUI _suiBalanceText;


        public static UnityAction<bool> UIControllerEvent;

        [Header("Mining Property")]
        [SerializeField] private Button _miningButtonBack;


        [Header("Player Stats")]
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private Image _hpBar;
        [SerializeField] private TextMeshProUGUI _avatarLevelText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private Image _expBar;

        private bool IsExecutingGoToMining;

        //sui 
        private RpcResult<TransactionBlockBytes> _currentTx;
        public static UnityAction ConfirmGasFeesAction { get; set; }
        public static UnityAction ExitMiningAction { get; set; }

        private void OnEnable()
        {
            ConfirmGasFeesAction += GotoMining;
        }
        private void OnDisble()
        {
            ConfirmGasFeesAction -= GotoMining;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }


        }

        public void SetLevel()
        {

        }

        void Start()
        {
            _inventoryButton.onClick.AddListener(() => OpenInventory());
            _hunterButton.onClick.AddListener(() => OpenHunterInfo());
            _gotoMiningButton.onClick.AddListener(() => PrepareToGotoMining());
            _menuButton.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.MenuGame));
            _miningButtonBack.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.ExitMiningPopup));

        }

        public void ToggleAttackButton(bool isActive)
        {

            _attackBtn.Interactable = isActive;
            _attackBtn.GetComponent<Button>().interactable = isActive;

            _attackBtn.GetComponent<CanvasGroup>().alpha = isActive ? 1 : 0.5f;

        }



        public async void SetupBalance()
        {
            if (SuiWallet.GetActiveAddress() == null) return;
            string balance = await SuiWalletManager.GetSuiWalletBalance();
            _suiBalanceText.text = balance;
        }

        public async void SetupHunterInfo()
        {
            _playerNameText.text = PlayerData.PlayerDataManager.DisplayName;
            var hunterResult = await SuiWalletManager.GetHunterInfo(InventoryManager.Instance.CurrentHunterAddressInUse);

            if (hunterResult.IsSuccess == true)
            {
                string jsonNft = JsonConvert.SerializeObject(hunterResult.Result.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);
                string level = nftJsonObject.SelectToken("fields.level").ToString();
                string exp = nftJsonObject.SelectToken("fields.exp").ToString();
                _levelText.text = "Lv " + level;
                _avatarLevelText.text = level;
                float currentExp = float.Parse(exp) < 1000f ? float.Parse(exp) : (float.Parse(exp) - float.Parse(level) * 1000f);
                _expBar.fillAmount = currentExp / 1000f;
                _expText.text = $"{currentExp}/1000";
            }
        }


        public void OpenInventory()
        {
            UIControllerEvent?.Invoke(false);
            HideAllController();
            UIManager.Instance.ShowPopup(PopupName.Inventory);
        }
        public void OpenHunterInfo()
        {
            UIControllerEvent?.Invoke(false);
            HideAllController();
            UIManager.Instance.ShowPopup(PopupName.CharacterInfo);
        }

        public void DisplayController()
        {

            _playerState = Network_Player.Local.PlayerState;

            SetupBalance();
            SetupHunterInfo();

            if (_playerState.L_IsInsideBuilding)
            {
                ActiveActionControlller();
            }
            else
            {
                ActiveCombatController();
            }

            _movementJoy.gameObject.SetActive(true);

            _touchPanel.enabled = true;

            UIControllerEvent?.Invoke(true);
            IsActive = true;

            // //sceneMining Test
            // _uiSceneMining.gameObject.SetActive(true);
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
              _touchPanel.enabled = false;

            IsActive = false;
        }


        public Joystick GetMovementJoystick() => _movementJoy;

        public bool GetAttackBtn() => _attackBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetCombo1Btn() => _combo1Btn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetJumpBtn() => _jumpBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetDashAttackBtn() => _dashAttackBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetActiveTestModeBtn() => _activeTestModeBtn.GetComponent<UIButtonCustom>().IsPressed;

        public void ShowGotoMiningBtn(bool isActive) => _gotoMiningButton.gameObject.SetActive(isActive);

        public UITouchField GetTouchField() => _touchfield;

        private async void PrepareToGotoMining()
        {
            if(IsExecutingGoToMining == true) 
                return;
            
            IsExecutingGoToMining = true;

            UIManager.Instance.ShowWaiting();

            var rpcResult = await SuiWalletManager.StartFarming();

            if (rpcResult.IsSuccess)
            {
                _currentTx = rpcResult;

                var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(rpcResult.Result.TxBytes.ToString());

                JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);

                JArray balanceChangesArray = (JArray)jsonObject["result"]["balanceChanges"];

                SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();
                gasFeesModel.CanExcute = true;
                if (balanceChangesArray[0]["amount"].ToString() != null)
                    gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
                else
                    gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas, gasFeesModel, ConfirmGasFeesAction);
            }
            else
            {
                UIManager.Instance.ShowAlert(rpcResult.ErrorMessage, AlertType.Error);
                UIManager.Instance.HideWaiting();
                IsExecutingGoToMining = false;
 
            }

        }

        public async void GotoMining()
        {
            if (_currentTx == null) return;

            UIManager.Instance.ShowWaiting();
            var rpcResult = await SuiWalletManager.Execute(_currentTx);
            UIManager.Instance.HideWaiting();

            if (rpcResult.IsSuccess == true)
            {
                Network_ClientManager.MoveToRoom(SceneDefs.scene_mining);
            }
            else
            {
                UIManager.Instance.ShowAlert("Some thing wrong. Please recheck", AlertType.Warning);
            }

            IsExecutingGoToMining = false;
        }


        public void AddInteractButton(int id, InteractButtonType type, Dictionary<string, object> customProperties)
        {

            int hasIndex = _interactBtnList.FindIndex(interactBtn => interactBtn.Id == id);

            if (hasIndex != -1) return;

            int indexToInstance = _interactBtnList.FindIndex(interactBtn => interactBtn.IsSet == false);

            if (indexToInstance == -1) return;

            _interactBtnList[indexToInstance].SetContentOfButton(type, customProperties);

            _interactBtnList[indexToInstance].Id = id;


        }
        public void RemoveInteractionButton(int id)
        {
            int index = _interactBtnList.FindIndex(interactBtn => interactBtn.Id == id);

            if (index == -1) return;

            _interactBtnList[index].DisableBtn();

        }

        //Mining;
        public void SetHP(int hp)
        {

            _hpText.text = $"{hp}/100";
            float targetFillAmount = (float)hp / 100f;
            DOTween.To(() => _hpBar.fillAmount, x => _hpBar.fillAmount = x, targetFillAmount, 1f);
        }

        public void DisplayMiningButton(bool isActive)
        {
            _miningButtonBack.gameObject.SetActive(isActive);
        }




    }

}
