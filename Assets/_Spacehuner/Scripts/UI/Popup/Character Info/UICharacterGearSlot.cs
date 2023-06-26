using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SH.UI {
    public class UICharacterGearSlot : MonoBehaviour
{
   //PUBLIC MEMEBER
        [HideInInspector] public int Level;
        [HideInInspector] public UICharacterInfoPanel_Gear _uiWeaponPanel;
        public Sprite ItemIcon => _itemIconUI.sprite;

        //PRIVATE MEMEBER
        [SerializeField] private Button _itemButton;
        [SerializeField] private Image _itemFrameUI;
        [SerializeField] private GameObject _itemActiveFrame;
        [SerializeField] private Image _itemIconUI;

        private ItemConfig _itemConfig;
        public ItemConfig ItemConfig => _itemConfig;

        [SerializeField] private Image _quantityFrameUI;
        [SerializeField] private TextMeshProUGUI _quantityTextUI;
        [SerializeField] private int _quantity = 1;

        [SerializeField] private string _weaponAddress;

        void Start() {
            _itemButton.onClick.AddListener(() => Select());
        }
        // public void StackItem() {
        //     _quantity++;
        //     _quantityTextUI.text = _quantity.ToString();
        //     _quantityFrameUI.gameObject.SetActive(true);
        // }

        public void Select() {
            
            _uiWeaponPanel.SelectWeapon(_weaponAddress);
      
            _itemActiveFrame.SetActive(true);
        }

        public void DeSelect() => _itemActiveFrame.SetActive(false);
           

        public void Setup(int level , ItemConfig itemConfig, string address, UICharacterInfoPanel_Gear weaponUI) {
            _uiWeaponPanel = weaponUI;
            _weaponAddress = address;
            _itemConfig = itemConfig;
            _quantity = 1;
            Level = level;

            _itemFrameUI.sprite = InventoryManager.Instance.ItemFrame[Level-1];
            
            if(_itemConfig.IconWithLevel.Count > 0) 
                _itemIconUI.sprite = _itemConfig.IconWithLevel[Level-1];
            else 
                _itemIconUI.sprite = _itemConfig.ItemIcon;
            
        }
}
}

