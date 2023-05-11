using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.PlayerData;
using SH.Models.Azure;

namespace SH.AzureFunction
{
    #region Account Register
    public class AccountRegisterInfoRequest : AzureFunctionAPIRequest
    {
        public string Email;
        public string Password;

        public AccountRegisterInfoRequest(string email, string password) : base("AccountRegisterInfo")
        {
            this.Email = email;
            this.Password = password;
        }
    }

    public class AccountRegisterInfoRespone : AzureFunctionAPIRespone
    {

    }

    public class AccountRegisterSendCodeRequest : AzureFunctionAPIRequest
    {
        public string Email;

        public AccountRegisterSendCodeRequest(string email) : base("AccountRegisterSendCode")
        {
            this.Email = email;
        }
    }

    public class AccountRegisterSendCodeRespone : AzureFunctionAPIRespone
    {

    }

    public class AccountRegisterVerifyCodeRequest : AzureFunctionAPIRequest
    {
        public string Code;

        public AccountRegisterVerifyCodeRequest(string code) : base("AccountRegisterVerifyCode")
        {
            this.Code = code;
        }
    }

    public class AccountRegisterVerifyCodeRespone : AzureFunctionAPIRespone
    {

    }
    #endregion

    #region Account Reset Password
    public class AccountResetPasswordSendCodeRequest : AzureFunctionAPIRequest
    {
        public string Email;

        public AccountResetPasswordSendCodeRequest(string email) : base("AccountResetPasswordSendCode")
        {
            this.Email = email;
        }

    }

    public class AccountResetPasswordSendCodeRespone : AzureFunctionAPIRespone
    {

    }

    public class AccountResetPasswordUpdateRequest : AzureFunctionAPIRequest
    {
        public string Email;
        public string Password;

        public AccountResetPasswordUpdateRequest(string email, string password) : base("AccountResetPasswordUpdate")
        {
            this.Email = email;
            this.Password = password;
        }
    }

    public class AccountResetPasswordUpdateRespone : AzureFunctionAPIRespone
    {

    }

    public class AccountResetPasswordVerifyCodeRequest : AzureFunctionAPIRequest
    {
        public string Email;
        public string Code;

        public AccountResetPasswordVerifyCodeRequest(string email, string code) : base("AccountResetPasswordVerifyCode")
        {
            this.Email = email;
            this.Code = code;
        }
    }

    public class AccountResetPasswordVerifyCodeRespone : AzureFunctionAPIRespone
    {

    }
    #endregion

    #region Account Login
    public class AccountLoginRequest : AzureFunctionAPIRequest
    {
        public string Email;
        public string Password;
        public AccountLoginRequest(string email, string password) : base("AccountLogin")
        {
            this.Email = email;
            this.Password = password;
        }
    }

    public class AccountLoginRespone : AzureFunctionAPIRespone
    {

    }
    #endregion

    #region Create Character
    public class CreateCharacterRequest : AzureFunctionAPIRequest
    {
        public int CharacterType;
        public int IndexSlot;

        public CreateCharacterRequest(int characterType, int indexSlot) : base("CreateCharacter")
        {
            this.CharacterType = characterType;
            this.IndexSlot = indexSlot;
        }
    }

    public class CreateCharacterRespone : AzureFunctionAPIRespone
    {

    }
    #endregion

    #region Get User Data
    public class GetUserDataRequest : AzureFunctionAPIRequest
    {
        public GetUserDataRequest() : base("GetUserData")
        {

        }
    }

    public class GetUserDataRespone : AzureFunctionAPIRespone
    {
        public string PlayFabId;
        public string DisplayName;
        public CharacterData CharacterData;
        public InventoryData InventoryData;
    }
    #endregion

    #region GetVersion
    public class GetGameInformationRequest : AzureFunctionAPIRequest
    {
        public GetGameInformationRequest() : base("GetGameInformation")
        {

        }
    }

    public class GetGameInformationRespone : AzureFunctionAPIRespone
    {
        public string body;
    }
    #endregion

    #region ClaimItem
    public class ClaimItemsRequest : AzureFunctionAPIRequest
    {

        public ClaimItemRequestModel[] Items;

        public ClaimItemsRequest(ClaimItemRequestModel[] items) : base("ClaimItems")
        {
            this.Items = items;
        }

    }

    public class ClaimItemsRespone : AzureFunctionAPIRespone
    {
        public ClaimItemsResponeModel[] Items;

    }
    #endregion
}
