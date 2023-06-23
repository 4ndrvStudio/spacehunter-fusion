using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SH.Define;
using SH.PlayerData;
using SH.AzureFunction;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using UnityEngine.Events;
using Suinet.Faucet;
using Newtonsoft.Json.Linq;

namespace SH.Account
{
    public class CharacterNFTModel
    {
        public string DataType { get; set; }
        public string Type { get; set; }
        public bool HasPublicTransfer { get; set; }
        public CharacterNFTFieldModel Fields { get; set; }

    }
    public class CharacterNFTFieldModel
    {

        public FieldId Id;
        [JsonProperty("collection_name")]
        public string Name;
        [JsonProperty("image_url")]
        public string ImageURL;
        [JsonProperty("collection_description")]
        public string Description;
        public CharacterNFTFieldsModel Fields;
        // public string Project_url;
    }
    public class CharacterNFTFieldsModel {
        [JsonProperty("image_url")]
        public string ImageURL;
        [JsonProperty("name")]
        public string Name;
    }
    public class FieldId
    {
        public string Id;
    }

    public class UICreateCharacterPanel : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _tmpSex = default;

        [SerializeField] private TextMeshProUGUI _tmpName = default;

        [SerializeField] private List<UICharacterSelect> _lstCharacterType = default;

        [SerializeField] private List<Image> _lstBtnRace = default;

        [SerializeField] private UICharacterSelect _characterSelected = default;

        [SerializeField] private int _slotIndex = default;

        [SerializeField] private GameObject _slotCharacterPanel = default;
        [Space]
        [Header("Sui Properties")]
        [SerializeField] private TextMeshProUGUI _suiBalanceText;

        [SerializeField] private TextMeshProUGUI _suiAddressText;
        [SerializeField] private Button _suiAddressCoppyButton;
        [SerializeField] private Button _refreshButton;

        public UnityAction MintNFTComplete { get; set; }
        public UnityAction ConfirmGasFeesAction { get; set; }

        private RpcResult<TransactionBlockBytes> _currentTx;

        private void Start()
        {
            SuiSetup();
        }

        private void SuiSetup()
        {
            SetupBalance();
            string suiAddress = SuiWallet.GetActiveAddress();
            _suiAddressText.text = suiAddress.Substring(0, 11) + "..." + suiAddress.Substring(suiAddress.Length - 5);

            _suiAddressCoppyButton.onClick.AddListener(() =>
            {
                UniClipboard.SetText(SuiWallet.GetActiveAddress());
                UIManager.Instance.ShowAlert("Your address has been coppied!", AlertType.Normal);
            });

            _refreshButton.onClick.AddListener(() =>
            {
                SetupBalance();
            });

        }

        private async void SetupBalance()
        {
            string balance = await SuiWalletManager.GetSuiWalletBalance();
            _suiBalanceText.text = $"{balance} SUI";
        }


        private void OnEnable()
        {
            OnAllClick();
            MintNFTComplete += OnSaveCharacter;
            ConfirmGasFeesAction += OnConfirmMintHunter;
        }
        private void OnDisable()
        {
            MintNFTComplete -= OnSaveCharacter;
            ConfirmGasFeesAction -= OnConfirmMintHunter;
        }

        public void Setup(int slotIndex)
        {
            _slotIndex = slotIndex;
        }

        public void OnBackClick()
        {
            gameObject.SetActive(false);
            _slotCharacterPanel.SetActive(true);
        }

        public async void OnFaucetClick()
        {
            var faucet = new UnityWebRequestFaucetClient();
            var success = await faucet.AirdropGasAsync(SuiWallet.GetActiveAddress());
            Dictionary<NotifyProperty, string> dictionary = new Dictionary<NotifyProperty, string>();

            if (success == true)
            {
                dictionary.Add(NotifyProperty.OkBtn, "true");
                dictionary.Add(NotifyProperty.Title, "Faucet Complete");
                dictionary.Add(NotifyProperty.Content, "You Faucet 1 SUI from SUI Testnet Chain.");
                dictionary.Add(NotifyProperty.State, "true");
            }
            else
            {
                dictionary.Add(NotifyProperty.OkBtn, "true");
                dictionary.Add(NotifyProperty.Title, "Faucet Fail");
                dictionary.Add(NotifyProperty.Content, "Too many request in your ip location! Please try again after few hours.");
                dictionary.Add(NotifyProperty.State, "false");
            }

            UIManager.Instance.ShowPopup(PopupName.Notification, dictionary);
        }

