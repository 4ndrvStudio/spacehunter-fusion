using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public class Weapon : MonoBehaviour
    {
        public GameObject Body;
        public WeaponDissolve WeaponDissolve;
        public Transform CenterOverlapse;
        public Vector3 OverlapseExtends;
    
        void OnDrawGizmosSelected()
        {
            // Draw a semitransparent red cube at the transforms position
            Gizmos.color = new Color(0, 255, 0, 0.8f);
            DrawCube(CenterOverlapse.transform.position, Quaternion.LookRotation(CenterOverlapse.transform.up) ,new Vector3(OverlapseExtends.x, OverlapseExtends.y, OverlapseExtends.z));
        }


        public void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

            Gizmos.matrix *= cubeTransform;

            Gizmos.DrawCube(Vector3.zero, Vector3.one);

            Gizmos.matrix = oldGizmosMatrix;
        }
    }



}
