using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace SH.Multiplayer
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField] private Camera _mainCam;
        public Camera MainCamera => _mainCam;

        [SerializeField] private CinemachineVirtualCamera _cameraVT;

        // rotate Cam
        [SerializeField] private UITouchField _touchField;
        [SerializeField] private Transform _rotatePoint;

        [SerializeField] private float _touchSpeedSensitivityX = 10f;
        [SerializeField] private float _touchSpeedSensitivityY = 10f;

        float _xInput;
        float _yInput;
        [SerializeField] float _MinX;
        [SerializeField] float _MaxX;

        void Start()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _touchField = UIControllerManager.Instance.GetTouchField();

        }
        void Update() {
            if(_rotatePoint != null) {
            
                _xInput += _touchField.TouchDist.x * _touchSpeedSensitivityX;
                _yInput += _touchField.TouchDist.y * -_touchSpeedSensitivityY;
                _yInput = Mathf.Clamp(_yInput, _MinX, _MaxX);
                _rotatePoint.rotation = Quaternion.Euler(_yInput, _xInput, 0f);
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        public void SetAimTarget(Transform body, Transform lookPoint)
        {
                _cameraVT.Follow = lookPoint;
                _rotatePoint = lookPoint;
        }

        public void ToggleInOutSide(bool isInSide)
        {
                if (isInSide == true)
                {
                    _cameraVT.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 4.5f;
                }
                else
                {
                    _cameraVT.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = 6f;
                }
        }

        public Vector3 GetMainCamEuler() => _mainCam.transform.eulerAngles;

        public Transform GetTransform() => _mainCam.transform;

    }

}
