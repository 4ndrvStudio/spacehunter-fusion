using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SH.Multiplayer
{
    public class Network_Portal : MonoBehaviour
    {

        private void OnTriggerStay(Collider other)
        {
           if(other.tag != "Player") return;

           Network_Player network_Player = other.GetComponent<Network_Player>();
           
           if(network_Player.HasInputAuthority) {
                  UIControllerManager.Instance.ShowGotoMiningBtn(true);
           }
            
        }

        private void OnTriggerExit(Collider other) {

            if(other.tag != "Player") return;

           Network_Player network_Player = other.GetComponent<Network_Player>();
           
           if(network_Player.HasInputAuthority) {
                  UIControllerManager.Instance.ShowGotoMiningBtn(false);
           }
        }
       
    }

}
