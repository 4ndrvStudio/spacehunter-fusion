using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;
using UnityEngine.EventSystems;

namespace SH.Multiplayer
{
    public class Network_CameraManager : MonoBehaviour
    {

        public static Network_CameraManager Instance;

        [SerializeField] private Camera _mainCam;
        public Camera MainCamera => _mainCam;

        [SerializeField] private CinemachineFreeLook _cineCam;

        // rotate Cam
        [SerializeField] private UITouchPanel _touchInput;

        private Vector2 _lookInput;

        [SerializeField] private float _touchSpeedSensitivityX = 3f;
        [SerializeField] private float _touchSpeedSensitivityY = 3f;

        private string _touchXMapTo = "Mouse X";
        private string _touchYMapTo = "Mouse Y";

        public float TouchSensitivity_x = 10f;
        public float TouchSensitivity_y = 10f;



        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            CinemachineCore.GetInputAxis = GetInputAxis;
            _touchInput = UITouchPanel.Instance;
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

            _cineCam.Follow = body;
            _cineCam.LookAt = lookPoint;

        }

        public Vector3 GetMainCamEuler() => _mainCam.transform.eulerAngles;

        public Transform GetTransform() => _mainCam.transform;

        private float GetInputAxis(string axisName)
        {
            if (_touchInput == null) return 0;

            _lookInput = _touchInput.PlayerJoystickOutputVector();

            if (axisName == _touchXMapTo)
                return _lookInput.x / _touchSpeedSensitivityX;

            if (axisName == _touchYMapTo)
                return _lookInput.y / _touchSpeedSensitivityY;

            return Input.GetAxis(axisName);
        }

      

        float HandleAxisInputDelegate(string axisName)
        {
            switch(axisName)
         {
 
             case "Mouse X":
 
                 if (Input.touchCount>0)
                 {
                     return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
                     
                 }else{
                     return Input.GetAxis(axisName);
                 }
 
             case "Mouse Y":
                 if (Input.touchCount > 0)
                 {
                     return Input.touches[0].deltaPosition.y / TouchSensitivity_y;
                 }
                 else
                 {
                     return Input.GetAxis(axisName);
                 }
 
             default:
                 Debug.LogError("Input <"+axisName+"> not recognyzed.",this);
                 break;
         }
 
         return 0f;
        }




    }

}
