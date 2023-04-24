using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace SH
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            CheckVersion();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void CheckVersion()
        {

            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
              result =>
              {
                  if (result.Data == null || !result.Data.ContainsKey("Test")) Debug.Log("No MonsterName");
                  else Debug.Log("MonsterName: " + result.Data["Test"]);
                
              },
              error =>
              {
                  Debug.Log("Got error getting titleData: ");
                  Debug.Log(error.GenerateErrorReport());
              });
        }

    }

}
