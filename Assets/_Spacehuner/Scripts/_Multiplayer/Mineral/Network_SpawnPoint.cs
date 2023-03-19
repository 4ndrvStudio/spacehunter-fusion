using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer
{
    public class Network_SpawnPoint : MonoBehaviour
    {
        public Vector3 Size;
        

        void OnDrawGizmosSelected()
        {
            // Draw a semitransparent red cube at the transforms position
            Gizmos.color = new Color(0, 255, 0, 0.8f);
            Gizmos.DrawCube(transform.position, new Vector3(Size.x, Size.y, Size.z));

        }

        public Vector3 GetSpawnPosition()
        {
            Vector3 sizeOffset = new Vector3(GetRandomInSide(Size.x),0 , GetRandomInSide(Size.z));
            // RaycastHit hit;
            // if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
            // {
            //   return hit.transform.position + sizeOffset;
            // }

            Vector3 targetPos = transform.position + sizeOffset;
            return transform.position + sizeOffset;
        }

        private float GetRandomInSide(float size)
        {
            return Random.Range(-size / 2, size / 2);
        }



    }

}
