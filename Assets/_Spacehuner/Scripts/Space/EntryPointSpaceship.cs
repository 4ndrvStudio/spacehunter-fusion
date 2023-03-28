using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using SH;
using SH.Define;
using System.Threading.Tasks;
using DG.Tweening;

public class EntryPointSpaceship : MonoBehaviour
{
    [SerializeField] private Transform _entryPoint;
    
    [SerializeField] private Transform _player;
    [SerializeField] private CinemachineFreeLook _cine;
    [SerializeField] private GameObject _spaceShipController;


    // [SerializeField] private Transform _endPoint = default;


    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Player") 
        {
            Debug.Log("enter");
         
                _player = other.transform.parent.transform;
                _player.GetComponent<Rigidbody>().useGravity = false;
                _player.GetComponentInChildren<Animator>().SetFloat("vertical", 1f);
                _player.position = Vector3.MoveTowards(_player.position, _entryPoint.position, 10f * Time.deltaTime);
                _player.LookAt(_entryPoint.position);
                if (Vector3.Distance(_player.position, _entryPoint.position) < 0.5f)
                {
                    _player.gameObject.SetActive(false);
                    _cine.gameObject.SetActive(false);
                    Camera.main.GetComponent<SpaceshipCamera>().enabled = true;
                    LoadScene();
                }
            
        }
    }

    private  void LoadScene()
    {
               UIManager.Instance.LoadScene(SceneName.SceneSpace);
    }
}
