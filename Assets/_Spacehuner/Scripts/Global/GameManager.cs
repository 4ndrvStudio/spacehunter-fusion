using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using SH.Multiplayer;
using TMPro;

namespace SH
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private TextMeshProUGUI _versionText;
        public string GameVersion;
        
        public bool RequireReConnect;

        void Awake()
        {
            if (Instance == null) 
                Instance = this;

            _versionText.text = GameVersion;

        }
        void LateUpdate()
        {


            if (RequireReConnect) CheckReconnect();

        }

        void CheckReconnect()
        {
            if (Application.isFocused)
            {
                Network_ClientManager.Reconnecting();
                RequireReConnect = false;
            }
        }


        public async Task<bool> CheckUpdate()
        {
            TaskCompletionSource<bool> async = new TaskCompletionSource<bool>();

            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
                result =>
            {
                if (result.Data == null || !result.Data.ContainsKey("gameVersion")) Debug.Log("No Found GameInformation");
                else Debug.Log("Version: " + result.Data["gameVersion"]);

                if (result.Data["gameVersion"] != GameVersion)
                    async.SetResult(true);
                else
                    async.SetResult(false);
            },
            error =>
            {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            }
            );

            await async.Task;



            return async.Task.Result;
        }

    }

}
