using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SH.UI
{
    public enum UICharacterTabName
    {
        Races,
        Weapon,
        Gear,
        SpaceShip,
        Super,
    }
    public class UICharacterInfoPopup : UIPopup
    {
         public UICharacterTabName CurrentTab;
        [SerializeField] private List<UICharacterTabButton> _tabButtonList = new List<UICharacterTabButton>();
        [SerializeField] private List<UICharacterInfoPanel> _characteInfoPanelList = new List<UICharacterInfoPanel>();
        
        
        [SerializeField] private TextMeshProUGUI _suiBalanceText;
        [SerializeField] private Button _inventoryButton;

        // Start is called before the first frame update
        void Start()
        {
            _tabButtonList.ForEach(tabButton =>
            {
                tabButton.Button.onClick.AddListener(() =>
                {
                    ChangeTab(tabButton.TabName);
                    _tabButtonList.ForEach(tab => tab.SetDeactive());
                    tabButton.SetActive();
                });
            });

            _inventoryButton.onClick.AddListener(() => {
                UIManager.Instance.ShowPopup(PopupName.Inventory);
                Hide();
            });

            UpdateSuiBalance();
        }

        public async void UpdateSuiBalance() {
            _suiBalanceText.text = await SuiWalletManager.GetSuiWalletBalance();
        }


        public void ChangeTab(UICharacterTabName tab)
        {
            CurrentTab = tab;
            _characteInfoPanelList.ForEach(infoPanel => {
                infoPanel.Hide();
                if(infoPanel.TabName == tab)
                    infoPanel.Display();
            });
        }

    }

}

