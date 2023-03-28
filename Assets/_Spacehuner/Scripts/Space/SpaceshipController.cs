using SH.Networking.Space;
using System.Collections;
using UnityEngine;
using SH.Define;

public class SpaceshipController : MonoBehaviour
{

    [Header("Initials")]
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private Joystick _dirJoyStick;
    [SerializeField] private Joystick _moveJoyStick;
    [SerializeField] private ButtonRef _boostButton;
    [SerializeField] private GameObject _shipBody;
    [SerializeField] private GameObject _explosionFX;


    [Header("Movement")]
    [SerializeField] private float _lookRotateSpeed = 90f;

    [SerializeField] private float _rollSpeed = 90f;

    [SerializeField] private float _rollAcceleration = 3.5f;

    [SerializeField] private float _forwardSpeed = 25f;

    [SerializeField] private float _forwardAcceleration = 2.5f;


    [SerializeField] private float _boostSpeed = 100f;

    [SerializeField] private float _boostAcceleration = 3.5f;

    [SerializeField] private Vector3 _lastPosition;

    [SerializeField] private RoomSpaceNetworkEntityView _view = default;

    public bool IsMine => _view.IsMine;
    public string Id => _view.OwnerId;

    [HideInInspector]
    private Vector3 currentMovement;

    private float startVelocity;

    private AudioSource engineSound;

    private AudioSource rollSound;

    [Header("Direction")]
    private Vector2 _moveDirection;

    private Vector2 screenCenter;

    private Vector2 screenSafeCenterSize;

    private float rollInput;

    private float activeRollAmount;

    private float activeForwardSpeed;

    private float activeStrafeSpeed;

    private float activeHoverSpeed;

    private bool _isController = false;

    private bool _isDeath = false;

    public void Setup(bool isController)
    {
        _isController = isController;
        if (_isController)
        {
            _lastPosition = this.transform.position;
            _rigid.velocity = this.transform.forward * startVelocity;
            _dirJoyStick = GameObject.Find("Direction Controller").GetComponent<DynamicJoystick>();
            _moveJoyStick = GameObject.Find("Roll Controller").GetComponent<DynamicJoystick>();
        }
    }

    private void Update()
    {
        if (_view != null)
        {
            if (_view.IsMine)
            {
                GetInput();
                Movement();
            }
        }
    }


    private void GetInput()
    {
        // _moveDirection.x = _dirJoyStick.Horizontal;
        // _moveDirection.y = _dirJoyStick.Vertical;
        _moveDirection = _dirJoyStick.Direction;
        _moveDirection = Vector2.ClampMagnitude(_moveDirection, 0.95f);
        if (_moveJoyStick == null) { }
        else rollInput = -_moveJoyStick.Horizontal;
    }


    private void Movement()
    {

        activeRollAmount = Mathf.Lerp(activeRollAmount, rollInput, _rollAcceleration * Time.deltaTime);
        this.transform.Rotate((0f - _moveDirection.y) * _lookRotateSpeed * Time.deltaTime, _moveDirection.x * _lookRotateSpeed * Time.deltaTime, activeRollAmount * _rollSpeed * Time.deltaTime, Space.Self);
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, 1f * _forwardSpeed, _forwardAcceleration * Time.deltaTime);

        if (_boostButton != null && _boostButton.isPressed)
        {
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, _boostSpeed, _boostAcceleration * Time.deltaTime);
        }

        this.transform.position += this.transform.forward * activeForwardSpeed * Time.deltaTime + this.transform.right * activeStrafeSpeed * Time.deltaTime + this.transform.up * activeHoverSpeed * Time.deltaTime;
        //engineSound.pitch = 0.5f + Mathf.Abs(activeForwardSpeed) / boostSpeed * 2.5f;

        currentMovement = this.transform.position - _lastPosition;
        _lastPosition = this.transform.position;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        if (!_isDeath)
        {
            if (other.CompareTag("Asteroid"))
            {
                SpaceshipCollide();
                _isDeath = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (!_isDeath)
        {
            if (other.collider.CompareTag("Planet") || other.collider.CompareTag("Asteroid"))
            {
                SpaceshipCollide();
                _isDeath = true;
            }
        }
    }

    public void SpaceshipCollide() {
                _forwardSpeed = 0;
                _shipBody.SetActive(false);
                _explosionFX.SetActive(true);
                StartCoroutine(ReturnToSpaceStation());
    }
    private IEnumerator ReturnToSpaceStation()
    {
        yield return new WaitForSeconds(3f);
        RoomSpaceManager.Instance.LeaveRoom(() =>
             {
                 UIManager.Instance.LoadScene(SceneName.SceneStation);
             });
    }






}
