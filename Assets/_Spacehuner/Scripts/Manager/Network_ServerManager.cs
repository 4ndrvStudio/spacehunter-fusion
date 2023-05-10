using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using SH.Define;

namespace SH.Multiplayer
{
   

    public class Network_ServerManager : MonoBehaviour
    {
       // Start is called before the first frame update
        [SerializeField] private NetworkRunner _networkRunner;
        [SerializeField] private int Room = -1;
        

        void Start()
        {   
                 
            Debug.Log(Application.version);

            if (CommandLineUtils.IsHeadlessMode() == true)
            {
                if (CommandLineUtils.TryGetArg(out string room, "-room"))
                {
                    Room = int.Parse(room);
                }
                if (Room != -1) StartGame();
            } else {
                SceneManager.LoadScene(SceneName.SceneLogin);
            }
    
        }

        async void StartGame()
        {
            _networkRunner.ProvideInput = true;

            SceneDefs scene = (SceneDefs)Room;

            await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Server,
                SessionName = scene.ToString()+ "test2",
                Scene = Room,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }



       
    }

}
