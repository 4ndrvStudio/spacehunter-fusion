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

namespace SH.Multiplayer
{

    public class Network_ClientManager : MonoBehaviour
    {

        public static Network_ClientManager Instance;

        [SerializeField] private static NetworkRunner _networkRunner;
        [SerializeField] private static Network_GameManager _networkGameManager;
        [SerializeField] private static NetworkSceneManagerDefault _networkSceneManagerDefault;

        [SerializeField] private static SceneDefs _currentScene;


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
                GameMode = GameMode.Client,
                SessionName = sceneDefs.ToString() + "test3",
                Scene = (int)sceneDefs,
                SceneManager = _networkSceneManagerDefault,
                
            });

            Debug.Log("Load game Status :  " + startGameResult.ErrorMessage);

            Application.targetFrameRate = 300;

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

        private void OnGUI()
        {

            // if (GUI.Button(new Rect(0, 0, 600, 120), "Join Game"))
            // {
            //     StartGame(SceneDefs.scene_stationFusion);
            // }
            // if (GUI.Button(new Rect(0, 120, 600, 120), "Go To Mining"))
            // {
            //     MoveToRoom(SceneDefs.scene_miningFusion);
            // }

        }
     

    }

}
