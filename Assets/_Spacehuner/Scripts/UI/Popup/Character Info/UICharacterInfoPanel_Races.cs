using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Suinet.Rpc.Types;
namespace SH.UI
{
    public class UICharacterInfoPanel_Races : UICharacterInfoPanel
    {
        [SerializeField] private GameObject ContentOb;
        [SerializeField] private UICharacterInfoPopup _uiCharacterInfoPopup;

        [SerializeField] private UICharacterRenderTexture _uiCharacterRenderTexture;
        [Header("Character Info")]
        [SerializeField] private TextMeshProUGUI _hunterNameText;
        [SerializeField] private TextMeshProUGUI _hunterIdText;
        [SerializeField] private TextMeshProUGUI _defendText;
        [SerializeField] private TextMeshProUGUI _attackText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _expText;
        [SerializeField] private Image _expBar;


        [SerializeField] private GameObject _loaderIcon;
        [SerializeField] private Button _unEquipButton;

        // Start is called before the first frame update
        void Start()
        {
            Display();
        }


        public override void Display()
        {
            GetHunterData();


        }
        public override void Hide()
        {
            ContentOb.SetActive(false);

        }

        private async void GetHunterData()
        {

            _loaderIcon.SetActive(true);
            _uiCharacterInfoPopup.GetEquipedItem();
            var weaponResult = await SuiWalletManager.GetHunterInfo(InventoryManager.Instance.CurrentHunterAddressInUse);

            if (weaponResult.IsSuccess == true)
            {
                string nameNft = weaponResult.Result.Data.Display.Data["name"];
                int indexSharp = nameNft.IndexOf("#");

                _hunterNameText.text = nameNft.Substring(0, nameNft.Length - (nameNft.Length - indexSharp));

                _hunterIdText.text = nameNft.Substring(indexSharp);

                string jsonNft = JsonConvert.SerializeObject(weaponResult.Result.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);

                _hpText.text = nftJsonObject.SelectToken("fields.hp").ToString();
                _attackText.text = nftJsonObject.SelectToken("fields.attack").ToString();
                _defendText.text = nftJsonObject.SelectToken("fields.defend").ToString();
                _speedText.text = nftJsonObject.SelectToken("fields.speed").ToString();
                string level = nftJsonObject.SelectToken("fields.level").ToString();
                string exp = nftJsonObject.SelectToken("fields.exp").ToString();
                _levelText.text = "Lv " + level;
                float currentExp = float.Parse(exp) < 1000f ? float.Parse(exp) : (float.Parse(exp) - float.Parse(level) * 1000f);
                _expBar.fillAmount = currentExp / 1000f;
                _expText.text = $"{currentExp}/1000";
                _descriptionText.text = weaponResult.Result.Data.Display.Data["description"];
            }
            _loaderIcon.SetActive(false);

            if (_uiCharacterInfoPopup.CurrentTab != UICharacterTabName.Races)
                return;

            ContentOb.SetActive(true);

            if (_uiCharacterInfoPopup.HasWeapon == true)
            {
                _uiCharacterRenderTexture.SetToIdle(false);
            }
            else
            {
                _uiCharacterRenderTexture.SetToIdle(true);
            }
        }
    }

}
