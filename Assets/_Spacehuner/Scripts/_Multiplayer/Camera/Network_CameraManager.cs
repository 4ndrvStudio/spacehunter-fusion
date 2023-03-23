using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace SH.Multiplayer
{
    public class Network_CameraManager : MonoBehaviour
    {  

        public static Network_CameraManager Instance;

        [SerializeField] private Camera _mainCam;
        
        [SerializeField] private CinemachineFreeLook _cineCam;

        // rotate Cam
        [SerializeField] private UITouchPanel _touchInput;

        private Vector2 _lookInput;

        [SerializeField] private float _touchSpeedSensitivityX = 3f;
        [SerializeField] private float _touchSpeedSensitivityY = 3f;

        private string _touchXMapTo = "Mouse X";
        private string _touchYMapTo = "Mouse Y";


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
        } 



        
        void LateUpdate() {
                if(Input.GetKey(KeyCode.Delete)) {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                if(Input.GetKey(KeyCode.Tab)) {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                //get touch input
                while(_touchInput == null) 
                {
                    _touchInput = UITouchPanel.Instance;
                }
              
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }




        public void SetAimTarget(Transform body, Transform lookPoint) {
            
            _cineCam.Follow = body;
            _cineCam.LookAt = lookPoint;
       
        }

        public Vector3 GetMainCamEuler() => _mainCam.transform.eulerAngles;

        public Transform GetTransform() => _mainCam.transform;

        private float GetInputAxis(string axisName)
        {
            if(_touchInput == null) return 0;

            _lookInput = _touchInput.PlayerJoystickOutputVector();

            if (axisName == _touchXMapTo)
                return _lookInput.x / _touchSpeedSensitivityX;

            if (axisName == _touchYMapTo)
                return _lookInput.y / _touchSpeedSensitivityY;

            return Input.GetAxis(axisName);
        }

        public void SetUITouchPanel (UITouchPanel touchPanel) {
             _touchInput = touchPanel;
        }
    
    
    }

}
