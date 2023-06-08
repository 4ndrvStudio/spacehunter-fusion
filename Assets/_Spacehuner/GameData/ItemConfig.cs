using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Game Data/Item/Item Config")]
    public class ItemConfig : ScriptableObject
    {
        public bool CanUse;
        public string ItemId;
        public Sprite ItemIcon;
        public string ItemInstanceId;
        //Check Later
        public UIInventoryTabName TypeTab;
        // custom icon with level

        public List<Sprite> IconWithLevel = new List<Sprite>();

    }



}
