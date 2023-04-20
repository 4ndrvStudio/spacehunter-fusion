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

            //DOTween.KillAll();
       
            _outside.SetActive(false);

            Building building  = _buildingList.Find(building => building.BuildingName == buildingName);
            if ( Network_Player.Local.HasInputAuthority != false)  {
                
                Network_Player.Local.transform.position = building.InsideSpawner.position;
                Network_Player.Local.PlayerState.RPC_SetIsInsideBuilding(true);

            };

          
            building.Enter();

            StartCoroutine(ShowWaiting());
            
            Network_CameraManager.Instance.ToggleInOutSide(true);

        }

        public void ExitBuilding(BuildingName buildingName) {

            //DOTween.KillAll();

            _outside.SetActive(true);

            Building building  = _buildingList.Find(building => building.BuildingName == buildingName);
            
            if (Network_Player.Local.HasInputAuthority != false)  {
                Network_Player.Local.transform.position = building.OutsideSpawner.position;
                  Network_Player.Local.PlayerState.RPC_SetIsInsideBuilding(false);

            };

            building.Exit();

            StartCoroutine(ShowWaiting());

            Network_CameraManager.Instance.ToggleInOutSide(false);

          
        }
        private IEnumerator ShowWaiting() {

            UIManager.Instance.ShowWaiting(hasBackground: true);

            //UIControllerManager.Instance.ActiveController(false);
            UIControllerManager.Instance.HideAllController();
          
            yield return new WaitForSeconds(3);
           
            UIManager.Instance.HideWaiting();
                
            //UIControllerManager.Instance.ActiveController(true);
            UIControllerManager.Instance.DisplayController();

        }
    }

    

}
