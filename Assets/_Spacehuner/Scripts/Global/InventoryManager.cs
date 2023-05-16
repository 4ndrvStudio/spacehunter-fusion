using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using SH.PlayerData;
using SH.AzureFunction;
using SH.Models.Azure;

namespace SH
{
    public class InventoryManager : MonoBehaviour
    {

        public static InventoryManager Instance;

        [SerializeField] private List<ItemInstance> _items = new List<ItemInstance>();
        [HideInInspector] public List<ItemInstance> Items => _items;


        
        public List<Sprite> ItemFrame = new List<Sprite>();

        public List<ItemConfig> ItemConfigs = new List<ItemConfig>();

        public static UnityAction OnInventoryDataChange;

        void Awake()
        {

            if (Instance == null)
                Instance = this;

        }

        public void GetInventoryData()
        {
            PlayFabManager.Instance.GetInventoryData(
                res =>
                {
                    this._items = res.Inventory;
                    OnInventoryDataChange?.Invoke();
                },
                err =>
                {
                    Debug.LogError("Get inventory Error : " + err.ErrorMessage);
                }
            );
        }

        public void AddInventoryItem(ClaimItemRequestModel[] requestModels)
        {
            ClaimItemsRequest claimItemsRequest = new ClaimItemsRequest(requestModels);
            PlayerDataManager.CallFunction<ClaimItemsRespone>(claimItemsRequest, 
                    (res) =>
                           {
                               if (string.IsNullOrEmpty(res.Error))
                               {
                                   foreach (ClaimItemsResponeModel item in res.Items)
                                   {
                                       Debug.Log($"You claim {item.DisplayName} !");
                                   }
                                   GetInventoryData();
                               }
                               else
                               {
                                   Debug.LogError(res.Error);
                               }
                           });
        }
        
       




    }
}

