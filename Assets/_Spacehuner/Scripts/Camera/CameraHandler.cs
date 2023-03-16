using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{

    [SerializeField] private CinemachineFreeLook _cam;    
    [SerializeField] private float _maxZoomIO =3f;
    [SerializeField] private float _initialZoom=0f;

    float __speedSmoothZoom;


    private void Awake() {
     }
    // Update is called once per frame
    void FixedUpdate()
    {
        ZoomIO();
    }

    private void ZoomIO() {
        _initialZoom +=  Input.mouseScrollDelta.y /1.5f;
        if(_initialZoom > _maxZoomIO + 3f) {
            _initialZoom = _maxZoomIO + 3f;
            return;
        }
        if(_initialZoom < -_maxZoomIO) {
            _initialZoom = -_maxZoomIO;
            return;
        }
        // // Excute zoom
        for ( int i = 0; i < 3 ; i++) {
            float targetZoom = _cam.m_Orbits[i].m_Radius += Input.mouseScrollDelta.y /1.5f  ;
            _cam.m_Orbits[i].m_Radius = Mathf.SmoothDamp(_cam.m_Orbits[i].m_Radius, targetZoom, ref __speedSmoothZoom, 0.4f *Time.deltaTime);
        }
    }   

    
}
