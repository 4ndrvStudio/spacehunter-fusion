using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SH.PlayerData;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SH.UI
{
    public class UIMenuPanel_Profile : UIMenuPanel
    {
        [SerializeField] private UIMenuGamePopup _uiMenuGamePopup;
        [SerializeField] private GameObject _loaderIcon;
        [SerializeField] private GameObject Content;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _addressText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private Image _expBar;
        
        [SerializeField] private Button _copyButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _hunterButton;
        
        void Start() {

            //button Coppy
            _copyButton.onClick.AddListener(() => {
                UniClipboard.SetText (SuiWallet.GetActiveAddress());
                UIManager.Instance.ShowAlert("Your Sui address has been coppied!",AlertType.Normal);
            });

            _inventoryButton.onClick.AddListener(() => {
                UIManager.Instance.ShowPopup(PopupName.Inventory);
                _uiMenuGamePopup.Hide();
            });

            _hunterButton.onClick.AddListener(() => {
                UIManager.Instance.ShowPopup(PopupName.CharacterInfo);
                _uiMenuGamePopup.Hide();
            });
        }
 
        public override void Display()
        {
            SetUpInfo();
        }

        public override void Hide()
        {
            Content.SetActive(false);
        }


        public async void SetUpInfo()
        {
            _loaderIcon.SetActive(true);

            _nameText.text = PlayerDataManager.DisplayName;
            string address = SuiWallet.GetActiveAddress();
            _addressText.text = address.Substring(0, 7) + "..." +
                                address.Substring(address.Length - 5);

            var rpcResult = await SuiWalletManager.GetHunterInfo(InventoryManager.Instance.CurrentHunterAddressInUse);
            
            if(_uiMenuGamePopup.CurrentTab != UIMenuTabName.Profile) {
                _loaderIcon.SetActive(false);
                return;
            };
                       
            if(rpcResult.IsSuccess == true) {
                 string nameNft = rpcResult.Result.Data.Display.Data["name"];
                int indexSharp = nameNft.IndexOf("#");

                string jsonNft = JsonConvert.SerializeObject(rpcResult.Result.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);

                string level = nftJsonObject.SelectToken("fields.level").ToString();
                string exp = nftJsonObject.SelectToken("fields.exp").ToString();
                _levelText.text = "Lv " + level;
                float currentExp = float.Parse(exp) < 1000f ? float.Parse(exp) : (float.Parse(exp) - float.Parse(level) * 1000f);
                _expBar.fillAmount = currentExp / 1000f;
                _expText.text = $"{currentExp}/1000";

            } else {
                UIManager.Instance.ShowAlert("Some things wrong in your Hunter NFT@!", AlertType.Warning);
            }

            _loaderIcon.SetActive(false);

            Content.SetActive(true);
        }

    }

}
