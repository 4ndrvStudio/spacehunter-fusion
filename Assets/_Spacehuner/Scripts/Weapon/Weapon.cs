using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public PlayerStatsManager _playerStatsManager;
    public InputHandler _inputHandler;
    protected int _attackDame = 1 ;
    
    protected void Start() {
        InitialSetup();
    }

    protected void Update() {
        UpdateStates();
    }

    protected virtual void InitialSetup() {} 
    protected virtual void UpdateStates() {}

    public virtual int GetDame() {
        int atkDame = _playerStatsManager.AtkDame + _attackDame;
        return atkDame;
    }

}
