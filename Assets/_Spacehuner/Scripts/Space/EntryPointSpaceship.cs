using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using SH;
using SH.Define;
using System.Threading.Tasks;
using SH.Networking.Station;
using DG.Tweening;

public class EntryPointSpaceship : MonoBehaviour
{
    [SerializeField] private Transform _entryPoint;
    
    [SerializeField] private Transform _player;
    [SerializeField] private CinemachineFreeLook _cine;
    [SerializeField] private GameObject _spaceShipController;

   [SerializeField] private bool moveDirection = false;
   [SerializeField] private bool enterSpaceship =false;

    [SerializeField] private Transform _endPoint = default;

    private RoomStationNetworkEntityView _currentRoomStationView = default;

    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Player") 
        {
            Debug.Log("enter");
            _currentRoomStationView = other.GetComponentInParent<RoomStationNetworkEntityView>();
            if (_currentRoomStationView.IsMine)
            {
                _player = other.transform.parent.transform;
                _player.GetComponent<PlayerMovement>().LockControl();
                _player.GetComponent<PlayerMovement>().enabled = false;
                _player.GetComponent<Rigidbody>().useGravity = false;
                _player.GetComponentInChildren<Animator>().SetFloat("vertical", 1f);
                _player.position = Vector3.MoveTowards(_player.position, _entryPoint.position, 10f * Time.deltaTime);
                _player.LookAt(_entryPoint.position);
                if (Vector3.Distance(_player.position, _entryPoint.position) < 0.5f)
                {
                    _player.gameObject.SetActive(false);
                    moveDirection = false;
                    _cine.gameObject.SetActive(false);
                    Camera.main.GetComponent<SpaceshipCamera>().enabled = true;
                    enterSpaceship = false;
                    LoadScene();
                }
            }
        }
    }

    private async void LoadScene()
    {
        _spaceShipController.transform.DOMove(_endPoint.position, 3).SetEase(Ease.OutFlash);
        await Task.Delay(3000);
        if (RoomStationManager.Instance != null)
        {
            RoomStationManager.Instance.LeaveRoom(async () =>
            {
               UIManager.Instance.LoadScene(SceneName.SceneSpace);
            });
        }
    }
}
