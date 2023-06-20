using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SH.UI
{
    public class UICharacterTabButton : MonoBehaviour
    {

        public UICharacterTabName TabName;
        
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
