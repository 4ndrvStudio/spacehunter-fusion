using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH.UI
{
    public class UICraftingSuccess : MonoBehaviour
    {
    
        [SerializeField] private UICraftingPopup _craftingPopop;
        [SerializeField] private Button _closeButton;

        private void Awake() {
            _closeButton.onClick.AddListener(() => {
                _craftingPopop.CloseCrafting();
            });
        }
        
    }
}

