using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    using UnityEngine.UI;
    
    public class UISuiWalletPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject _selectMethodPanel;
        [SerializeField] private GameObject _createSuiWalletPanel;
        [SerializeField] private GameObject _importExistingWalletPanel;
        [SerializeField] private GameObject _notifyCompletePanel;
        
        [Space] 
        [Header("Button")]
        [SerializeField] private Button _createNewWalletButton;
        [SerializeField] private Button _importWalletButton;
        
        // Start is called before the first frame update
        void Start()
        {
            _selectMethodPanel.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

