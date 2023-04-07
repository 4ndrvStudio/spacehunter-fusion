using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_RootmotionCustom : MonoBehaviour
{
    [SerializeField] private Animator _anim;


    // Update is called once per frame
    void Update()
    {   
        if(_anim.deltaPosition != Vector3.zero) {
            Debug.Log(_anim.deltaPosition);      
        }
      
    }
}
