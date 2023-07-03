using SH.PlayerData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SH.Account
{
    public class UISlotCharacterPanel : MonoBehaviour
    {
        [SerializeField] private UISlotCharacter _slotCharacter1 = default;

        [SerializeField] private UISlotCharacter _slotCharacter2 = default;

        [SerializeField] private UISlotCharacter _slotCharacter3 = default;

        [SerializeField] private GameObject _loginPanel = default;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _suiBalanceText;
        [SerializeField] private TextMeshProUGUI _suiAddressText;
        [SerializeField] private Button _coppyButton;
        [SerializeField] private Button _refreshButton;
        

        private void OnEnable()
        {
            _slotCharacter1.Setup();
            _slotCharacter2.Setup();
            _slotCharacter3.Setup();
            
            SetupInfo();
            
        }

        private void SetupInfo() {
            _nameText.text = PlayerDataManager.DisplayName;
            SetupBalance();
            string suiAddress = SuiWallet.GetActiveAddress();
            _suiAddressText.text = suiAddress.Substring(0, 11) + "..." + suiAddress.Substring(suiAddress.Length - 5);
            _coppyButton.onClick.AddListener(() =>
            {
                UniClipboard.SetText(SuiWallet.GetActiveAddress());
                UIManager.Instance.ShowAlert("Your address has been coppied!", AlertType.Normal);
            });

            _refreshButton.onClick.AddListener(() =>
            {
                SetupBalance();
            });
        }
        private async void SetupBalance() {
            _suiBalanceText.text = await SuiWalletManager.GetSuiWalletBalance();
        }

        public void OnLogoutClick()
        {
            _loginPanel.SetActive(true);
            SHLocalData.Instance.RemoveLoginData();
            SuiWallet.Logout();
            _loginPanel.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.SetActive(false);
        }
    }

}
