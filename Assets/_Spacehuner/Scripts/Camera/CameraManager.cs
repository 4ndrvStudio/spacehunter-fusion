using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;
using UnityEngine.EventSystems;

namespace SH.Multiplayer
{
    public class CameraManager : MonoBehaviour
    {

        public static CameraManager Instance;

        [SerializeField] private Camera _mainCam;
        public Camera MainCamera => _mainCam;

        [SerializeField] private CinemachineFreeLook _cineCam;

        // rotate Cam
        [SerializeField] private UITouchPanel _touchInput;

        private Vector2 _lookInput;

        [SerializeField] private float _touchSpeedSensitivityX = 10f;
        [SerializeField] private float _touchSpeedSensitivityY = 10f;

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

        public void ToggleInOutSide(bool isInSide)
        {
            if (isInSide == true)
            {
                _cineCam.m_Orbits[0].m_Radius = 3;
                _cineCam.m_Orbits[1].m_Radius = 6;
                _cineCam.m_Orbits[2].m_Radius = 3;
            }
            else
            {
                _cineCam.m_Orbits[0].m_Radius = 7;
                _cineCam.m_Orbits[1].m_Radius = 9;
                _cineCam.m_Orbits[2].m_Radius = 7;
            }
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
            switch (axisName)
            {

                case "Mouse X":

                    if (Input.touchCount > 0)
                    {
                        return Input.touches[0].deltaPosition.x / TouchSensitivity_x;

                    }
                    else
                    {
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
                    Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                    break;
            }

            return 0f;
        }




    }

}
