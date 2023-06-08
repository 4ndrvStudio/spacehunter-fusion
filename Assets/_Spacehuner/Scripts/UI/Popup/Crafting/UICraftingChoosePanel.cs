using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SH.Multiplayer;

namespace SH.UI
{
    public class UICraftingChoosePanel : MonoBehaviour
    {
        [SerializeField] private UICraftingPopup _uiCraftingPopup;
        [SerializeField] private List<UICraftingChooseSlot> _craftingItemSlot;

        void Start() {
            _craftingItemSlot.ForEach(item => {
                item.CraftButton.onClick.AddListener(() => _uiCraftingPopup.ProcessNextStep(ECraftingState.Crafting,item.CraftingType));
            });
        }



    }

}
