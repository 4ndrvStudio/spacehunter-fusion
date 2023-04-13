using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;

namespace SH
{
    public class BuildingDoorTrigger : MonoBehaviour
    {
        [SerializeField] private Building _building;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;

            Network_Player _network_Player = other.gameObject.GetComponent<Network_Player>();

            if (_network_Player.HasInputAuthority == false) return;

            if (_building.IsEnter == false)
            {
               
                _building.EnterBuilding();
                 _network_Player.transform.position  = _building.InsideSpawner.position;
                _building.IsEnter = true;
            }
            else
            {
               
                _building.ExitBuilding();
                 _network_Player.transform.position  = _building.OutsideSpawner.position;
                _building.IsEnter = false;
            }



        }


    }

}
