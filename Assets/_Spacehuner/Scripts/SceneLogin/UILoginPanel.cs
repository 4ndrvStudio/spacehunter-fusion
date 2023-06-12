using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SH.PlayerData;
using SH.AzureFunction;
using SH.Define;
using System.Threading.Tasks;
using System.Linq;
using SH.Models.Azure;


namespace SH.Account
{
    public class UILoginPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loginCanvas;
        [SerializeField] private GameObject _slotCharacterPanel = default;
        [SerializeField] private GameObject _suiWalletPanel = default;

        [Header("Login")]

        [SerializeField] private bool _autoLogin = default;

        [SerializeField] private GameObject _signUpPanel = default;

        [SerializeField] private GameObject _resetPasswordPanel = default;

        [SerializeField] private TMP_InputField _inputEmail = default;

        [SerializeField] private TMP_InputField _inputPassword = default;

        [SerializeField] private Toggle _toggleRememberAccount = default;

        [SerializeField] private TextMeshProUGUI _tmpNotice = default;

        [SerializeField] private Image _imgShowPassword = default;

        [SerializeField] private Sprite _sprShowPassword = default;

        [SerializeField] private Sprite _sprHidePassword = default;

        [SerializeField] private GameObject _createPlayerNamePanel = default;

        private void OnEnable()
        {
            _tmpNotice.SetText("");
            _inputPassword.text = string.Empty;
            _inputEmail.text = string.Empty;
            _toggleRememberAccount.isOn = false;

        }

        private void Start()
        {
            string email = SHLocalData.Instance.Data.Email;
            string password = SHLocalData.Instance.Data.Password;
            Debug.Log(email + " " + password);

            _autoLogin = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password);

            if (_autoLogin == true)
            {
                Login(email, password);
            }
            else
            {
                _loginCanvas.alpha = 1;
            }

        }

        private void Login(string email, string password)
        {

            string lowerCaseEmail = email.ToLower();
            PlayFabManager.Instance.LoginWithCustomId(lowerCaseEmail, true, (result) =>
            {
                PlayerDataManager.CallFunction<AccountLoginRespone>(new AccountLoginRequest(lowerCaseEmail, password), async (result) =>
                {
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        _tmpNotice.SetText(result.Error);
                        Debug.LogError(result.Error);
                        if (_autoLogin) _loginCanvas.alpha = 1;
                        return;
                    }


                    Debug.Log("Login success!");

                    //check update
                    bool requireUpdate = await GameManager.Instance.CheckUpdate();
                    if (requireUpdate)
                    {
                        Debug.Log("Show update popup");
                        UIManager.Instance.ShowPopup(PopupName.UpdateNotification);
                    }

                    // PlayFabManager.Instance.GetInventoryData(res =>
                    // {
                    //     Debug.Log(res.Inventory.Count);
                    // },
                    // err =>
                    // {
                    //     Debug.Log(err);
                    // });


                    PlayFabManager.Instance.CheckAccountInfo((result) =>
                    {
                        if (string.IsNullOrEmpty(result.AccountInfo.TitleInfo.DisplayName))
                        {
                            gameObject.SetActive(false);
                            _createPlayerNamePanel.SetActive(true);
                        }
                        else
                        {
                            Debug.Log("Display name: " + result.AccountInfo.TitleInfo.DisplayName);
                            PlayerDataManager.CallFunction<GetUserDataRespone>(new GetUserDataRequest(), (resp) =>
                            {
                                if (string.IsNullOrEmpty(resp.Error))
                                {
                                    Debug.Log("Get data user success!");

                                    PlayerDataManager.Instance.Setup(resp);

                                    Debug.Log(resp.PlayFabId);

                                    if (_toggleRememberAccount.isOn)
                                    {
                                        SHLocalData.Instance.Data.Email = email;
                                        SHLocalData.Instance.Data.Password = password;
                                        SHLocalData.Instance.Save();
                                    }
                                    gameObject.SetActive(false);
                                    _suiWalletPanel.SetActive(true);
                                   //_slotCharacterPanel.SetActive(true);
                                }
                                else
                                {
                                    Debug.LogError(resp.Error);
                                    _tmpNotice.SetText(resp.Error);
                                }
                            });
                        }
                    }, (error) =>
                    {
                        // //display login canvas when auto login
                        if (_autoLogin) _loginCanvas.alpha = 1;

                        UIManager.Instance.ShowAlert(error.ErrorMessage, AlertType.Error);
                    });
                });
            }, (error) =>
            {
                Debug.LogError(error.ErrorMessage);
                _loginCanvas.alpha = 1;
                Debug.Log(_loginCanvas.alpha);
                UIManager.Instance.ShowAlert(error.ErrorMessage, AlertType.Error);
            });
        }

        #region Login
        public void OnLoginClick()
        {
            string email = _inputEmail.text;
            string password = _inputPassword.text;

            if (string.IsNullOrEmpty(email))
            {
                _tmpNotice.SetText("Email invalid");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                _tmpNotice.SetText("Password invalid");
                return;
            }

            if (!RegisterAccountUtils.IsValidEmail(email))
            {
                _tmpNotice.SetText("Email invalid");
                return;
            }

            var validPassword = RegisterAccountUtils.IsValidPassword(password);
            if (!validPassword.isEnoughLength || !validPassword.hasNumber || !validPassword.hasUpperCase)
            {
                _tmpNotice.SetText("Password invalid");
                return;
            }

            Login(email, password);
        }

        public void OnRegisterClick()
        {
            gameObject.SetActive(false);
            _signUpPanel.SetActive(true);
        }

        public void OnResetPasswordClick()
        {
            _resetPasswordPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnShowPasswordClick()
        {
            var contentType = _inputPassword.contentType;
            if (contentType == TMP_InputField.ContentType.Standard)
            {
                _inputPassword.contentType = TMP_InputField.ContentType.Password;
                _imgShowPassword.sprite = _sprHidePassword;
                _inputPassword.ForceLabelUpdate();
            }
            else if (contentType == TMP_InputField.ContentType.Password)
            {
                _inputPassword.contentType = TMP_InputField.ContentType.Standard;
                _imgShowPassword.sprite = _sprShowPassword;
                _inputPassword.ForceLabelUpdate();
            }
        }
        #endregion


    }
}
