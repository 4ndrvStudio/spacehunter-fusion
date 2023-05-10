using Newtonsoft.Json;
using PlayFab;
using SH.AzureFunction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.CloudScriptModels;

namespace SH.PlayerData
{
    public class PlayerDataManager : MonoBehaviour
    {
        public static PlayerDataManager Instance = null;
        public static string PlayFabId;
        public static string Address;
        public static string DisplayName;

        public static PlayerCharacter Character = new PlayerCharacter();
        public static PlayerInventory Inventory = new PlayerInventory();


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void Setup(GetUserDataRespone resp)
        {
            PlayFabId = resp.PlayFabId;
            DisplayName = resp.DisplayName;
            Character.Setup(resp.CharacterData);
            Inventory.Setup(resp.InventoryData);
            
        }

        public static void CallFunction<T>(AzureFunctionAPIRequest request, Action<T> onFinish, bool showWaiting = true) where T : AzureFunctionAPIRespone, new()
        {
            if(string.IsNullOrEmpty(request.FunctionName))
            {
                Debug.LogError("Invalid function name");
                return;
            }

            Debug.LogWarning($"Request: {JsonConvert.SerializeObject(request)}");

            if (showWaiting)
                UIManager.Instance.ShowWaiting(20);

            var funcRequest = new ExecuteFunctionRequest();
            funcRequest.FunctionName = request.FunctionName;
            funcRequest.FunctionParameter = request;
            funcRequest.GeneratePlayStreamEvent = true; 
            PlayFabCloudScriptAPI.ExecuteFunction(funcRequest, (result) => 
            {
                if (showWaiting)
                    UIManager.Instance.HideWaiting();
                T respone = null;
                try
                {
                    respone = JsonConvert.DeserializeObject<T>(result.FunctionResult.ToString());
                    //respone = JsonUtility.FromJson<T>(result.FunctionResult.ToString());
                   Debug.LogWarning($"Respone: {JsonConvert.SerializeObject(result.FunctionResult)}");
                  
                
                }
                catch(Exception ex)
                {
                    if(result.FunctionResult != null)
                    {
                        Debug.LogError(result.FunctionResult.ToString());
                        Debug.LogError(ex.ToString());
                        onFinish?.Invoke(new T() { Error = ex.ToString()});
                    }
                    else
                    {
                        Debug.LogError($"No respone: {JsonConvert.SerializeObject(result)}");
                        onFinish?.Invoke(new T() { Error = "No respone"});
                    }
                    return;
                    
                }

                //if (!string.IsNullOrEmpty(respone.Error))
                //    Debug.LogError(respone.Error);
                onFinish?.Invoke(respone);
            }, (error) =>
            {
                UIManager.Instance.HideWaiting();
                Debug.LogError(error.ErrorMessage);
                Debug.LogError(error.Error.ToString());
                onFinish?.Invoke(new T() { Error = error.ErrorMessage });
            });
        }
    }
}
