using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.UI
{
    public class UICharacterInfoPanel_Gear : UICharacterInfoPanel
    {
        [SerializeField] private GameObject ContentOb;
        [SerializeField] private UICharacterRenderTexture _uiCharacterRenderTexture;

        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        } 

        public override void Display() {
            ContentOb.SetActive(true);
        }
        public override void Hide() {
            ContentOb.SetActive(false);

        }
    }

}
