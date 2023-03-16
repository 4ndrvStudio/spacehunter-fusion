using UnityEngine;
using Cinemachine;
using SH.Networking.Station;
using System.Threading.Tasks;
using SH.Networking.Mining;
using SH.Networking.PVE;
using DG.Tweening;

namespace SH
{
    public class CameraManager : MonoBehaviour
    {
        
        [Header("Initial")]
        [SerializeField] private CinemachineFreeLook _cineCam;

        [SerializeField] private StationGameManager _stationManager = default;

        [SerializeField] private MiningGameManager _miningManager = default;

        [SerializeField] private PVEGameManager _pveManager = default;
        [Space]

        [Header("Rotate Setting")] 
        [SerializeField] private float _rotX_Speed;
        [SerializeField] private float _rotY_Speed;

        

        //Hidden in inspector
        private Vector2 currentRotPos;

        private async void Start()
        {
            if(_stationManager != null)
            {
                UIManager.Instance.ShowWaiting();
                while(_stationManager.PlayerMovement == null)
                {
                    await Task.Yield();
                }
                UIManager.Instance.HideWaiting();
                _cineCam.Follow = _stationManager.PlayerMovement.gameObject.transform;
                _cineCam.LookAt = _stationManager.PlayerMovement.LookAt;
            }

            if(_miningManager != null)
            {
                UIManager.Instance.ShowWaiting();
                while (_miningManager.PlayerMovement == null)
                {
                    await Task.Yield();
                }
                UIManager.Instance.HideWaiting();
                _cineCam.Follow = _miningManager.PlayerMovement.gameObject.transform;
                _cineCam.LookAt = _miningManager.PlayerMovement.LookAt;
            }

            if(_pveManager != null)
            {
                UIManager.Instance.ShowWaiting();
                while (_pveManager.PlayerMovement == null)
                {
                    await Task.Yield();
                }
                UIManager.Instance.HideWaiting();
                _cineCam.Follow = _pveManager.PlayerMovement.gameObject.transform;
                _cineCam.LookAt = _pveManager.PlayerMovement.LookAt;
            }
        }

        void Update()
        {
           // _cineCam.m_XAxis.Value += _roteJoy.Horizontal * 1.5f;
            //_cineCam.m_YAxis.Value -= _roteJoy.Vertical / 100;

        }

        public void RotateCam(Vector2 pointerPos) {
                
                _cineCam.m_XAxis.Value +=  pointerPos.x * _rotX_Speed;
                _cineCam.m_YAxis.Value +=  pointerPos.y / 100f * _rotY_Speed;
       
        }


        


    }
}

