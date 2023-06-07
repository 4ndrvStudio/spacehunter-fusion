using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Dialogue;
namespace SH.Multiplayer
{
    public class UICraftingPopup : UIPopup
    {

        [SerializeField] private CraftingType _currentCraftType;
        [SerializeField] private GameObject _craftingChooseCraftPanel;
        [SerializeField] private GameObject _craftingPanel;
        [SerializeField] private GameObject _craftingSuccessPanel;

        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);
            _craftingChooseCraftPanel.SetActive(true);
        }

        //Button;
        public void TakeToCraft(CraftingType craftingType)
        {
            _craftingChooseCraftPanel.SetActive(false);
            _craftingSuccessPanel.SetActive(false);
            _craftingPanel.SetActive(true);
          //  DialogueManager.Instance.EnterDialogueMode();
        }

        public void TakeToSuccess()
        {

        }


    }

}
