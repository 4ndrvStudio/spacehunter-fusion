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

namespace SH.Account
{
    public class UILoginPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _slotCharacterPanel = default;

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
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && _autoLogin)
            {
                Login(email, password);
            }
        }

        private void Login(string email, string password)
        {
            PlayFabManager.Instance.LoginWithCustomId(email, true, (result) =>
            {
                PlayerDataManager.CallFunction<AccountLoginRespone>(new AccountLoginRequest(email, password), (result) =>
                {
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        _tmpNotice.SetText(result.Error);
                        Debug.LogError(result.Error);
                        return;
                    }

                    Debug.Log("Login success!");

                    PlayFabManager.Instance.CheckAccountInfo((result) =>
                    {
                        if(string.IsNullOrEmpty(result.AccountInfo.TitleInfo.DisplayName))
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
                                    if (_toggleRememberAccount.isOn)
                                    {
                                        SHLocalData.Instance.Data.Email = email;
                                        SHLocalData.Instance.Data.Password = password;
                                    }
                                    gameObject.SetActive(false);
                                    _slotCharacterPanel.SetActive(true);
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
                        UIManager.Instance.ShowAlert(error.ErrorMessage, AlertType.Error);
                    });
                });
            }, (error) =>
            {
                Debug.LogError(error.ErrorMessage);
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

            if(string.IsNullOrEmpty(password))
            {
                _tmpNotice.SetText("Password invalid");
                return;
            }

            if(!RegisterAccountUtils.IsValidEmail(email))
            {
                _tmpNotice.SetText("Email invalid");
                return;
            }

            var validPassword = RegisterAccountUtils.IsValidPassword(password);
            if(!validPassword.isEnoughLength || !validPassword.hasNumber || !validPassword.hasUpperCase)
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
