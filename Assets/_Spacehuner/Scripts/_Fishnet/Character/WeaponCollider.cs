using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace SH.Multiplayer.Player
{
    using FishNet;
    using FishNet.Object;
    using FishNet.Object.Prediction;
    using FishNet.Transporting;
    using FishNet.Object.Synchronizing;

    public class WeaponCollider : NetworkBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private GameObject _Center;
        [SerializeField] private Vector3 _Extends;
        [SerializeField] private LayerMask _playerMask;


        public Player _weaponOwner;

        private RaycastHit[] _hitColliders;

        [SerializeField] private List<Collider> hitColliders;

        private Dictionary<GameObject, bool> _damagedTargets = new Dictionary<GameObject, bool>();

        int hitcount = 0;

        public bool CanDamge = false;

        // // Start is called before the first frame update
        // private void Awake()
        // {
        //     InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;

        // }

        // public override void OnStartClient()
        // {
        //     base.OnStartClient();
        // }



        // private void OnDestroy()
        // {
        //     if (InstanceFinder.TimeManager != null)
        //     {
        //         InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
        //     }
        // }

        private void OnTriggerStay(Collider collider)
        {
            if (IsServer == true || CanDamge == false)
                return;

            if (collider.TryGetComponent<PlayerDamageable>(out var playerDamageable))
            {
                if (playerDamageable.OwnerId != _weaponOwner.OwnerId)
                {

                    playerDamageable.TakeDamage();
                }
            }
        }

        private void FixedUpdate()
        {

            if (IsOwner == false)
                return;

            TakeDamage();

        }

        [Client]
        public void TakeDamage()
        {
            if(CanDamge == false) 
                return;

            Physics.SyncTransforms();

            Vector3 boxCenter = transform.TransformPoint(_boxCollider.center);
            Vector3 boxExtents = _boxCollider.bounds.extents;
            Quaternion worldRotation = Quaternion.identity;

            Transform parent = _Center.transform.parent;

            while (parent != null)
            {
                worldRotation *= parent.rotation;
                parent = parent.parent;
            }

            Vector3 direction = worldRotation * Vector3.forward; 

            float maxDistance = _Extends.magnitude; 

            _hitColliders = Physics.BoxCastAll(boxCenter, boxExtents, direction, worldRotation, maxDistance, _playerMask);
            hitColliders.Clear();

            RaycastHit[] hits;
            int maxIterations = 1;
            float deltaTime = Time.fixedDeltaTime; 

            for (int i = 0; i < maxIterations; i++)
            {
                float timeStep = (i + 1) * deltaTime;
                hits = Physics.BoxCastAll(boxCenter, boxExtents, direction, worldRotation, maxDistance, _playerMask);


                foreach (RaycastHit hit in hits)
                {

                    if (hit.collider.TryGetComponent<PlayerDamageable>(out var playerDamageable))
                    {
                        // playerDamageable.TakeDamage();
                        if (playerDamageable.OwnerId != _weaponOwner.OwnerId)
                        {
                              ApplyDamage(playerDamageable);
                           
                        }
                    }
                }
            }

          
        }

        [ServerRpc]
        private void ApplyDamage(PlayerDamageable playerDamageable)
        {
              playerDamageable.TakeDamage();
        }

        public void AttackAnother()
        {
            if(CanDamge == false) 
                return;

            Physics.SyncTransforms();

            Vector3 boxCenter = transform.TransformPoint(_boxCollider.center);
            Vector3 boxExtents = _boxCollider.bounds.extents;
            Quaternion worldRotation = Quaternion.identity;

            Transform parent = _Center.transform.parent;

            while (parent != null)
            {
                worldRotation *= parent.rotation;
                parent = parent.parent;
            }

            Vector3 direction = worldRotation * Vector3.forward; 

            float maxDistance = _Extends.magnitude; 

            _hitColliders = Physics.BoxCastAll(boxCenter, boxExtents, direction, worldRotation, maxDistance, _playerMask);
            hitColliders.Clear();

            RaycastHit[] hits;
            int maxIterations = 10;
            float deltaTime = Time.fixedDeltaTime; 

            for (int i = 0; i < maxIterations; i++)
            {
                float timeStep = (i + 1) * deltaTime;
                hits = Physics.BoxCastAll(boxCenter, boxExtents, direction, worldRotation, maxDistance, _playerMask);


                foreach (RaycastHit hit in hits)
                {

                    if (hit.collider.TryGetComponent<PlayerDamageable>(out var playerDamageable))
                    {
                        // playerDamageable.TakeDamage();
                        if (playerDamageable.OwnerId != _weaponOwner.OwnerId)
                        {
                            playerDamageable.TakeDamage();
                        }
                    }
                }
            }

        }
        //     // foreach (RaycastHit hit in _hitColliders)
        //     // {

        //     //         if (hit.collider.name == "Cube (2)")
        //     //         {
        //     //              hitColliders.Add(hit.collider);
        //     //             HpUiTest.Instance.SetTest((hitcount++).ToString());
        //     //         }
        //     //     // if (_weaponOwner.OwnerId != hitCollider.GetComponent<Player>().OwnerId)
        //     //     // {
        //     //     //     PlayerDamageable playerDamageable = hitCollider.GetComponent<PlayerDamageable>();
        //     //     //     playerDamageable.TakeDamage();
        //     //     // }

        //     //     if (hit.collider.TryGetComponent<PlayerDamageable>(out var playerDamageable))
        //     //     {
        //     //         // playerDamageable.TakeDamage();
        //     //         if (playerDamageable.OwnerId != _weaponOwner.OwnerId)
        //     //         {
        //     //             playerDamageable.TakeDamage();
        //     //         }
        //     //     }
        //     // }
        // }

        // private void OnDrawGizmos()
        // {
        //     BoxCollider boxCollider = GetComponent<BoxCollider>();
        //     if (boxCollider == null)
        //         return;

        //     Vector3 boxCenter = transform.TransformPoint(boxCollider.center);
        //     Vector3 boxExtents = boxCollider.size;

        //     Quaternion worldRotation = transform.rotation;

        //     Gizmos.color = Color.red;
        //     Gizmos.matrix = Matrix4x4.TRS(boxCenter, worldRotation, Vector3.one);
        //     Gizmos.DrawWireCube(Vector3.zero, boxExtents);
        // }



    }




}