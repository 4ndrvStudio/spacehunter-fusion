using SH.AzureFunction;
using SH.PlayerData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SH.Account
{
    public class UIResetPasswordPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _loginPanel = default;

        [SerializeField] private GameObject _sendCode = default;

        [SerializeField] private GameObject _verifyCode = default;

        [SerializeField] private GameObject _createPassword = default;

        [SerializeField] private GameObject _resetComplete = default;

        [SerializeField] private TextMeshProUGUI _tmpNotice = default;

        [Header("Send Code")]

        [SerializeField] private TMP_InputField _inputEmail = default;

        [Header("Verify Code")]

        [SerializeField] private TMP_InputField _inputCode = default;

        [SerializeField] private TextMeshProUGUI _tmpResendCode = default;

        [SerializeField] private Button _btnResend = default;

        private int _counterResend = 0;

        private int _timeResend = 30;

        [Header("Create password")]

        [SerializeField] private TMP_InputField _inputPassword = default;

        [SerializeField] private TMP_InputField _inputConfirmPassword = default;

        [SerializeField] private Image _imgPassword = default;

        [SerializeField] private Image _imgConfirmPassword = default;

        [SerializeField] private Sprite _sprShowPassword = default;

        [SerializeField] private Sprite _sprHidePassword = default;

        [Header("Reset complete")]

        [SerializeField] private Button _btnBackLogin = default;

        [Header("Tooltip")]

        [SerializeField] private GameObject _tooltip = default;

        [SerializeField] private Image _imgEnoughCharacters = default;

        [SerializeField] private Image _imgHasUpperCase = default;

        [SerializeField] private Image _imgHasNumber = default;

        [SerializeField] private Sprite _sprvalid = default;

        [SerializeField] private Sprite _sprInValid = default;

        private void OnEnable()
        {
            _sendCode.SetActive(true);
            _btnBackLogin.gameObject.SetActive(true);
            _verifyCode.SetActive(false);
            _createPassword.SetActive(false);
            _resetComplete.SetActive(false);
            _tmpNotice.SetText("");
        }

        #region Send Code

        public void OnBackLoginClick()
        {
            _loginPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnSendCodeClick()
        {
            Debug.Log("OnSend code Send");
            string email = _inputEmail.text;
            if (!RegisterAccountUtils.IsValidEmail(email))
            {
                _tmpNotice.SetText("Invalid email");
                return;
            }

            PlayFabManager.Instance.LoginWithCustomId(email, true, (result) =>
            {
                PlayerDataManager.CallFunction<AccountResetPasswordSendCodeRespone>(new AccountResetPasswordSendCodeRequest(email), (result) =>
                {
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        _tmpNotice.SetText(result.Error);
                        return;
                    }
                    _sendCode.SetActive(false);
                    _verifyCode.SetActive(true);
                    CountdownResendCode();
                    _tmpNotice.SetText("");
                });
            }, (error) =>
            {
                Debug.LogError(error.ErrorMessage);
                UIManager.Instance.ShowAlert(error.ErrorMessage, AlertType.Error);
            });
        }
        #endregion


        #region Verify Code
        public void OnResendCodeClick()
        {
            OnSendCodeClick();
        }

        public void OnResetPasswordClick()
        {
            string email = _inputEmail.text;
            string code = _inputCode.text;
            PlayerDataManager.CallFunction<AccountResetPasswordVerifyCodeRespone>(new AccountResetPasswordVerifyCodeRequest(email, code), (result) =>
            {
                if (!string.IsNullOrEmpty(result.Error))
                {
                    _tmpNotice.SetText(result.Error);
                    return;
                }
                _createPassword.SetActive(true);
                _verifyCode.SetActive(false);
                _tmpNotice.SetText("");
                CountdownResendCode();
            });
        }

        private async void CountdownResendCode()
        {
            while (_counterResend < _timeResend)
            {
                await Task.Delay(1000);
                _counterResend++;
                _tmpResendCode.SetText($"{_timeResend - _counterResend}s");
                _btnResend.interactable = false;
            }
            _counterResend = 0;
            _tmpResendCode.SetText("Send Code");
            _btnResend.interactable = true;
        }
        #endregion

        #region Create Password

        public void OnCreateNewPasswordClick()
        {
            string email = _inputEmail.text;
            string password = _inputPassword.text;
            string confirmPassword = _inputConfirmPassword.text;

            if(password != confirmPassword)
            {
                _tmpNotice.SetText("Password not match!");
                return;
            }

            var validPassword = RegisterAccountUtils.IsValidPassword(password);
            if (!validPassword.isEnoughLength || !validPassword.hasUpperCase || !validPassword.hasNumber)
            {
                OnTooltipPasswordShow();
                return;
            }

            PlayerDataManager.CallFunction<AccountResetPasswordUpdateRespone>(new AccountResetPasswordUpdateRequest(email, password), (result) =>
            {
                if(!string.IsNullOrEmpty(result.Error))
                {
                    _tmpNotice.SetText(result.Error);
                    return;
                }
                _resetComplete.SetActive(true);
                _createPassword.SetActive(false);
                _btnBackLogin.gameObject.SetActive(false);
                _tmpNotice.SetText("");
            });

        }

        public void OnShowPasswordClick()
        {
            var contentType = _inputPassword.contentType;
            if (contentType == TMP_InputField.ContentType.Standard)
            {
                _inputPassword.contentType = TMP_InputField.ContentType.Password;
                _imgPassword.sprite = _sprHidePassword;
                _inputPassword.ForceLabelUpdate();
            }
            else if (contentType == TMP_InputField.ContentType.Password)
            {
                _inputPassword.contentType = TMP_InputField.ContentType.Standard;
                _imgPassword.sprite = _sprShowPassword;
                _inputPassword.ForceLabelUpdate();
            }
        }

        public void OnShowConfirmPasswordClick()
        {
            var contentType = _inputConfirmPassword.contentType;
            if (contentType == TMP_InputField.ContentType.Standard)
            {
                _inputConfirmPassword.contentType = TMP_InputField.ContentType.Password;
                _imgConfirmPassword.sprite = _sprHidePassword;
                _inputConfirmPassword.ForceLabelUpdate();

            }
            else if (contentType == TMP_InputField.ContentType.Password)
            {
                _inputConfirmPassword.contentType = TMP_InputField.ContentType.Standard;
                _imgConfirmPassword.sprite = _sprShowPassword;
                _inputConfirmPassword.ForceLabelUpdate();
            }
        }

        public void OnTooltipPasswordShow()
        {
            string password = _inputPassword.text;
            var validPassword = RegisterAccountUtils.IsValidPassword(password);
            _imgEnoughCharacters.sprite = validPassword.isEnoughLength ? _sprvalid : _sprInValid;
            _imgHasUpperCase.sprite = validPassword.hasUpperCase ? _sprvalid : _sprInValid;
            _imgHasNumber.sprite = validPassword.hasNumber ? _sprvalid : _sprInValid;
            _tooltip.SetActive(true);
        }

        public void OnTooltipPasswordHide() => _tooltip.SetActive(false);
        #endregion
    }
}
