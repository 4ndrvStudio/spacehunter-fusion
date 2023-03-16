using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    
    [SerializeField] private int _atkDame = 0;
    public int AtkDame => _atkDame;

    [SerializeField] private int _heal = 0;
    public int Heal => _heal;

    [SerializeField] private int _pDef = 0;
    public int PDef => _pDef;

    [SerializeField] private int _mDef = 0;
    public int MDef => _mDef;
    
    private void Awake() {
        GetStatsFromServer();
    }

    private void GetStatsFromServer() {
        //test
        _atkDame = 1;
    }

}
