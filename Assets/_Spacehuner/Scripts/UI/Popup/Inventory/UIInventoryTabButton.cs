using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SH
{
    public class UIInventoryTabButton : MonoBehaviour
    {

        public UIInventoryTabName TabName;
        public GameObject BackgroundImage;
        public TextMeshProUGUI TabText;

        [HideInInspector] public Button Button;

        public void Awake() {
            Button = gameObject.GetComponent<Button>();
        }

        public void SetActive() {
            BackgroundImage.SetActive(true);
            TabText.color = Color.white;
        }
        public void SetDeactive() {
            BackgroundImage.SetActive(false);
            TabText.color = Color.gray;
        }
        
    }

}
