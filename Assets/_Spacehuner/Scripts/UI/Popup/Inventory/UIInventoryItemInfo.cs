using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SH.UI
{
    public class UIInventoryItemInfo : MonoBehaviour
    {
        [SerializeField] private Image _displayImage;
        [SerializeField] private TextMeshProUGUI _itemNameText;

        [SerializeField] private Button _useButton;
        [SerializeField] private TextMeshProUGUI _useButtonText;

        [SerializeField] private Color _useTextActiveColor;
        [SerializeField] private Color _useTextDeactiveColor;

        void Start() {
            _useButton.onClick.AddListener(()=> UIInventoryPopup.Instance.UseItem());
        }

        public void DisplayItemInfo(UIItemSlot uiInventoryItem) {
            _displayImage.gameObject.SetActive(false);
            _displayImage.sprite = uiInventoryItem.ItemIcon;
            _displayImage.gameObject.SetActive(true);
            
            _useButton.interactable = uiInventoryItem.ItemConfig.CanUse;
            _useButtonText.color = uiInventoryItem.ItemConfig.CanUse ? _useTextActiveColor : _useTextDeactiveColor;

        }
        public void ClearDisplay() {
            _displayImage.sprite = null;
            _displayImage.gameObject.SetActive(false);
            _useButton.interactable = false;
            _useButtonText.color = _useTextDeactiveColor;

        }
    }

}
