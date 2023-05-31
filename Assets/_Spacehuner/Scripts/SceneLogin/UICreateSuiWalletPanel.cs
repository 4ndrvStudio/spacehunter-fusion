using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SH
{
    public class UICreateSuiWalletPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _slotCharacterPanel;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _coppyButton;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TMP_InputField _newWalletMnemonicsText;

        private string walletmnemo = string.Empty;
        // Start is called before the first frame update
        void Start()
        {
            _slotCharacterPanel.gameObject.SetActive(false);
            _newWalletMnemonicsText.text = "Create and save your 12 password phases";
            _newWalletMnemonicsText.interactable = false;
            _createButton.onClick.AddListener(() =>
            {
                CreateSuiWallet();
            });
            _continueButton.onClick.AddListener(() => {
                Continue();
            });

            _coppyButton.onClick.AddListener(() => {
                if(string.IsNullOrEmpty(walletmnemo) == false) {
                    UniClipboard.SetText(walletmnemo);
                }

             });
        }

        private void CreateSuiWallet()
        {
            walletmnemo = SuiWallet.CreateNewWallet();
            _newWalletMnemonicsText.gameObject.SetActive(true);
            _newWalletMnemonicsText.text = walletmnemo;
            _createButton.gameObject.SetActive(false);
            _continueButton.interactable = false;
            _continueButton.gameObject.SetActive(true);
            _newWalletMnemonicsText.interactable = true;
            _coppyButton.gameObject.SetActive(true);
            StartCoroutine(ActiveContinueButton());
        }

        private void Continue() {
            _newWalletMnemonicsText.gameObject.SetActive(false);
            _newWalletMnemonicsText.text = "Create and save your 12 password phases";
            _createButton.gameObject.SetActive(true);
            _continueButton.gameObject.SetActive(false);
            _slotCharacterPanel.SetActive(true);
            this.gameObject.SetActive(false);
             _coppyButton.gameObject.SetActive(false);
             _newWalletMnemonicsText.interactable = false;

        }

        IEnumerator ActiveContinueButton()
        {
            yield return new WaitForSeconds(3);
            _continueButton.interactable = true;

        }
    }

}
