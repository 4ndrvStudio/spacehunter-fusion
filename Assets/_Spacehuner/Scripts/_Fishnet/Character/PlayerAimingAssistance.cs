using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.Multiplayer.Player
{
    using FishNet.Object;
    public class PlayerAimingAssistance : NetworkBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private LayerMask _enemyMask;
        [SerializeField] private List<Collider> _nearestToAttack;
        [SerializeField] private float _nearestTemp = Mathf.Infinity;
        [SerializeField] private float _assistantRangeAngle = 120;

        public Collider SelectedNearest;

        // Update is called once per frame
        public void FixedUpdate()
        {
            if (IsOwner == false)
                return;

            NearestCheck();
        }

        void NearestCheck()
        {

            //select enemy in range
            _nearestToAttack = new List<Collider>(Physics.OverlapSphere(transform.position, 3f, _enemyMask));

            if (_nearestToAttack.Count > 0)
            {

                float temp = Mathf.Infinity;
                //check nearest enemy by quick sort
                _nearestToAttack.ForEach(targetAttack =>
                {
                    if (targetAttack.TryGetComponent<Player>(out Player player))
                    {
                        if (_player.OwnerId != player.OwnerId)
                        {
                            Transform enemy = targetAttack.transform;
                            Vector3 dirToEnemy = (enemy.position - transform.position).normalized;
                            if (Vector3.Angle(transform.forward, dirToEnemy) < _assistantRangeAngle / 2)
                            {
                                Vector3 from = targetAttack.transform.position - transform.position;
                                Vector2 to = transform.forward;
                                float angle = Vector3.Angle(from, to);
                                if (angle < temp)
                                {
                                    SelectedNearest = targetAttack;
                                    temp = angle;
                                    _nearestTemp = temp;
                                }
                            }
                            else
                            {
                                if (SelectedNearest == targetAttack) SelectedNearest = null;
                            };
                        }
                    } 
                });

            }
            else
            {
                _nearestTemp = 360;
                SelectedNearest = null;
            };

        }

    }
}
