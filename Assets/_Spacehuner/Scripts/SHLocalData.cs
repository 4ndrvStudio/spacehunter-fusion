using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class SHLocalData : MonoBehaviour
    {
        public static SHLocalData Instance = default;
        private const string LocalDataKey = "SHLocalData";
        public LocalData Data = default;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (PlayerPrefs.HasKey(LocalDataKey))
            {
                Data = JsonUtility.FromJson<LocalData>(PlayerPrefs.GetString(LocalDataKey));
            }
            else
            {
                Data = new LocalData();
            }
        }

        public void Save() {
            string json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(LocalDataKey, json);
            Debug.Log($"save {Data.Password}");
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            string json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(LocalDataKey, json);
        }
    }

    [Serializable]
    public class LocalData
    {
        public string Email;
        public string Password;
    
    }

}
