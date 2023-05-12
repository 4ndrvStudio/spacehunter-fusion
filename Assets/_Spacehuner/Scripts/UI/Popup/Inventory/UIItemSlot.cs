using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH
{
    public class UIItemSlot : MonoBehaviour
    {
        //PUBLIC MEMEBER
        [HideInInspector] public int Level;
        public Sprite ItemIcon => _itemIconUI.sprite;

        //PRIVATE MEMEBER
        [SerializeField] private Button _itemButton;
        [SerializeField] private Image _itemFrameUI;
        [SerializeField] private Image _itemIconUI;

        private ItemConfig _itemConfig;
        public ItemConfig ItemConfig => _itemConfig;

        [SerializeField] private int _quantity = 1;

        void Start() {
            _itemButton.onClick.AddListener(() => {
                UIInventoryPopup.Instance.SetMainItem(this);
            });
        }

        public void StackItem() => _quantity++;

        public void Setup(int level , ItemConfig itemConfig) {
            _itemConfig = itemConfig;
            _quantity = 1;
            Level = level;
            //setup for mineral "special case";
            // if(itemConfig.ItemId == "mineral") {
                 _itemFrameUI.sprite = InventoryManager.Instance.MineralFrame[Level-1];
                 _itemIconUI.sprite = _itemConfig.IconWithLevel[Level-1];
            //} 
        }


        



    }


}
