using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SH.Multiplayer;

namespace SH
{   
    public enum BuildingName {
        Music,
    }
    public enum BuildingInteractType {
        Enter,
        Exit
    }

    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance;

        [SerializeField] private GameObject _outside;
        [SerializeField] private List<Building> _buildingList = new List<Building>();

          void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }       
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void EnterBuilding(BuildingName buildingName) {

            Building building  = _buildingList.Find(building => building.BuildingName == buildingName);
            
            if ( Network_Player.Local.HasInputAuthority == false) return;
                
            Network_Player.Local.PlayerMovement.SetPosition(building.InsideSpawnerPoint);
        

            building.Enter();
          
            StartCoroutine(ShowWaiting());
            
            CameraManager.Instance.ToggleInOutSide(true);

        }

        public void ExitBuilding(BuildingName buildingName) {



            Building building  = _buildingList.Find(building => building.BuildingName == buildingName);
            
            if ( Network_Player.Local.HasInputAuthority == false) return;

            Network_Player.Local.PlayerMovement.SetPosition(building.OutsideSpawnerPoint);

            building.Exit();

            StartCoroutine(ShowWaiting());

            CameraManager.Instance.ToggleInOutSide(false);

        }
        private IEnumerator ShowWaiting() {

            UIManager.Instance.ShowWaiting(hasBackground: true);

            UIControllerManager.Instance.HideAllController();
          
            yield return new WaitForSeconds(2);
           
            UIManager.Instance.HideWaiting();
                
            UIControllerManager.Instance.DisplayController();

        }
    }

    

}
