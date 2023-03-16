using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    [SerializeField] private Animator _anim;
     private void OnAnimatorMove()
    {
        Vector3 delta = _anim.deltaPosition;
				delta.y = 0;
       transform.parent.position += delta;
    }
}
