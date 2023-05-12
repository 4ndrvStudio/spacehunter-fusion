using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Game Data/Item/Item Config")]
    public class ItemConfig : ScriptableObject
    {
        public string ItemId;
        public Sprite ItemIcon;

        // custom icon with level
        public List<Sprite> IconWithLevel = new List<Sprite>();

    }

 

}
