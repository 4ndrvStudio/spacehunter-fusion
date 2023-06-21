using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SH.UI
{
    public class UIInventoryTabButton : MonoBehaviour
    {

        public UIInventoryTabName TabName;
        public Image Icon;
        public GameObject ActiveBackground;
        public TextMeshProUGUI TabText;

        [HideInInspector] public Button Button;

        public void Awake() {
            Button = gameObject.GetComponent<Button>();
        }

        public void SetActive() {
            Icon.color = Color.white;
            ActiveBackground.SetActive(true);
            TabText.color = Color.white;
        }
        public void SetDeactive() {
            Icon.color = Color.gray;
            ActiveBackground.SetActive(false);
            TabText.color = Color.gray;
        }
        
    }

}
