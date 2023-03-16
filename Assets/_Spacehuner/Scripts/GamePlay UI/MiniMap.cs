using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Camera _miniMapCam;
    [SerializeField] private CinemachineFreeLook _cineCam;
    [SerializeField] private GameObject _spherePoint;
    [SerializeField] private Material _pointMaterial;

    private Transform _playerPoint;

     void Start()
     {
        StartCoroutine(InitialSetup());
     }

    void FixedUpdate()
    {
        //camera follow player 
        if (_playerPoint != null)
            _miniMapCam.transform.position = new Vector3(_playerPoint.position.x, _miniMapCam.transform.position.y, _playerPoint.position.z);
    }

    IEnumerator InitialSetup() {
         yield return new WaitUntil(() => _cineCam.Follow != null);
        _playerPoint = _cineCam.Follow;
         Instantiate(_spherePoint, _playerPoint.transform);
    }

  


    
    
}
