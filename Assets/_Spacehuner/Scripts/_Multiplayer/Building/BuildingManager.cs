using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SH
{   
    public enum BuildingName {
        Music,

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
            DOTween.KillAll();
            _outside.SetActive(false);
       
            Building building  = _buildingList.Find(building => building.BuildingName == buildingName);
            building.InsideBuildingObject.SetActive(true); 
            building.BuildingObject.SetActive(false); 
            
          

        }

        public void ExitBuilding(BuildingName buildingName) {
            DOTween.KillAll();
            _outside.SetActive(true);
             Building building  = _buildingList.Find(building => building.BuildingName == buildingName);
            building.InsideBuildingObject.SetActive(false); 
            building.BuildingObject.SetActive(true); 
        }
    }

    

}
