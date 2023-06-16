using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;
using SH.Define;
using SH.PlayerData;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Suinet.Rpc.Types;
using Suinet.Rpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayFab;
using PlayFab.ClientModels;


namespace SH.Multiplayer
{

    public class Network_ClientManager : MonoBehaviour
    {

        public static Network_ClientManager Instance;

        [SerializeField] private static NetworkRunner _networkRunner;
        [SerializeField] private static Network_GameManager _networkGameManager;
        [SerializeField] private static NetworkSceneManagerDefault _networkSceneManagerDefault;

        [SerializeField] private static SceneDefs _currentScene;
        
        //just for test
        private static RpcResult<TransactionBlockBytes> _currentTx;
        private static int _currentStoneToClaim;

        public static UnityAction ConfirmGasFeesAction;
        public static UnityAction ConfirmClaimAction;

        void OnEnable() {
            ConfirmGasFeesAction += ClaimRewardFromMining;
            ConfirmClaimAction += ExecuteExitMining;
        }

        void OnDisable() {
            ConfirmGasFeesAction -= ClaimRewardFromMining;
            ConfirmClaimAction -= ExecuteExitMining;
        }

        void Start()
        {
            if (Instance == null) Instance = this;

            _networkRunner = GetComponent<NetworkRunner>();
            _networkGameManager = GetComponent<Network_GameManager>();
            _networkSceneManagerDefault = GetComponent<NetworkSceneManagerDefault>();
            
        }

        public static async void StartGame(SceneDefs sceneDefs)
        {
            _networkRunner.ProvideInput = true;

            UIManager.Instance.ShowLoadScene(false);

            StartGameResult startGameResult = await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Host,
                SessionName = sceneDefs.ToString() + "suidemo",
                Scene = (int)sceneDefs,
                SceneManager = _networkSceneManagerDefault,
            });

            Debug.Log("Load game Status :  " + startGameResult.ErrorMessage);

            Application.targetFrameRate = 60;
        }

        public static async void ExitRoomMining(ulong exp, List<ulong> amountStone, List<string> symbolStone) {
         
            UIManager.Instance.ShowWaiting();


            var rpcResult = await SuiWalletManager.EndFarming(exp,amountStone,symbolStone);

            _currentTx = rpcResult;
            _currentStoneToClaim = (int) amountStone[0];

            var getDry = await SuiApi.Client.DryRunTransactionBlockAsync(rpcResult.Result.TxBytes.ToString()); 
            
            JObject jsonObject = JObject.Parse(getDry.RawRpcResponse);

            JArray balanceChangesArray = (JArray)jsonObject["result"]["balanceChanges"];

            Debug.Log(balanceChangesArray[0]["amount"].ToString());
       
            SuiEstimatedGasFeesModel gasFeesModel = new SuiEstimatedGasFeesModel();
            gasFeesModel.CanExcute = true;
            if(balanceChangesArray[0]["amount"].ToString() != null)
                gasFeesModel.EstimatedGasFees = balanceChangesArray[0]["amount"].ToString();
            else 
                gasFeesModel.EstimatedGasFees = "Can not estimated gas fees";
            UIManager.Instance.HideWaiting();

            UIManager.Instance.ShowPopupWithCallback(PopupName.SuiEstimatedGas,gasFeesModel, ConfirmGasFeesAction);
        }

        public static async void ClaimRewardFromMining() {
            if(_currentTx == null)  return;

            UIManager.Instance.ShowWaiting();

            var rpcResult = await SuiWalletManager.Execute(_currentTx);
            
            if(rpcResult.IsSuccess) {
                List<ItemInstance> rewardItem =  InventoryManager.Instance.GetFakeStoneItems(_currentStoneToClaim);
                UIManager.Instance.ShowPopupWithCallback(PopupName.SuiMiningReward, rewardItem, ConfirmClaimAction);
            }
            else {
                UIManager.Instance.ShowAlert("Something errors!", AlertType.Warning);
            }

            Debug.Log(rpcResult.RawRpcResponse);
        }
        public static void ExecuteExitMining() {
            MoveToRoom(SceneDefs.scene_station);
        }

        public static async void MoveToRoom(SceneDefs sceneDefs)
        {
            UIManager.Instance.ShowLoadScene(false);  
            await _networkGameManager.LeaveRom();
            Instance.StartCoroutine(StartGameAsync(sceneDefs));
        }

        public static async void Reconnecting() 
        {
            UIManager.Instance.ShowLoadScene(true);  

            await _networkGameManager.LeaveRom();

            Instance.StartCoroutine(StartGameAsync((SceneDefs) SceneManager.GetActiveScene().buildIndex));

        }


        static IEnumerator StartGameAsync(SceneDefs sceneDefs)
        {

            yield return new WaitUntil(() => _networkRunner.IsRunning == false);

            StartGame(sceneDefs);
        }

    
     

    }

}
