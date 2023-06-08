using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SH.UI
{
    public class UIItemSlot : MonoBehaviour
    {
        //PUBLIC MEMEBER
        [HideInInspector] public int Level;
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

        void Start() {
            _itemButton.onClick.AddListener(() => {
                UIInventoryPopup.Instance.SetPreUseItem(this);
                IsPreUse(true);
            });
        }

        public void StackItem() {
            _quantity++;
            _quantityTextUI.text = _quantity.ToString();
            _quantityFrameUI.gameObject.SetActive(true);
        }

        public void Setup(int level , ItemConfig itemConfig) {
            
            _itemConfig = itemConfig;
            _quantity = 1;
            Level = level;
          
            _itemFrameUI.sprite = InventoryManager.Instance.ItemFrame[Level-1];
            
            if(_itemConfig.IconWithLevel.Count > 0) 
                _itemIconUI.sprite = _itemConfig.IconWithLevel[Level-1];
            else 
                _itemIconUI.sprite = _itemConfig.ItemIcon;
            
        }

        public void IsPreUse(bool isPreUse) {
            _itemActiveFrame.SetActive(isPreUse);
        }
        


        



    }


}
