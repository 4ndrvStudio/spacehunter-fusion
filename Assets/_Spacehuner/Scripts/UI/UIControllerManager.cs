using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SH.Define;
using SH.Multiplayer;

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
            _gotoMiningBtn.onClick.AddListener(() => GotoMining());

        }

        public void OpenInventory() {
            HideAllController();
            UIManager.Instance.ShowPopup(PopupName.Inventory);
        }

        public void DisplayController() {
            
            _playerState  = Network_Player.Local.PlayerState;
            
            if(_playerState.L_IsInsideBuilding) {
                ActiveActionControlller();
            }  else { 
                ActiveCombatController();
            }

            _movementJoy.gameObject.SetActive(true);

            _touchPanel.enabled = true;

            IsActive = true;
        }

        private void ActiveCombatController() {

            _combatGroup.SetActive(true);

            _actionGroup.SetActive(false);

        }

        private void ActiveActionControlller() {
            _combatGroup.SetActive(false);

            _actionGroup.SetActive(true);
        }

        public void HideAllController() {

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

        public void GotoMining()
        {
            Network_ClientManager.MoveToRoom(SceneDefs.scene_mining);
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





    }

}
