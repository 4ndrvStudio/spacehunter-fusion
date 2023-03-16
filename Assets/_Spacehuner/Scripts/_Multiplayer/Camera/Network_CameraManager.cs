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
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        [SerializeField] private CinemachineFreeLook _cineCam;


        public void SetAimTarget(Transform body, Transform lookPoint) {
            
            _cineCam.Follow = body;
            _cineCam.LookAt = lookPoint;
       
        }

        public Vector3 GetMainCamEuler() => _mainCam.transform.eulerAngles;

        public Transform GetTransform() => _mainCam.transform;


    
    
    }

}
