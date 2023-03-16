using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponHookTest : MonoBehaviour
{
    
    [SerializeField] private InputTest _inputHandler;
    [SerializeField] private PlayerStatsManager _playerStatsManager;
    [SerializeField] private GameObject _weaponHolder;
    [SerializeField] private List<WeaponClass> _weaponClass = new List<WeaponClass>();
    
    public GameObject _weaponInUse = null;
    private Weapon _weapon;    
    [SerializeField] private bool _canChangeWeapon = true;
    
    public BoxCollider _weaponCol;

    // Start is called before the first frame update
    void Start  () {
    }
    
    void Update() {
       // GetDefaultWeaponInMining();
        _weaponInUse = _weaponHolder.transform.GetChild(0).gameObject;
        _weapon = _weaponInUse.GetComponent<Weapon>();
        _weaponCol = _weaponInUse.GetComponent<BoxCollider>();
        _weapon._playerStatsManager = _playerStatsManager;
        //_weapon._inputHandler = _inputHandler;
    }
   

    public void ChangeWeapon(int weaponId) {
        //
        if(!_canChangeWeapon) return;
        
        if(_weaponInUse != null) RemoveWeapon();
        
        // equip weapon
        GameObject weapon = _weaponClass[_weaponClass.FindIndex( weapon => weapon.Id == weaponId)].Prefab;
        _weaponInUse = Instantiate(weapon);
        _weaponInUse.transform.SetParent(_weaponHolder.transform);
        _weaponInUse.transform.localPosition = weapon.transform.position;
        _weaponInUse.transform.localRotation = weapon.transform.localRotation;
        _inputHandler.WeaponEquipped = true;

        //setup weapon
        InitialSetupWeapon(_weaponInUse);

    }

     void InitialSetupWeapon(GameObject _weaponInUse) {
          //get weapon col
        _weaponCol = _weaponInUse.GetComponent<BoxCollider>();
        // get weapon script
        _weapon = _weaponInUse.GetComponent<Weapon>();
        _weapon._playerStatsManager = _playerStatsManager;
        //_weapon._inputHandler = _inputHandler;
    }

    public void RemoveWeapon() {
        if(_weaponInUse != null) Destroy(_weaponInUse);
        _weaponInUse = null;
        _inputHandler.WeaponEquipped = false;
    }

    private void GetDefaultWeaponInMining() {
        if(SceneManager.GetActiveScene().name == SH.Define.SceneName.SceneMining) {
            ChangeWeapon(1001);
            _canChangeWeapon = false;
        } else {
            _canChangeWeapon = true;
        }
    }


    // enable and disable collider in Animation Event;
    public void enableBoxCol() {
        _weaponCol.enabled = true;
    }
    public void disableBoxCol() {
        _weaponCol.enabled = false;
    }
}
