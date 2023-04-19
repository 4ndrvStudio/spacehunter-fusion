using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SH.Define;

namespace SH.Multiplayer
{
    public class UIControllerManager : MonoBehaviour
    {

        public static UIControllerManager Instance;

        [Header("Controller")]
        [SerializeField] private Image _touchPanel;
        [SerializeField] private Joystick _movementJoy;
        [SerializeField] private UIButtonCustom _attackBtn;
        [SerializeField] private UIButtonCustom _jumpBtn;
        [SerializeField] private UIButtonCustom _combo1Btn;
        [SerializeField] private UIButtonCustom _dashAttackBtn;
        [SerializeField] private UIButtonCustom _activeTestModeBtn;

        [Header("UI")]
        [SerializeField] private Button _inventoryBtn;
        [SerializeField] private Button _gotoMiningBtn;

        [Header("Interact")]
        [SerializeField] private GameObject _interactBtnPanel;
        [SerializeField] private List<UIInteractButton> _interactBtnList = new List<UIInteractButton>();

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
            _inventoryBtn.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.Inventory));
            _gotoMiningBtn.onClick.AddListener(() => GotoMining());
        }

        public void ActiveController(bool isActive)
        {
            _movementJoy.gameObject.SetActive(isActive);
            _attackBtn.gameObject.SetActive(isActive);
            _jumpBtn.gameObject.SetActive(isActive);
            //_activeTestModeBtn.gameObject.SetActive(isActive);
            _combo1Btn.gameObject.SetActive(isActive);
            _dashAttackBtn.gameObject.SetActive(isActive);
            _inventoryBtn.gameObject.SetActive(isActive);

            _touchPanel.enabled = true;

            IsActive = true;
        }

        public void HideController()
        {

        }

        public void ActiveTestModeController(bool isActive)
        {
            _combo1Btn.gameObject.SetActive(isActive);
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
            Network_ClientManager.MoveToRoom(SceneDefs.scene_miningFusion);
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
