using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using SH.PlayerData;
using SH.AzureFunction;

namespace SH.Account
{
    public class UICreatePlayerNamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputName = default;
        [SerializeField] private GameObject _slotCharacterPanel = default;
        [SerializeField] private GameObject _createSuiWalletPanel = default;
 
        private void OnEnable()
        {
            _inputName.text = null;
        }

        public void OnCreateClick()
        {
            string displayName = _inputName.text;
            if(string.IsNullOrEmpty(displayName))
            {
                UIManager.Instance.ShowAlert("Display name is null", AlertType.Error);
                return;
            }

            if(displayName.Length < 3 || displayName.Length > 16)
            {
                UIManager.Instance.ShowAlert("Invalid length display name ", AlertType.Error);
                return;
            }

            if(!Regex.IsMatch(displayName, @"^[a-zA-Z0-9]+$"))
            {
                UIManager.Instance.ShowAlert("Display name only letters and numbers", AlertType.Error);
                return;
            }

            PlayFabManager.Instance.SetDisplayName(displayName, (result) =>
            {
                PlayerDataManager.CallFunction<GetUserDataRespone>(new GetUserDataRequest(), (resp) =>
                {
                    if (string.IsNullOrEmpty(resp.Error))
                    {
                        Debug.Log("Get data user success!");
                        PlayerDataManager.Instance.Setup(resp);
                        gameObject.SetActive(false);
                        //_slotCharacterPanel.SetActive(true);
                        _createSuiWalletPanel.SetActive(true);
                    }
                    else
                    {
                        UIManager.Instance.ShowAlert(resp.Error, AlertType.Error);
                        Debug.LogError(resp.Error);
                    }
                });
            }, (error) =>
            {
                UIManager.Instance.ShowAlert(error.ErrorMessage, AlertType.Error);
            });
        }
    }
}
