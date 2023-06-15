using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelect : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    public void Select()
    {
        _anim.SetTrigger("select");
        _anim.ResetTrigger("deselect");
    }

    public void Deselect()
    {
        _anim.SetTrigger("deselect");
        _anim.ResetTrigger("select");
    }
}
