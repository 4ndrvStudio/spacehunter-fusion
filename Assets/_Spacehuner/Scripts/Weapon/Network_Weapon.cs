using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class Network_Weapon : MonoBehaviour
    {
        
        [SerializeField] private Transform _centerOverlapse;
        [SerializeField] private Vector3 _extends;

        public Transform GetCenter() => _centerOverlapse;
        public Vector3 GetExtends() => _extends;

        void OnDrawGizmosSelected()
        {
            // Draw a semitransparent red cube at the transforms position
            Gizmos.color = new Color(0, 255, 0, 0.8f);
            Gizmos.DrawCube(_centerOverlapse.transform.position, new Vector3(_extends.x, _extends.y, _extends.z));

        }

    }

}
