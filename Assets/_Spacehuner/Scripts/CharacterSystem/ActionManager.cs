using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    [Header("Animator")]
    [SerializeField] private Animator _anim;
    [SerializeField] public bool _enableRootMotion;
 
    [Header("Attack Type")]
    [SerializeField] private string[] _oh_attack;
    [SerializeField] private string[] _mining_attack;

    [Header("Play animation")]
    [SerializeField] private bool _playAttack;
    [SerializeField] private bool _playUseItem;
    [SerializeField] private bool _interacting;
    [SerializeField] private bool _lockOn;


    private void Update() 
    {
        Setup();
        CheckWeaponEquipped();
        //InventoryAction();
    }

    private void Setup() {
        _enableRootMotion = !_anim.GetBool("canMove");
        if (_anim.GetCurrentAnimatorStateInfo(4).IsTag("LocalAction")) {
            _anim.applyRootMotion = false;
        } else _anim.applyRootMotion = _enableRootMotion;
        _interacting = _anim.GetBool("interacting");

        if(_lockOn) {
            // lock on mode
        }
        _anim.SetBool("lockOn", _lockOn);

        if(_enableRootMotion) return;

        if(_playUseItem) {
            _anim.Play("use_item");
            _playUseItem = false;
        }
        if (_interacting) {
				_inputHandler.PlayAttack = false;
                _inputHandler.Vertical = Mathf.Clamp (_inputHandler.Vertical, 0, 0.5f);
		}
        PlayWeaponAttack();
    }
    private void PlayWeaponAttack() {
        //check weapon equipped
        if(!_inputHandler.WeaponEquipped) _inputHandler.PlayAttack = false;
        
        //play Attack
        if(_inputHandler.PlayAttack) {
            string targetAnim;
            if(_inputHandler.IsMining) {
                targetAnim = _mining_attack[Random.Range(0, _mining_attack.Length)];
            } else {
                targetAnim = _oh_attack[Random.Range(0, _oh_attack.Length)];
            }
            _anim.CrossFade(targetAnim, 0.2f);
            _inputHandler.PlayAttack = false;
        }
    }

    private void CheckWeaponEquipped() {
        float _weight = _inputHandler.WeaponEquipped ? .75f : 0f;
        _anim.SetLayerWeight(1, _weight);
    }

    private void InventoryAction() {
        if(_inputHandler.isInventoryOpen) {
            UIManager.Instance.ShowPopup(PopupName.Inventory);
        } else UIManager.Instance.HidePopup(PopupName.Inventory);
    }
}

    
