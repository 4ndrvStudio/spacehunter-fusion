using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SH.UI
{
    public class UIMenuTabButton : MonoBehaviour
    {
        public UIMenuTabName TabName;

        public GameObject BackgroundImage;
        public TextMeshProUGUI TabText;
        public Image Icon;

        [HideInInspector] public Button Button;

        public void Awake()
        {
            Button = gameObject.GetComponent<Button>();
        }

        public void SetActive()
        {
            BackgroundImage.SetActive(true);
            TabText.color = Color.white;
            Icon.color = Color.white;
        }
        public void SetDeactive()
        {
            BackgroundImage.SetActive(false);
            TabText.color = Color.gray;
            Icon.color = Color.grey;
        }
    }

}
