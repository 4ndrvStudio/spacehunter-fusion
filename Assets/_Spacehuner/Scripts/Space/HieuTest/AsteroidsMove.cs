using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Networking.Space;

public class AsteroidsMove : MonoBehaviour
{
    private Vector3 _randomRotation;

    private Vector3 _randomMovement;

    [Header("Explosion")]
    [SerializeField] private GameObject asteroidsExplosion;

    private void Start()
    {
       // _randomRotation = new Vector3(Random.Range(0f, 100f), Random.Range(0f, 100f), Random.Range(0f, 100f));
        //_randomMovement = new Vector3(Random.Range(0.0f, 0.6f), Random.Range(0.0f, 0.6f), Random.Range(0.0f, 0.6f));
    }

    private void Update()
    {
        //if (RoomSpaceManager.Instance.CurrentNetworkEntity.IsHost)
        //{
        //    transform.Rotate(_randomRotation * Time.deltaTime);
        //    transform.Translate(_randomMovement);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Instantiate(asteroidsExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
