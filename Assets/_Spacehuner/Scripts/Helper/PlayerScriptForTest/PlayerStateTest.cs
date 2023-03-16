using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
using SH.PlayerData;
using SH.Define;

public class PlayerStateTest : MonoBehaviour
{
    [Header("Init")]
    [SerializeField] private InputTest _inputHandler;
    [SerializeField] private ActionTest _actionManager;
    [SerializeField] private GameObject _activeModel;

    [SerializeField] public Animator _anim;
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private Transform _lookPoint;

    [Header("Stats")]
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _turnSmoothTime = 0.2f;
    [SerializeField] private float _turnSmoothVelocity;
    [SerializeField] private float _speedSmoothTime = 0.1f;
    [SerializeField] private float _jumpHigh = 6f;
    [SerializeField] private float _distToGround = 0.9f;
    [SerializeField] private float _animationSpeedPercent;
    

    // c# out
    private float _speedSmoothVelocity;
    private float _currentSpeed;

    [Header("Camera")]
    private Transform _cameraPos;
    [SerializeField] private CinemachineFreeLook _cineCam;
    [SerializeField] private TextMeshPro _tmpName = null;

    private void Start()
    {
       
            _cameraPos = Camera.main.transform;
            _anim.applyRootMotion = false;
            // _cineCam = GameObject.Find("Freelook Camera").GetComponent<CinemachineFreeLook>();
            // _cineCam.LookAt = _lookPoint;
            // _cineCam.Follow = _lookPoint;        
        
    }


    void FixedUpdate()
    {
        GroundCheck();
        PlayerMovement();
   
    }
    private void PlayerMovement()
    {
    //    if(_actionManager._enableRootMotion) {
    //            return;
    //     }
        Vector2 _input = new Vector2(_inputHandler.Horizontal, _inputHandler.Vertical);

        Vector2 inputDir = _input.normalized;
        
        // rotate player
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _cameraPos.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, _turnSmoothTime);
        }
        //check run or walk
        float targetSpeed = ((_inputHandler.Run) ? _runSpeed : _walkSpeed) * inputDir.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, _speedSmoothTime);

        //move Player
        if (_input.magnitude > 0.2f)
        {   
            
            transform.Translate(transform.forward * _currentSpeed * Time.deltaTime, Space.World);
            // transform.Translate(new Vector3(Vertical, transform.position.y, Horizontal) * _currentSpeed  * _deltaTime, Space.World );
        }
        _animationSpeedPercent = ((_inputHandler.Run) ? 1 : .5f) * inputDir.magnitude;
        

        //Player Jump;
        if (_inputHandler.Jump)
        {
            _rigid.velocity += (Vector3.up * _jumpHigh);
            _inputHandler.Jump  = false;
        }
        

        // Check Lockon on Animation
        if (!_inputHandler.LockOn) {
				HandleMovementAnimations ();
			} else {
				HandleLockOnAnimations();
		}

    }
    private void HandleMovementAnimations()
    {
        _anim.SetFloat(AnimationParam.Vertical,_animationSpeedPercent, _speedSmoothTime, Time.deltaTime);

    }
    private void HandleLockOnAnimations() {
        // Xay dung sau..
    }

    public bool isGrounded = false;

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _distToGround + 0.05f))
        {
            _anim.SetBool(AnimationParam.OnGround, false);
            isGrounded = true;
        }
        else
        {   
            isGrounded = false;
            _anim.SetBool(AnimationParam.OnGround, true);
        }
    }

   
}
