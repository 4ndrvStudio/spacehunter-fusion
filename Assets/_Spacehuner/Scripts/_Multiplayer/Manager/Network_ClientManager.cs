using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using SH.Define;
using SH.PlayerData;
namespace SH.Multiplayer
{
    
    public class Network_ClientManager : MonoBehaviour
    {

        [SerializeField] private static NetworkRunner _networkRunner;
        [SerializeField] private static Network_GameManager _networkGameManager;
        [SerializeField] private static NetworkSceneManagerDefault _networkSceneManagerDefault;

        void Start() {

            _networkRunner = GetComponent<NetworkRunner>();
            _networkGameManager = GetComponent<Network_GameManager>();
            _networkSceneManagerDefault = GetComponent<NetworkSceneManagerDefault>();
            

        }

        public static async void StartGame(SceneDefs sceneDefs)
        {
            _networkRunner.ProvideInput = true;

            await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = sceneDefs.ToString(),
                Scene = (int) sceneDefs,
                SceneManager = _networkSceneManagerDefault
            });
      
            
        }

        public static async void MoveToRoom(SceneDefs sceneDefs)
        {   
            await _networkGameManager.LeaveRom();
            StartGame(sceneDefs);
        }
        private void OnGUI()
        {
         
                if (GUI.Button(new Rect(0, 0, 200, 40), "Station"))
                {
                    StartGame(SceneDefs.scene_stationFusion);
                }
              
            
        }


    }

}
