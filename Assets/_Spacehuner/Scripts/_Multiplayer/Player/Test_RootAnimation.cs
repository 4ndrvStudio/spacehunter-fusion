using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_RootAnimation : MonoBehaviour
{

    public AnimationClip clip;
    public Animator anim;

    void Start()
    {
      
    }

    void Update()
    {
       Debug.Log(anim.deltaPosition);
      
        clip.SampleAnimation(transform.gameObject,Time.deltaTime);
    }
}
