using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class TargetLookPoint : MonoBehaviour
    {
        public Transform TargetFollow;

        // Update is called once per frame
        void Update()
        {
            if(TargetFollow != null) {
                transform.position = Vector3.Lerp(transform.position, TargetFollow.transform.position + new Vector3(0,1.172f,0), 30f * Time.deltaTime);
            }
        }
    }

}
