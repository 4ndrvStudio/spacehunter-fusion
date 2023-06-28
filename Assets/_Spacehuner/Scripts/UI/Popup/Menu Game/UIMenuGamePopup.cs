using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SH.PlayerData;
using SH.Multiplayer;
using UnityEngine.SceneManagement;
namespace SH.UI
{
    public enum UIMenuTabName
    {
        Profile,
        Setting
    }
    public class UIMenuGamePopup : UIPopup
    {
        public UIMenuTabName CurrentTab;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _logoutButton;
        [SerializeField] private List<UIMenuTabButton> _tabButtonList = new List<UIMenuTabButton>();
        [SerializeField] private List<UIMenuPanel> _menuPanelList = new List<UIMenuPanel>();
        
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private TextMeshProUGUI _suiBalanceText;

        [SerializeField] private GameObject _exitGamePanel;
        [SerializeField] private Button _confirmExit;
        [SerializeField] private Button _cancelExit;

        private bool IsLogout;

        private void Start()
        {
            _backButton.onClick.AddListener(() => Hide());

            _tabButtonList.ForEach(tabButton =>
            {
                tabButton.Button.onClick.AddListener(() =>
                {
                    ChangeTab(tabButton.TabName);
                    _tabButtonList.ForEach(tab => tab.SetDeactive());
                    tabButton.SetActive();
                });
            });

            _logoutButton.onClick.AddListener(() => _exitGamePanel.gameObject.SetActive(true));
            _cancelExit.onClick.AddListener(() => _exitGamePanel.gameObject.SetActive(false));
            _confirmExit.onClick.AddListener(() => Logout());
        

        }

        public void OnEnable()  {
            _playerNameText.text = PlayerDataManager.DisplayName;
            
            ChangeTab(UIMenuTabName.Profile);
           
            _tabButtonList.ForEach(tab =>
            {
                tab.SetDeactive();
                if (tab.TabName == UIMenuTabName.Profile)
                {
                    tab.SetActive();
                }
            });
        }

        public async void Logout() {
            IsLogout = true; 
            UIManager.Instance.ShowWaiting();
            _exitGamePanel.gameObject.SetActive(false);
            SuiWallet.Logout();
            await Network_ClientManager._networkGameManager.LeaveRom();
            UIManager.Instance.HideWaiting();
            Hide();
            SceneManager.LoadScene("scene_login");
       
        } 



        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);

            UpdateSuiBalance();
            IsLogout = false;
            UIControllerManager.Instance.HideAllController();

        }

        public override void Hide()
        {
            base.Hide();

            if(IsLogout == true) return;

            UIControllerManager.Instance.DisplayController();
        }

        public async void UpdateSuiBalance() {
            _suiBalanceText.text =  await SuiWalletManager.GetSuiWalletBalance();
        }

        public void ChangeTab(UIMenuTabName tab)
        {
            CurrentTab = tab;

            _menuPanelList.ForEach(infoPanel =>
            {
                infoPanel.Hide();
                if (infoPanel.TabName == tab)
                    infoPanel.Display();
            });
        }
    }

}
