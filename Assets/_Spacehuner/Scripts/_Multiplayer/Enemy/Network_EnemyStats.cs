using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_EnemyStats : NetworkBehaviour
    {

        [SerializeField] private Network_EnemyBrain _enemyBrain;


        [Networked(OnChanged = nameof(OnHpChanged))]  
        [SerializeField] public int HP {get; set;}
        

        public void ReduceHealth(int targetReduce) 
        {   
            if(Object.HasStateAuthority == false) return;
               HP-= targetReduce;
        }

        static void OnHpChanged(Changed<Network_EnemyStats> changed)
        {
            changed.Behaviour.OnHpChanged();
        }
        private void OnHpChanged()
        {
            
            if(HP<=0) {
                _enemyBrain.E_AI_STATE = E_AI_STATE.Dead;
            } 
           
            
        }

    

    
    }

}
