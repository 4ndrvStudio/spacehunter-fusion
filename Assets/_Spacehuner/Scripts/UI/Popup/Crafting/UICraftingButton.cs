using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SH.UI
{
    public enum ECraftingButtonState
    {
        Disable,
        Enable,
        Processing
    }

    public class UICraftingButton : MonoBehaviour
    {
        [SerializeField] private UICraftingPanel _craftingPanel;
        [SerializeField] private Button _craftingButton;
        [SerializeField] private GameObject _activeImage;
        [SerializeField] private GameObject _disableImage;
        [SerializeField] private GameObject _processingImage;
        [SerializeField] private bool _canCraft;

        void Start()
        {
            _craftingButton.onClick.AddListener(() =>
            {
                if (_canCraft) _craftingPanel.CraftItem();
            });
        }

        public void ProcessState(ECraftingButtonState craftingButtonstate)
        {     
            _activeImage.SetActive(false);
            _processingImage.SetActive(false);
            _canCraft = false;

            switch (craftingButtonstate)
            {
                case ECraftingButtonState.Enable:
                    _activeImage.SetActive(true);
                    _canCraft = true;
                    break;
                case ECraftingButtonState.Processing:
                    _processingImage.SetActive(true);
                    break;
            }
        }
    }

}
