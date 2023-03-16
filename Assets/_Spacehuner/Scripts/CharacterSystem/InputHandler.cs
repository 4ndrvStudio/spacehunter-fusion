using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{

    private static InputHandler _instance = null;
    public static InputHandler Instance => _instance;

    [SerializeField] private PlayerStateManager _playerState;

    //Acces Input
    public float Vertical, Horizontal;
    public bool Run;
    public bool PlayAttack;
    public bool LockOn = false;
    public bool WeaponEquipped = false;
    public bool IsMining = false;
    public bool Jump = false;
    public bool CanPress = true;
    public bool CursorLock = false;

    public bool UseMiningSkill = false;
    public bool CanUseMiningSkill = false;


    //test local
    public bool hasSpaceShip = false;
    public bool isInventoryOpen = false;


    void Start() {
        if (_instance == null)
            _instance = this;

        if (_instance != this)
            Destroy(this);
        //EnterCursorMode();
    }

    void Update()
    {
        //CheckCursorMode();
        GetInput();
    }

    private void GetInput() {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");

        //runing
        Run = Input.GetKey(KeyCode.LeftShift);

        //attack
        if (Input.GetMouseButton(0) && CanPress)
        {
            PlayAttack = true;
            StartCoroutine(AvoidMultiPress());
        }

        //lock
        // if (Input.GetKeyDown(KeyCode.F))
        //     LockOn = !LockOn;

        
        if (Input.GetKeyDown(KeyCode.I))
            IsMining = !IsMining;

        //jump
        if (_playerState.isGrounded && Input.GetButtonDown("Jump") && CanPress) {
            Jump = true;
            StartCoroutine(AvoidMultiPress());
        };

        //use MiningSkill
        if(Input.GetKeyDown(KeyCode.E) && CanUseMiningSkill) {
            UseMiningSkill = true;
        }
 
        if(Input.GetKeyDown(KeyCode.I)) isInventoryOpen = !isInventoryOpen;

    }

    private void EnterCursorMode() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CursorLock = true;
    }

    private void CheckCursorMode() 
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if(CursorLock)
            {
                CursorLock = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Vertical = 0;
                Horizontal = 0;
                //UIManager.Instance.ChatManager.EnterToChat();
            }
            else
            {
                CursorLock = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }


    private IEnumerator AvoidMultiPress()
    {
        CanPress = false;
        yield return new WaitForSeconds(0.5f);
        CanPress = true;
    }


}
