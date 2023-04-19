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

        
    }

}
