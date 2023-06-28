using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SH.UI
{
    using UnityEngine.UI;
    using TMPro;
    public enum ESuiWalletState
    {
        Select,
        Create,
        Import,
        Confirm
    }

    public class UISuiWalletPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _slotCharacterPanel;
        [Header("Sui Method")]
        [SerializeField] private GameObject _selectMethodPanel;
        [SerializeField] private GameObject _createSuiWalletPanel;
        [SerializeField] private GameObject _importExistingWalletPanel;
        [SerializeField] private GameObject _notifyCompletePanel;

        [Space]
        [Header("Create Wallet Panel")]
        [SerializeField] private Button _createWalletMethodButton;
        [SerializeField] private Button _createButton;
        [SerializeField] private GameObject _createDisable;
        [SerializeField] private GameObject _createActive;
        [SerializeField] private TMP_InputField _mnenoInput;
        [SerializeField] private UICheckbox _checkbox;
        [SerializeField] private List<Button> _backButtons = new List<Button>();

        [Space]
        [Header("Import Wallet Panel")]
        [SerializeField] private Button _importWalletMethodButton;
        [SerializeField] private Button _importButton;
        [SerializeField] private UI12InputPhases _12InputPhases;

        [Space]
        [Header("Notify Panel")]
        [SerializeField] private Button _confirmButton;

        public void OnEnable()
        {
            UICheckbox.StateChange += CheckBoxChange;
        }
        public void OnDisable()
        {
            UICheckbox.StateChange -= CheckBoxChange;
        }

        private void CheckBoxChange(bool isActive)
        {

            if (isActive == true)
            {
                _createActive.SetActive(true);
                _createDisable.SetActive(false);
            }
            else
            {
                _createActive.SetActive(false);
                _createDisable.SetActive(true);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            _selectMethodPanel.gameObject.SetActive(true);

            _createWalletMethodButton.onClick.AddListener(() => ProcessToNextStep(ESuiWalletState.Create));
            _importWalletMethodButton.onClick.AddListener(() => ProcessToNextStep(ESuiWalletState.Import));

            _backButtons.ForEach(button =>
            {
                button.onClick.AddListener(() => ProcessToNextStep(ESuiWalletState.Select));
            });

            _importButton.onClick.AddListener(() => ImportWallet());
            _createButton.onClick.AddListener(() =>
            {
                if (_checkbox.IsActive == true)
                    ProcessToNextStep(ESuiWalletState.Confirm);

            });

            _confirmButton.onClick.AddListener(() =>
            {
                Continue();
            });

            var currentKeyPair =  SuiWallet.GetActiveKeyPair();

            if(currentKeyPair != null) {
                Continue();
            }
        }

        private void ProcessToNextStep(ESuiWalletState suiWalletState)
        {

            _selectMethodPanel.SetActive(false);
            _createSuiWalletPanel.SetActive(false);
            _importExistingWalletPanel.SetActive(false);
            _notifyCompletePanel.SetActive(false);

            switch (suiWalletState)
            {
                case ESuiWalletState.Create:
                    CreateWallet();
                    _createSuiWalletPanel.SetActive(true);
                    break;
                case ESuiWalletState.Import:
                    _importExistingWalletPanel.SetActive(true);
                    break;
                case ESuiWalletState.Confirm:
                    _notifyCompletePanel.SetActive(true);
                    break;
                case ESuiWalletState.Select:
                    _selectMethodPanel.SetActive(true);
                    break;
            }
        }

        private void CreateWallet()
        {
            string mnemo = SuiWallet.CreateNewWallet();

            _mnenoInput.text = mnemo;

        }

        private void ImportWallet()
        {
            if (_12InputPhases.Get12Phases().Trim().Split(" ").Length != 12)
            {
                UIManager.Instance.ShowAlert("Your mnenonics not enough 12 words!", AlertType.Warning);
                return;
            }

            SuiWallet.RestoreWalletFromMnemonics(_12InputPhases.Get12Phases());

            //countinue
            ProcessToNextStep(ESuiWalletState.Confirm);

            


        }
        private async void Continue()
        {
            UIManager.Instance.ShowWaiting();
            bool hasHunter = false;

            var allNft = await SuiWalletManager.GetAllNFT();
            
            UIManager.Instance.HideWaiting();

            allNft.Result.Data.ForEach(nft =>
            {
                string jsonNft = JsonConvert.SerializeObject(nft.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);

                if (nftJsonObject.SelectToken("type").ToString().Contains($"{InventoryManager.Instance.SuiPackageId}::hunter::Hunter"))
                {
                    hasHunter = true;
                }

                PlayerData.PlayerDataManager.Character.Data.Characters[0].CharacterType = Define.CharacterType.MutasFemale;

            });
            
            if(hasHunter == false) {
                PlayerData.PlayerDataManager.Character.Data.Characters[0].CharacterType = 0;
            }

            this.gameObject.SetActive(false);
            _slotCharacterPanel.SetActive(true);
            InventoryManager.Instance.GetInventoryData();
        }



    }
}

