using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{

    public class Network_Mineral : NetworkBehaviour
    {

        [Networked] private NetworkBool _wasHit { get; set; }

        [Networked(OnChanged = nameof(OnHpChanged))]  
        [SerializeField] private byte _hp {get; set;}
        
        //local
        [SerializeField] private HealthBar _healthBar;

        public override void Spawned()
        {
            _healthBar.Setup(_hp);
        }

        public void HitMineral(PlayerRef player)
        {
            if (Object == null) return;
            if (Object.HasStateAuthority == false) return;
            if (_wasHit) return;
            

            if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            {
              
               playerNetworkObject.GetComponentInChildren<Network_WeaponCollider>().ToggleActiveCollider(CanHitName.Mineral, false);
           
            }
            _wasHit = true;
        }
        public override void FixedUpdateNetwork()
        {   
           

            if (Object.HasStateAuthority && _wasHit)
            {
                _wasHit = false;
                _hp--;

                if(_hp <=0) {
                    Runner.Despawn(Object);
                }
            }

     
           
        }

        static void OnHpChanged(Changed<Network_Mineral> changed)
        {
            changed.Behaviour.OnHpChanged();
        }
        private void OnHpChanged()
        {
            _healthBar.UpdateHealth(_hp);
        }


        
    }

}
