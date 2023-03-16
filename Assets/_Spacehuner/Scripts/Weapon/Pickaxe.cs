using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Networking.PVE;


public class Pickaxe : Weapon
{

    [SerializeField] private int _atkDame = 1;
    [SerializeField] private int _skillDame = 5;
    [SerializeField] private int _hitCount = 0;
    [SerializeField] private int _skillTime = 5;
    [SerializeField] private bool _useSkill  = false;

    [SerializeField] private GameObject _skillFX;


    //Event 
    public delegate void MiningSkillEvent(int stack);
    public static event MiningSkillEvent UpdateSkillStack;
    public static event MiningSkillEvent UsedSkill;

    //public override int GetDame()
    //{
    //    _hitCount+=1;
    //    if(UpdateSkillStack != null) UpdateSkillStack(_hitCount);
    //    if(_hitCount >4  && _inputHandler.UseMiningSkill) {
    //        _hitCount = 0;
    //        _inputHandler.UseMiningSkill = false;
    //        if(UsedSkill != null ) UsedSkill(_hitCount);
    //        return _skillDame;
    //    } else return _atkDame;
    //}
    //protected override void UpdateStates() {
    //    if(_inputHandler != null) {
    //        _inputHandler.CanUseMiningSkill  = _hitCount > 4 ? true: false;
    //        _skillFX.gameObject.SetActive( _inputHandler.UseMiningSkill && _hitCount >=4);
    //    }
    //}
    public override int GetDame()
    {
        return 5;
    }







}
