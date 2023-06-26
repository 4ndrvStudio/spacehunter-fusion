using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

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
        [SerializeField] private Button _backButton;

        [SerializeField] private UICharacterRenderTexture _uiCharacterRenderTexture;

        public bool HasWeapon;
        public bool HasGear;

        public string CurrentWeaponEquipedAddress;
        public string CurrentGearEquipedAddress;


        // Start is called before the first frame update
        void Start()
        {
            _uiCharacterRenderTexture.SetHideWeapon();
            
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

            _inventoryButton.onClick.AddListener(() => {
                UIManager.Instance.ShowPopup(PopupName.Inventory);
                Hide();
            });

        }

        public void OnEnable() {
            ChangeTab(UICharacterTabName.Races);
              UpdateSuiBalance();
               _tabButtonList.ForEach(tab => {
                    tab.SetDeactive();
                    if(tab.TabName == UICharacterTabName.Races) {
                        tab.SetActive();
                    }
               } );
              
              
        }
        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);
            UIControllerManager.Instance.HideAllController();

        }

        public override void Hide()
        {
            base.Hide();
            UIControllerManager.Instance.DisplayController();

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

        public async void GetEquipedItem() {
            
            var data = await SuiWalletManager.GetHunterWeaponEquipment();
            var listItem = data.Result.Data.ToList().FindAll(data => data.ObjectType.Type.Contains("item::Item"));
            var listWeapon = data.Result.Data.ToList().FindAll(data => data.ObjectType.Type.Contains("sword::Sword"));
         
            HasWeapon = listWeapon.Count > 0 ? true: false;
           
            if(listItem.Count >0 ) {
                HasGear = true;
                CurrentGearEquipedAddress = listItem[0].ObjectId;
                _uiCharacterRenderTexture.EquipGear(true);
            } else {
                HasGear = false;
                 _uiCharacterRenderTexture.EquipGear(false);
            }
             if(listWeapon.Count >0 ) {
                HasWeapon = true;
                CurrentWeaponEquipedAddress = listWeapon[0].ObjectId;
            } else {
                HasWeapon = false;
            }
        }

    }

}

