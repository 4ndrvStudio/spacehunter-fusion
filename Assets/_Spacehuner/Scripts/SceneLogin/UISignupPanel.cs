using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using SH.PlayerData;
using SH.AzureFunction;
using System.Threading.Tasks;
using SH.Define;

namespace SH.Account
{
    public class UISignupPanel : MonoBehaviour
    {
        [Header("Sign up")]

        [SerializeField] private TMP_InputField _inputEmail = default;

        [SerializeField] private TMP_InputField _inputPassword = default;

        [SerializeField] private TMP_InputField _inputConfirmPassword = default;

        [SerializeField] private Toggle _togglePrivacy = default;

        [SerializeField] private TextMeshProUGUI _tmpNotice = default;

        [SerializeField] private GameObject _signUp = default;

        [SerializeField] private Image _imgShowPassword = default;

        [SerializeField] private Image _imgShowConfirmPassword = default;

        [SerializeField] private Sprite _sprShowPassword = default;

        [SerializeField] private Sprite _sprHidePassword = default;

        [Header("Tooltip")]

        [SerializeField] private GameObject _tooltip = default;

        [SerializeField] private Image _imgEnoughCharacters = default;

        [SerializeField] private Image _imgHasUpperCase = default;

        [SerializeField] private Image _imgHasNumber = default;

        [SerializeField] private Sprite _sprvalid = default;

        [SerializeField] private Sprite _sprInValid = default;

        [Header("Verify Code")]

        [SerializeField] private GameObject _verifyCode = default;

        [SerializeField] private TMP_InputField _inputCode = default;

        [SerializeField] private TextMeshProUGUI _tmpNoticeVerify = default;

        [SerializeField] private TextMeshProUGUI _tmpSendCode = default;

        [SerializeField] private Button _btnResend = default;

        private int _timeResend = 30; // seconds

        private int _counterResend = 0;

        [Header("Account created")]

        [SerializeField] private GameObject _accountCreated = default;

        [SerializeField] private GameObject _loginPanel = default;


        private void OnEnable()
        {
            _signUp.SetActive(true);
            _verifyCode.SetActive(false);
            _accountCreated.SetActive(false);
            _tmpNotice.SetText("");
            _tmpNoticeVerify.SetText("");
        }

        #region Sign up
        public void OnNextClick()
        {
            string email = _inputEmail.text;
            string password = _inputPassword.text;
            string confirmPassword = _inputConfirmPassword.text;

            _tmpNotice.text = string.Empty;

            if (!RegisterAccountUtils.IsValidEmail(email))
            {
                _tmpNotice.SetText("* Please enter a correct email address");
                return;
            }

            var validPassword = RegisterAccountUtils.IsValidPassword(password);
            if (!validPassword.isEnoughLength || !validPassword.hasUpperCase || !validPassword.hasNumber)
            {
                OnTooltipPasswordShow();
                return;
            }

            if (!RegisterAccountUtils.IsMatchPassword(password, confirmPassword))
            {
                _tmpNotice.SetText("* Password not match, make sure your turn off caplock");
                return;
            }

            if (!_togglePrivacy.isOn)
            {
                _tmpNotice.SetText("* Please agree Terms of Service and privacy policy");
                return;
            }

            PlayFabManager.Instance.LoginWithCustomId(email, true, (result) =>
            {
                PlayerDataManager.CallFunction<AccountRegisterInfoRespone>(new AccountRegisterInfoRequest(email, password), (result) =>
                {
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        _tmpNotice.SetText(result.Error);
                        return;
                    }
                    _signUp.gameObject.SetActive(false);
                    _verifyCode.SetActive(true);
                    CountdownResendCode();
                });
            }, (error) =>
            {
                Debug.LogError(error.ErrorMessage);
                UIManager.Instance.ShowAlert(error.ErrorMessage, AlertType.Error);
            });
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

        public void OnShowPasswordConfirmClick()
        {
            var contentType = _inputConfirmPassword.contentType;
            if (contentType == TMP_InputField.ContentType.Standard)
            {
                _inputConfirmPassword.contentType = TMP_InputField.ContentType.Password;
                _imgShowConfirmPassword.sprite = _sprHidePassword;
                _inputConfirmPassword.ForceLabelUpdate();

            }
            else if (contentType == TMP_InputField.ContentType.Password)
            {
                _inputConfirmPassword.contentType = TMP_InputField.ContentType.Standard;
                _imgShowConfirmPassword.sprite = _sprShowPassword;
                _inputConfirmPassword.ForceLabelUpdate();
            }
        }

        public void OnTooltipPasswordHide() => _tooltip.SetActive(false);

        #endregion

        #region Verify Code
        public void OnSendCodeClick()
        {
            string email = _inputEmail.text;
            string password = _inputPassword.text;
            PlayerDataManager.CallFunction<AccountRegisterSendCodeRespone>(new AccountRegisterSendCodeRequest(email), (resp) =>
            {
                if (!string.IsNullOrEmpty(resp.Error))
                {
                    Debug.LogError(resp.Error);
                    _tmpNoticeVerify.SetText(resp.Error);
                    return;
                }
                else
                {
                    Debug.Log("Send code success!");
                    _signUp.SetActive(false);
                    _verifyCode.SetActive(true);
                    CountdownResendCode();
                }
            });
        }

        public void OnVerifyClick()
        {
            string email = _inputEmail.text;
            string password = _inputPassword.text;
            string code = _inputCode.text;

            if (code.Length < 6)
            {
                _tmpNoticeVerify.SetText("* Invalid code length");
                return;
            }

            PlayerDataManager.CallFunction<AccountRegisterVerifyCodeRespone>(new AccountRegisterVerifyCodeRequest(code), (resp) =>
            {
                if (string.IsNullOrEmpty(resp.Error))
                {
                    Debug.Log("verify success!");
                    _accountCreated.SetActive(true);
                    _verifyCode.SetActive(false);
                }
                else
                {
                    Debug.LogError(resp.Error);
                    _tmpNoticeVerify.SetText(resp.Error);
                }
            });
        }

        public void OnReturnClick()
        {

        }

        private async void CountdownResendCode()
        {
            while (_counterResend < _timeResend)
            {
                await Task.Delay(1000);
                _counterResend++;
                _tmpSendCode.SetText($"{_timeResend - _counterResend}s");
                _btnResend.interactable = false;
            }
            _counterResend = 0;
            _tmpSendCode.SetText("Send Code");
            _btnResend.interactable = true;
        }
        #endregion

        #region Account created
        public void OnLoginClick()
        {
            gameObject.SetActive(false);
            _loginPanel.GetComponent<CanvasGroup>().alpha =1;
            _loginPanel.SetActive(true);
        }
        #endregion
    }
}