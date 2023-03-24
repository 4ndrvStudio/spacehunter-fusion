using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Fusion;

namespace SH.Multiplayer
{
    public class Network_EnemyBrain : NetworkBehaviour
    {
       
        [Networked] public E_AI_STATE E_AI_STATE {get; set;}
        [Networked] public E_TYPE E_TYPE {get; set;}

        public List<Collider> PlayersInRange = new List<Collider>();
        public Collider SelectedPlayer;
        public float DisToPlayer = Mathf.Infinity;
        [Networked, HideInInspector] public float DisToCombat {get; set;}
        [Networked] public float DisToShortCombat {get; set;}
        [Networked] public float DisToLongCombat {get; set;}
        public LayerMask PlayerMask;
        public LayerMask ObstacleMask;
        [Networked] public NetworkBool IsStatic {get; set;}

        private void OnEnable()
        {
            // PlayerStats.PlayerDeathAction += PlayerDeathListener;
        }

        private void OnDisable()
        {
            // PlayerStats.PlayerDeathAction -= PlayerDeathListener;
        }

        public override void FixedUpdateNetwork()
        {
            if (Runner.IsServer == false) return;

            if (SelectedPlayer != null)
            {
                DisToPlayer = Vector3.Distance(transform.position, SelectedPlayer.transform.position);
            } else {
                DisToPlayer = Mathf.Infinity;

            }


            RangeCombatCheck();

        }



        void PlayerDeathListener()
        {
            SelectedPlayer = null;
            DisToPlayer = Mathf.Infinity;
        }

        void RangeCombatCheck()
        {
            if (DisToLongCombat == DisToShortCombat)
            {
                DisToCombat = DisToShortCombat;
                E_TYPE = E_TYPE.ShortRange;
            }
            else
            {
                DisToCombat = DisToPlayer < DisToShortCombat ? DisToShortCombat : DisToLongCombat;
                E_TYPE = DisToCombat == DisToLongCombat ? E_TYPE.LongRange : E_TYPE.ShortRange;
            }
        }

        public void RotateToPlayer(Vector3 target = default)
        {
            Vector3 targetRot = target != null && SelectedPlayer == null ? target : SelectedPlayer.transform.position;
           
            var lookPos = targetRot - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Runner.DeltaTime * 100);
        }

        //
        public void AimSupport()
        {
            if (SelectedPlayer != null && DisToPlayer != 0)
            {
                transform.DOMove(CalPosBeforeTarget(SelectedPlayer.transform.position, transform.position, DisToCombat), 2f);
            }
        }

        public Vector3 CalPosBeforeTarget(Vector3 target, Vector3 point, float distance)
        {

            Vector3 targetPos = target + ((point - target).normalized * distance);
            targetPos.y = point.y;
            return targetPos;
        }

        public void ReSelectNearestPlayer()
        {
            float nearestTemp = Mathf.Infinity;
            PlayersInRange.ForEach(player =>
            {
                float disToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (disToPlayer < nearestTemp)
                {
                    SelectedPlayer = player;
                    nearestTemp = disToPlayer;
                }
            });
        }

    }
    //State
    public enum E_AI_STATE
    {
        Idle,
        Chasing,
        Patrolling,
        Dead,
        Combat,
        Spawn,
        GetHit,
    }
    public enum E_TYPE
    {
        ShortRange,
        LongRange,
    }





}
