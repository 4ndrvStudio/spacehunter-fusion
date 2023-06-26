using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH.UI
{
    public class UICraftingSuccess : MonoBehaviour
    {
        [SerializeField] private UICraftingPopup _craftingPopop;
        [SerializeField] private Sprite _glassesSprite;
        [SerializeField] private Sprite _swordSprite;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _itemIcon;

        private void Awake() {
            _closeButton.onClick.AddListener(() => {
                _craftingPopop.CloseCrafting();
            });
        }

        public void SetupNotify(ECraftingType craftingType) {
            switch(craftingType) {
                case ECraftingType.Weapon: 
                    _itemIcon.sprite = _swordSprite;
                    break;
                case ECraftingType.Glass :
                    _itemIcon.sprite = _glassesSprite;
                    break;
            }
        }
        
    }
}