        public async void OnMintClick()
        {
            if (_characterSelected == null)
                return;

            //Mint Nft
            UIManager.Instance.ShowWaiting();

            var mintRpcResult = await SuiWalletManager.MintHunter();

            if (mintRpcResult.IsSuccess == true)
            {
                _currentTx = mintRpcResult;

                var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(mintRpcResult.Result.TxBytes.ToString());

                JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);

                JArray balanceChangesArray = (JArray)jsonObject["result"]["balanceChanges"];

                Debug.Log(balanceChangesArray[0]["amount"].ToString());

                SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();
                gasFeesModel.CanExcute = true;
                if (balanceChangesArray[0]["amount"].ToString() != null)
                    gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
                else
                    gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas, gasFeesModel, ConfirmGasFeesAction);
            }
            else
            {
                SuiNotificationModel modelPopup = new SuiNotificationModel();
                modelPopup.IsSuccess = false;
                modelPopup.ErrorDescription = mintRpcResult.ErrorMessage;
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiNotification, modelPopup);
            }



        }
        public async void OnConfirmMintHunter()
        {
            if (_currentTx == null) return;
            UIManager.Instance.ShowWaiting();

            var rpcResult = await SuiWalletManager.Execute(_currentTx);
            Debug.Log(rpcResult.RawRpcResponse);

            SuiNotificationModel modelPopup = new SuiNotificationModel();

            if (rpcResult.IsSuccess == false)
            {
                modelPopup.IsSuccess = false;
                modelPopup.ErrorDescription = rpcResult.ErrorMessage;
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiNotification, modelPopup);
            }
            else
            {
                var nftObject = await SuiApi.Client.GetObjectAsync(rpcResult.Result.Effects.SharedObjects[0].ObjectId, ObjectDataOptions.ShowAll());

                Debug.Log(nftObject.RawRpcResponse);
                //Debug.Log(rpcResult.Result.Effects.Created[0].Reference.ObjectId);

                //serialize object
                string nFTModel = JsonConvert.SerializeObject(nftObject.Result.Data.Content, Formatting.Indented);
                
                JObject nftJsonObject = JObject.Parse(nFTModel);

                //Cast to model
                modelPopup.IsSuccess = true;
                modelPopup.Name = nftJsonObject.SelectToken("fields.hunter_datas.fields.contents[0].fields.value.fields.name").ToString();
                modelPopup.Description = "";
                modelPopup.ImageURL = nftJsonObject.SelectToken("fields.hunter_datas.fields.contents[0].fields.value.fields.image_url").ToString();;
                modelPopup.ObjectId = rpcResult.Result.Effects.Created[0].Reference.ObjectId;
                InventoryManager.Instance.CurrentHunterAddressInUse  = rpcResult.Result.Effects.Created[0].Reference.ObjectId;
                UIManager.Instance.HideWaiting();
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiNotification, modelPopup, MintNFTComplete);

            }


        }

        public void SetCharacterSelect(UICharacterSelect selected)
        {
            _characterSelected = selected;
        }

        public void OnSaveCharacter()
        {
            PlayerDataManager.CallFunction<CreateCharacterRespone>(new CreateCharacterRequest((int)_characterSelected.CharacterType, _slotIndex), (resp) =>
            {
                if (string.IsNullOrEmpty(resp.Error))
                {
                    Debug.Log("Create character success!");

                    PlayerDataManager.CallFunction<GetUserDataRespone>(new GetUserDataRequest(), (resp) =>
                    {
                        if (string.IsNullOrEmpty(resp.Error))
                        {
                            PlayerDataManager.Instance.Setup(resp);
                            gameObject.SetActive(false);
                            _slotCharacterPanel.SetActive(true);

                            //UIManager.Instance.LoadScene(SceneName.SceneStation);
                        }
                        else
                        {
                            Debug.LogError(resp.Error);
                            UIManager.Instance.ShowAlert(resp.Error, AlertType.Error);
                        }
                    });
                }
                else
                {
                    Debug.LogError(resp.Error);
                    UIManager.Instance.ShowAlert(resp.Error, AlertType.Error);
                }
            });
        }


        public void OnAllClick()
        {
            _lstCharacterType.ForEach(item => item.gameObject.SetActive(true));
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstCharacterType[2].OnItemClick();
        }

        public void OnVasinClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.VasinMale || item.CharacterType == CharacterType.VasinFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });

            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[0].color = Color.red;
        }

        public void OnHumesClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.HumesMale || item.CharacterType == CharacterType.HumesFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[1].color = Color.yellow;
        }

        public void OnDisryClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.DisryMale || item.CharacterType == CharacterType.DisryFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[2].color = new Color(0, 0.9294118f, 0.9607843f, 1);
        }

        public void OnMutasClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.MutasMale || item.CharacterType == CharacterType.MutasFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[3].color = Color.green;
        }

        public void OnMabitClick()
        {
            _lstCharacterType.ForEach(item =>
            {
                if (item.CharacterType == CharacterType.MabitMale || item.CharacterType == CharacterType.MabitFemale)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            });
            _lstBtnRace.ForEach(item => item.color = Color.white);
            _lstBtnRace[4].color = Color.red;
        }

        public void SetSex(string sex)
        {
            _tmpSex.SetText(sex);
        }

        public void SetName(string name)
        {
            _tmpName.SetText(name);
        }
    }
}