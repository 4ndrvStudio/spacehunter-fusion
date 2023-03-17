using SH.Networking.Station;
using SH.PlayerData;

using UnityEngine;


namespace SH
{
    public class UISceneStation : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas = default;


        private void Start()
        {
            UIManager.Instance.ShowChat();
            _canvas.worldCamera = UIManager.Instance.UICamera;
        }

        private void Update()
        {
            // if (RoomStationManager.Instance != null)
            //     UIManager.Instance.SetPing(RoomStationManager.Instance.Ping);
        }
       
    }
}
