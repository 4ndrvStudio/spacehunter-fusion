using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_EnemyStats : NetworkBehaviour
    {

        [SerializeField] private Network_EnemyBrain _enemyBrain;
        [SerializeField] private HealthBar _healBar;


        [Networked(OnChanged = nameof(OnHpChanged))]
        public int HP { get; set; }
        
        public bool WasHealSetup;



        public override void Spawned()
        {
            if(Object.HasStateAuthority) {
                _healBar.Setup(HP);
            }
        }
        public void Update() {
            if(!WasHealSetup) {
                _healBar.Setup(HP);
                WasHealSetup = true;
            }
        }

        public void ReduceHealth(int targetReduce)
        {
            if (Object.HasStateAuthority == false) return;
            HP -= targetReduce;
        }

        static void OnHpChanged(Changed<Network_EnemyStats> changed)
        {
            changed.Behaviour.OnHpChanged();
        }
        private void OnHpChanged()
        {
            _healBar.UpdateHealth(HP);

            if (HP <= 0)
            {
                _enemyBrain.E_AI_STATE = E_AI_STATE.Dead;
                _healBar.gameObject.SetActive(false);

            }

        }

    }




}


