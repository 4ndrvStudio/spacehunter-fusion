using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private Vector3 _extends;

    public Collider[] hitColliders;

    // Update is called once per frame
    void Update()
    {
        hitColliders = Physics.OverlapBox(_center.transform.position, _extends / 2f, _center.transform.rotation);
        Debug.Log(Application.targetFrameRate);
    }

       void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 255, 0, 0.8f);
            DrawCube(_center.transform.position, _center.transform.rotation ,new Vector3(_extends.x, _extends.y, _extends.z));
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
