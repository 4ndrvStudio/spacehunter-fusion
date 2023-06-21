using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using PlayFab.ClientModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SH.UI
{
   public class UIMiningRewardPopup : UIPopup
    {
        [SerializeField] private Button _confirmBtn;
        [SerializeField] private Button _checkExplorerButton;

        [SerializeField] private Button _okeLevelButton;

        [SerializeField] private GameObject _levelUpPanel;
        [SerializeField] private GameObject _rewardPanel;

        [SerializeField] private GameObject _itemHolder;

        [SerializeField] private GameObject _itemSlotPrefab;

        [SerializeField] private List<GameObject> _inventoryItemList = new List<GameObject>();

        //Level Panel
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _expAmount;
        [SerializeField] private Image _expBar;
     
    
        private UnityAction _callback;
        private string _currentTx;
        private string _digest;
        
          // Start is called before the first frame update
        void Start()
        {
            _confirmBtn.onClick.AddListener(() => ConfirmClick());
            _checkExplorerButton.onClick.AddListener(() =>CheckSuiExplorer() );
            _okeLevelButton.onClick.AddListener(() => {
                _levelUpPanel.gameObject.SetActive(false);
                _rewardPanel.gameObject.SetActive(true);
            });

            _levelUpPanel.gameObject.SetActive(true);
            _rewardPanel.gameObject.SetActive(false);

            GetLevelInfo();
            
        }
        
        public async void GetLevelInfo() {
            var rpcResult = await SuiWalletManager.GetHunterInfo(InventoryManager.Instance.CurrentHunterAddressInUse);
            
            if (rpcResult.IsSuccess == true)
            {
                string jsonNft = JsonConvert.SerializeObject(rpcResult.Result.Data.Content, Formatting.Indented);
                JObject nftJsonObject = JObject.Parse(jsonNft);

               
                string level = nftJsonObject.SelectToken("fields.level").ToString();
                string exp = nftJsonObject.SelectToken("fields.exp").ToString();
                _levelText.text = "Lv " + level;
                float currentExp = (float.Parse(exp) - float.Parse(level) * 1000f);
                _expBar.fillAmount = currentExp / 1000f;
                _expAmount.text = $"{currentExp}/1000";
            }



        }


        public override void ShowWithCallback(object customProperties, UnityAction callback = null)
        {
            base.ShowWithCallback(customProperties, callback);
             _callback = callback;

            //List<ItemInstance> items = customProperties as List<ItemInstance>;
            SuiMiningRewardModel suiMiningRewardModel =  customProperties as SuiMiningRewardModel;
            _digest = suiMiningRewardModel.Digest;
            Dictionary<string, GameObject> itemDictionary = new Dictionary<string, GameObject>();

            foreach (var item in suiMiningRewardModel.itemsInstances)
            {
                int level = int.Parse(item.CustomData["Level"]);
                
                string itemKey = item.ItemId + level;

                if (!itemDictionary.ContainsKey(itemKey))
                {
                    GameObject inventoryItemEl = Instantiate(_itemSlotPrefab, _itemHolder.transform);
                    
                    UIItemSlotReward inventoryElScript = inventoryItemEl.GetComponent<UIItemSlotReward>();

                    ItemConfig itemConfig = InventoryManager.Instance.ItemConfigs.Find(itemConfig => itemConfig.ItemId == item.ItemId);
                    if (item.ItemInstanceId != null)
                    {
                        itemConfig.ItemInstanceId = item.ItemInstanceId;
                    }
                    inventoryElScript.Setup(level, itemConfig);

                    _inventoryItemList.Add(inventoryItemEl);
                    itemDictionary.Add(itemKey, inventoryItemEl);
                }
                else
                {
                    // Stack item
                    GameObject stackedItem = itemDictionary[itemKey];
                    stackedItem.GetComponent<UIItemSlotReward>().StackItem();
                }
            }

        }

      
        void CheckSuiExplorer() {
             string objectURL = $"https://suiexplorer.com/txblock/{_digest}?network=testnet";
            Application.OpenURL(objectURL);
        }
        void ConfirmClick()
        {
            _inventoryItemList.ForEach(item => {
                Destroy(item);
            });     
            _inventoryItemList.Clear();

            if (_callback != null)
                 _callback?.Invoke();

            Hide();
        }

    
    }
        public class SuiMiningRewardModel
        {   
            public List<ItemInstance> itemsInstances;
            public string Digest;
        }

}
