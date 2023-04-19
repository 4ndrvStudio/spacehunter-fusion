using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SH.Multiplayer;

namespace SH
{
    public class BuildingDoorTrigger : MonoBehaviour
    {
        public bool IsFirstLoad;
        [SerializeField] private Building _building;
      

        void OnTriggerEnter(Collider other)
        {

            if (other.tag != "Player" || IsFirstLoad == true) return;

            Network_Player _network_Player = other.gameObject.GetComponent<Network_Player>();

            if (_network_Player.HasInputAuthority == false) return;
            BuildingInteractType targetType = _building.IsEnter  ? BuildingInteractType.Exit : BuildingInteractType.Enter;
            
            Dictionary<string,object> customProperties = new Dictionary<string, object>() {
                    {InteractButtonCustomProperties.Name , _building.BuildingName},
                    {InteractButtonCustomProperties.BuildingInteractType, targetType }
            };

            UIControllerManager.Instance.AddInteractButton(_building.gameObject.GetInstanceID(),InteractButtonType.Building,customProperties);

        }

        void OnTriggerExit(Collider other) {
            
            if (other.tag != "Player") return;

            Network_Player _network_Player = other.gameObject.GetComponent<Network_Player>();
            
            if (_network_Player.HasInputAuthority == false) return;

            UIControllerManager.Instance.RemoveInteractionButton(_building.gameObject.GetInstanceID());

            IsFirstLoad = false;

        }


    }

}
