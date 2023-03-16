
using System.Collections;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    
    [SerializeField] private PlayerStateTest _playerState;
    [SerializeField] private Joystick _movementJoy;

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


    void Start() {
  
    }

    void Update() 
    {
        GetInput();
    }

    private void GetInput() {
        // Vertical = Input.GetAxis("Vertical");
        // Horizontal = Input.GetAxis("Horizontal");

        Vertical = _movementJoy.Vertical;
        Horizontal = _movementJoy.Horizontal;


        //runing
        Run = true; 

        // //attack
        // if(Input.GetMouseButton(0) && CanPress) 
        // {
        //     PlayAttack = true;
        //     StartCoroutine(AvoidMultiPress());
        // }

        // //lock
        // if(Input.GetKeyDown(KeyCode.F))
        //     LockOn = !LockOn;

        // if(Input.GetKeyDown(KeyCode.I))
        //     IsMining = !IsMining;

        // //jump
        // if (_playerState.isGrounded && Input.GetButtonDown("Jump") && CanPress) {
        //     Jump = true;
        //      StartCoroutine(AvoidMultiPress());
        // };

        // if(Input.GetKeyDown(KeyCode.E) && CanUseMiningSkill) {
        //     UseMiningSkill = true;
        // }

    }


    private void EnterCursorMode() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CursorLock = true;
    }

    private void CheckCursorMode() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CursorLock = false;
        } else if (!CursorLock && Input.GetMouseButtonDown(0)) {
            EnterCursorMode();
        }
    }


    private IEnumerator AvoidMultiPress()
    {
        CanPress = false;
        yield return new WaitForSeconds(0.5f);
        CanPress = true;
    }


}
