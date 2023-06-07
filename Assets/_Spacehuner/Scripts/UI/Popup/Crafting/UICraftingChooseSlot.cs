using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SH.Multiplayer
{
    public enum CraftingType {
        Weapon,
        Spaceship
    }
    public class UICraftingChooseSlot : MonoBehaviour
    {
        public CraftingType CraftingType;
        public bool CanCraft;
        public Button CraftButton;
      
    }   

}
