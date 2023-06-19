using PlayFab;
using PlayFab.ClientModels;
using SH.PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class PlayFabManager : MonoBehaviour
    {
        public static PlayFabManager Instance = null;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        //----------------------------------------Account 
        public void LoginWithCustomId(string customId, bool createAccount, Action<LoginResult> onSuccess, Action<PlayFabError> onError)
        {
            LoginWithCustomIDRequest req = new LoginWithCustomIDRequest()
            {
                CustomId = customId,
                CreateAccount = createAccount,
            };
            PlayFabClientAPI.LoginWithCustomID(req, onSuccess, onError);
        }

        public void SetDisplayName(string displayName, Action<UpdateUserTitleDisplayNameResult> onSuccess, Action<PlayFabError> onError)
        {
            UpdateUserTitleDisplayNameRequest req = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = displayName,
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(req, onSuccess, onError);
            
        }

        public void CheckAccountInfo(Action<GetAccountInfoResult> OnSuccess, Action<PlayFabError> onError)
        {
            GetAccountInfoRequest req = new GetAccountInfoRequest()
            {

            };
            PlayFabClientAPI.GetAccountInfo(req, OnSuccess, onError);
        }

        public void RegisterAccount(string email, string password, Action<RegisterPlayFabUserResult> onSuccess, Action<PlayFabError> onError)
        {
            RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest()
            {
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = false,
            };
            PlayFabClientAPI.RegisterPlayFabUser(req, onSuccess, onError);
        }

        public void LoginWithEmail(string email, string password, Action<LoginResult> onSuccess, Action<PlayFabError> onError)
        {
            LoginWithEmailAddressRequest req = new LoginWithEmailAddressRequest()
            {
                Email = email,
                Password = password,
            };
            PlayFabClientAPI.LoginWithEmailAddress(req, onSuccess, onError);
        }

        //----------------------------------------Inventory 
        
        public void GetInventoryData(Action<GetUserInventoryResult> onSuccess, Action<PlayFabError> onError) => 
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),onSuccess,onError);
        





    }
}
