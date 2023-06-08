using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Dialogue;

namespace SH.UI
{
    public enum ECraftingState
    {
        ChooseItem,
        Crafting,
        Complete
    }
    public class UICraftingPopup : UIPopup
    {
        [SerializeField] private ECraftingType _currentCraftType;
        [SerializeField] private GameObject _craftingChooseCraftPanel;
        [SerializeField] private GameObject _craftingPanel;
        [SerializeField] private GameObject _craftingSuccessPanel;

        public override void Show(object customProperties = null)
        {
            base.Show(customProperties);
            DialogueManager.Instance.ExitDialogueMode(isCanCountinue: true);
            DialogueManager.Instance.RemoveAllStartChatBtn();
            _craftingChooseCraftPanel.SetActive(true);
        }

        public void ProcessNextStep(ECraftingState craftingState, ECraftingType craftingType)
        {
            Debug.Log("next step " + craftingState.ToString());
            _craftingChooseCraftPanel.SetActive(false);
            _craftingSuccessPanel.SetActive(false);
            _craftingPanel.SetActive(false);
            _currentCraftType = craftingType;

            switch (craftingState)
            {
                case ECraftingState.ChooseItem:
                    _craftingChooseCraftPanel.SetActive(true);
                    break;
                case ECraftingState.Crafting:
                    _craftingPanel.SetActive(true);
                    break;
                case ECraftingState.Complete:
                    _craftingSuccessPanel.SetActive(true);
                    break;

            }
        }

        //Button;
        public void TakeToCraft(ECraftingType craftingType)
        {
            _craftingChooseCraftPanel.SetActive(false);
            _craftingSuccessPanel.SetActive(false);
            _craftingPanel.SetActive(true);
            //  DialogueManager.Instance.EnterDialogueMode();
        }

        public void CloseCrafting()
        {
            _craftingChooseCraftPanel.SetActive(false);
            _craftingSuccessPanel.SetActive(false);
            _craftingPanel.SetActive(false);
            DialogueManager.Instance.EnterDialogueMode();
            Hide();
        }


    }

}
