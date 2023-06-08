using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SH.UI
{
    public class UICraftingPiece : MonoBehaviour
    {
        [SerializeField] private UICraftingPanel _craftingPanel;
        [HideInInspector] public UICraftItemSlot _craftItemSlot;

        [SerializeField] private GameObject _hasItemOb;
        [SerializeField] private GameObject _nullItemOb;
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

        public bool HasPiece = false;

        void Start()
        {
            _itemButton.onClick.AddListener(() =>
            {    
                Reset();
                _craftingPanel.CheckCanCraft();
                _craftItemSlot.StackItem();
            });
        }

        public void Setup(UICraftItemSlot craftItemSlot)
        {
            _craftItemSlot = craftItemSlot;
            _itemConfig = craftItemSlot.ItemConfig;
            _quantity = 1;
            Level = craftItemSlot.Level;

            _itemFrameUI.sprite = InventoryManager.Instance.ItemFrame[Level - 1];

            if (_itemConfig.IconWithLevel.Count > 0)
                _itemIconUI.sprite = _itemConfig.IconWithLevel[Level - 1];
            else
                _itemIconUI.sprite = _itemConfig.ItemIcon;
            
            _nullItemOb.SetActive(false);
            _hasItemOb.SetActive(true);
            HasPiece = true;
        }

        public void Reset() {
            _itemConfig = null;
            _quantity = 0;
            Level = 0;
            _itemFrameUI.sprite = null;
            _itemIconUI.sprite = null;
            _nullItemOb.SetActive(true);
            _hasItemOb.SetActive(false);
            HasPiece = false;
        }
    }

}
