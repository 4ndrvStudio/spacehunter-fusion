using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningSkillBtn : MonoBehaviour
{

    [SerializeField] private List<GameObject> _stacks = new List<GameObject>();
    [SerializeField] private GameObject _fullStack;

    void OnEnable()
    {
        Pickaxe.UpdateSkillStack += UpdateSkillStackListener;
        Pickaxe.UsedSkill += UsedSkillListener;
    }

    void OnDisable()
    {
        Pickaxe.UpdateSkillStack -= UpdateSkillStackListener;
        Pickaxe.UsedSkill -= UsedSkillListener;
    }

    void UpdateSkillStackListener(int stack)
    {
        if(stack >= _stacks.Count) {
            _fullStack.SetActive(true);
        };
        if(stack > _stacks.Count) return;
         //update Stack UI
        for ( int i = 0 ; i < stack ; i++) {
            if(!_stacks[i].activeSelf) _stacks[i].SetActive(true);
        }
    } 

    void UsedSkillListener(int stack) {
        _stacks.ForEach(stack => stack.SetActive(false));
        _fullStack.SetActive(false);
    }



}
