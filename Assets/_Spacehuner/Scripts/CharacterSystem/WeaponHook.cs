using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SH.Define;
using SH.Networking.PVE;
using SH.Networking.Mining;

public class WeaponHook : MonoBehaviour
{
    [Header("Initial")]
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private PlayerStatsManager _playerStatsManager;
    [SerializeField] private GameObject _weaponHolder;
    
    [Header("List Weapons Of User")]
    [SerializeField] private List<WeaponClass> _weaponClass = new List<WeaponClass>();

    [Header("Weapon In Using")]
    [SerializeField] private GameObject _weaponInUse = null;
    [SerializeField] private bool _canChangeWeapon = true;
    [SerializeField] private Weapon _weapon;    
    [SerializeField] private BoxCollider _weaponCol;

    [SerializeField] private RoomMiningNetworkEntityView _miningView = default;

    public bool IsWeaponMining => _isWeaponMining; 

    private bool _isWeaponMining = false;

    private void Start  () 
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == SceneName.SceneMining)
        {
            ChangeWeapon(1001);
            _isWeaponMining = true;
        }

    }

    //for test only. khi nao co' UI Se su dung doi vu khi tu` inventory;
    private void Update() 
    {
            //if(Input.GetKeyDown(KeyCode.G)) {
            //    if(_weaponInUse != null) return;
            //    ChangeWeapon(1001); //change to mineaxe
            //}
            //if(Input.GetKeyDown(KeyCode.H)) {
            //    RemoveWeapon();
            //}
            //GetDefaultWeaponInMining();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SwapWeapon")
        {
            if (_isWeaponMining)
            {
                ChangeWeapon(1002);
                _isWeaponMining = false;
            }
            else
            {
                ChangeWeapon(1001);
                _isWeaponMining = true;
            }
        }
    }


    public void ChangeWeapon(int weaponId) {
        if(!_canChangeWeapon) return;
        
        if(_weaponInUse != null) 
            RemoveWeapon();
        
        // equip weapon
        GameObject weapon = _weaponClass[_weaponClass.FindIndex( weapon => weapon.Id == weaponId)].Prefab;
        _weaponInUse = Instantiate(weapon);
        _weaponInUse.transform.SetParent(_weaponHolder.transform);
        _weaponInUse.transform.localPosition = weapon.transform.position;
        _weaponInUse.transform.localRotation = weapon.transform.localRotation;
        //_inputHandler.WeaponEquipped = true;
        //setup weapon
        InitialSetupWeapon(_weaponInUse);

    }

     void InitialSetupWeapon(GameObject _weaponInUse) {
          //get weapon col
        _weaponCol = _weaponInUse.GetComponent<BoxCollider>();
        // get weapon script
         _weapon = _weaponInUse.GetComponent<Weapon>();
         _weapon._playerStatsManager = _playerStatsManager;
         _weapon._inputHandler = _inputHandler;
    }

    public void RemoveWeapon() {
        if(!_canChangeWeapon) return;
        
        if(_weaponInUse != null) Destroy(_weaponInUse);
        _weaponInUse = null;
        //_inputHandler.WeaponEquipped = false;
    }

    //private void GetDefaultWeaponInMining() {
    //    if(SceneManager.GetActiveScene().name == SceneName.SceneMining) {
    //        ChangeWeapon(1001);
    //        _canChangeWeapon = false;
    //        //_inputHandler.IsMining =true;
    //    } else {
    //        _canChangeWeapon = true;
    //    }
    //}

    public void EnableBoxCol() 
    {
        if(_miningView.IsMine)
        {
            _weaponCol.enabled = true;
        }
    }

    public void DisableBoxCol() 
    {
        if (_miningView.IsMine)
        {
            _weaponCol.enabled = false;
        }
    }
}
