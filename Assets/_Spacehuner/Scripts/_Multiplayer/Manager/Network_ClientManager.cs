using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;
using SH.Define;
using SH.PlayerData;
namespace SH.Multiplayer
{
    
    public class Network_ClientManager : MonoBehaviour
    {

        public static Network_ClientManager Instance;
 
        [SerializeField] private static NetworkRunner _networkRunner;
        [SerializeField] private static Network_GameManager _networkGameManager;
        [SerializeField] private static NetworkSceneManagerDefault _networkSceneManagerDefault;
        

        void Start() {
            if(Instance == null) Instance = this;

            _networkRunner = GetComponent<NetworkRunner>();
            _networkGameManager = GetComponent<Network_GameManager>();
            _networkSceneManagerDefault = GetComponent<NetworkSceneManagerDefault>();
            

        }

        public static async void StartGame(SceneDefs sceneDefs)
        {
            _networkRunner.ProvideInput = true;
            Debug.Log("Started : " + _networkRunner.IsRunning);

            StartGameResult startGameResult = await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = sceneDefs.ToString(),
                Scene = (int) sceneDefs,
                SceneManager = _networkSceneManagerDefault
            });
            Debug.Log("status :  " + startGameResult.ErrorMessage);
   
        }

        public static async void MoveToRoom(SceneDefs sceneDefs)
        {   
            await _networkGameManager.LeaveRom();
            Instance.StartCoroutine(StartGameAsync(sceneDefs));
        }


        static IEnumerator StartGameAsync(SceneDefs sceneDefs) {

                yield return new WaitUntil(() => _networkRunner.IsRunning == false);

                StartGame(sceneDefs);
        }

        private void OnGUI()
        {
         
                if (GUI.Button(new Rect(0, 0, 200, 40), "Join Game"))
                {
                    StartGame(SceneDefs.scene_stationFusion);
                }
                if (GUI.Button(new Rect(0, 40, 200, 40), "Go To Mining"))
                {
                    MoveToRoom(SceneDefs.scene_miningFusion);
                }
              
            
        }


    }

}
