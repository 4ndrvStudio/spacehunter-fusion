using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SH.UI
{
    
    public class UICraftingChooseSlot : MonoBehaviour
    {
        public ECraftingType CraftingType;
        public bool CanCraft;
        public Button CraftButton;
      
    }   

    public enum ECraftingType {
        None,
        Weapon,
        Spaceship
    }

}
