using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SH.PlayerData;
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

            _logoutButton.onClick.AddListener(() => Debug.Log("Logout"));

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

        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);

            UpdateSuiBalance();

            UIControllerManager.Instance.HideAllController();

        }

        public override void Hide()
        {
            base.Hide();
            
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
