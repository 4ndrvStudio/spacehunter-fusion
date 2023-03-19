using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public enum CanHitName {
        Mineral = 0,
        Enemy = 1
    }

    public class Network_WeaponCollider : NetworkBehaviour
    {
        
        [Networked]
        public NetworkBool CanHitMineral { get; set; }

        //private 
        [SerializeField] private LayerMask _mineralCollisionLayer;
        [SerializeField] private Transform _centerOverlapse;
        [SerializeField] private Vector3 _extends;
 

        private List<LagCompensatedHit> _lagCompensatedHits = new List<LagCompensatedHit>();


        public override void FixedUpdateNetwork()
        {
            if (Object.IsProxy == true)
                return;

            if (HasHitMineral()) Debug.Log("Hit Mineral");
          
        }

        public void ToggleActiveCollider(CanHitName canHitName,bool isActive) {
           
            switch (canHitName) {
                case CanHitName.Mineral : CanHitMineral= isActive;
                    break;
            }
   
        }


        public bool HasHitMineral()
        {
            if(CanHitMineral == false) return false;

            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation.OverlapBox(_centerOverlapse.transform.position, _extends, Quaternion.LookRotation(_centerOverlapse.transform.up),
                Object.InputAuthority, _lagCompensatedHits, _mineralCollisionLayer.value);

            if (count <= 0) return false;

            _lagCompensatedHits.SortDistance();

            var networkMineral = _lagCompensatedHits[0].GameObject.GetComponentInParent<Network_Mineral>();

            networkMineral.HitMineral(Object.InputAuthority);
            
      

            return true;
        }

        void OnDrawGizmosSelected()
        {
            // Draw a semitransparent red cube at the transforms position
            Gizmos.color = new Color(0, 255, 0, 0.8f);
            Gizmos.DrawCube(_centerOverlapse.transform.position, new Vector3(_extends.x, _extends.y, _extends.z));

        }
    

        

    }

}
