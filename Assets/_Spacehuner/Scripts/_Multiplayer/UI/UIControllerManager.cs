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

        [SerializeField] private Image _touchPanel;
        [SerializeField] private Joystick _movementJoy;
        [SerializeField] private UIButtonCustom _attackBtn;
        [SerializeField] private UIButtonCustom _jumpBtn;
        [SerializeField] private UIButtonCustom _combo1Btn;
        [SerializeField] private UIButtonCustom _activeTestModeBtn;

        //local
        [SerializeField] private Button _inventoryBtn;
        [SerializeField] private Button _gotoMiningBtn;


        public bool IsActive = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start() {
            _inventoryBtn.onClick.AddListener(() => UIManager.Instance.ShowPopup(PopupName.Inventory));
            _gotoMiningBtn.onClick.AddListener(() => GotoMining());
        }

        public void ActiveController(bool isActive)
        {
            _movementJoy.gameObject.SetActive(isActive);
            _attackBtn.gameObject.SetActive(isActive);
            _jumpBtn.gameObject.SetActive(isActive);
            _activeTestModeBtn.gameObject.SetActive(isActive);

            _inventoryBtn.gameObject.SetActive(true);

            _touchPanel.enabled = true;
            IsActive =true;
        }

        public void ActiveTestModeController(bool isActive) {
            _combo1Btn.gameObject.SetActive(isActive);
        }

        public Joystick GetMovementJoystick() => _movementJoy;

        public bool GetAttackBtn() => _attackBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetCombo1Btn() => _combo1Btn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetJumpBtn() => _jumpBtn.GetComponent<UIButtonCustom>().IsPressed;

        public bool GetActiveTestModeBtn() => _activeTestModeBtn.GetComponent<UIButtonCustom>().IsPressed;

        public void ShowGotoMiningBtn(bool isActive) => _gotoMiningBtn.gameObject.SetActive(isActive);

        public void GotoMining() {

             Network_ClientManager.MoveToRoom(SceneDefs.scene_miningFusion);

        }

        

    }

}
