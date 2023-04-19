using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    public abstract class Building : MonoBehaviour
    {
        
        public BuildingName BuildingName;
        public GameObject BuildingObject;
        public GameObject InsideBuildingObject;
        public bool IsEnter;
        
        public Transform InsideSpawner;
        public Transform OutsideSpawner;

        public BuildingDoorTrigger EnterTrigger;
        public BuildingDoorTrigger ExitTrigger;


        public virtual void Enter() {

            InsideBuildingObject.SetActive(true); 
            BuildingObject.SetActive(false); 
            IsEnter = true;
            ExitTrigger.IsFirstLoad = true;

        }

        public virtual void Exit() {

            IsEnter = false;

            InsideBuildingObject.SetActive(false); 

            BuildingObject.SetActive(true);

            EnterTrigger.IsFirstLoad = true;
        }
        
    }

}
