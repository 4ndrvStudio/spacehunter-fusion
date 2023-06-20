using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    public abstract class UICharacterInfoPanel : MonoBehaviour
    {
        public UICharacterTabName TabName;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public virtual void Display() {}

        public virtual void Hide() {}
    } 

}
