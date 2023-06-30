using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIToogleManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private GameObject[] panels;
    [SerializeField] private bool haveHover = false;
    [SerializeField] private bool havePanel = false;
    [SerializeField] private bool haveTxt = false;
    [SerializeField] private bool changeColorBtn = false;
    [SerializeField] private bool changeIconBtnColor = false;
    [Header("Add offset if you need")]
    [SerializeField] private int hoverUIOffset = 0;
    [SerializeField] private int buttonTxtOffser = 0;
    [SerializeField] private int buttonIconOffset = 0;
    [Header("Button color")]
    [SerializeField] private Color defaultBtnColor = new Color(77f / 255f, 77f / 255f, 77f / 255f);
    [SerializeField] private Color activeBtnColor;
    [Header("Icon button color")]
    [SerializeField] private Color defaultIconColor;
    [SerializeField] private Color activeIconColor;
    

    private Color defaultTextColor = new Color(121f / 255f, 121f / 255f, 121f / 255f); // màu hexadecimal 3 ông thần tài 797979 :)))
    private Color activeTextColor = Color.white;

    private void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ClickButton(button));
        }
    }

    private void ClickButton(Button clickedButton)
    {
        for (int i = 0; i < buttons.Count; i++)
        {            
            Button button = buttons[i];
            Transform activeUITransform = button.transform.GetChild(0 + hoverUIOffset);
            bool isButtonClicked = button == clickedButton;
            if (haveHover)
            {
                if (activeUITransform != null)
                {
                    activeUITransform.gameObject.SetActive(isButtonClicked);
                }
            }
            
            if (haveTxt)
            {
                TextMeshProUGUI buttonText = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = isButtonClicked ? activeTextColor : defaultTextColor;
                }
            }
            if (changeColorBtn)
            {               
                Image buttonImg = button.transform.GetComponent<Image>();
                buttonImg.color = isButtonClicked ? activeBtnColor : defaultBtnColor;
            }
            if (changeIconBtnColor)
            {
                Image iconBtn = button.transform.GetChild(0 + buttonIconOffset).GetComponent<Image>();
                if (iconBtn != null)
                {
                    iconBtn.color = isButtonClicked ? activeIconColor : defaultIconColor;
                }
            }
            if (havePanel)
            {
                GameObject panel = panels[i];
                panel.SetActive(button == clickedButton);
            }
            
        }
    }
    public void ButtonToAdd(Button button) 
    {
        buttons.Add(button);
        button.onClick.AddListener(() => ClickButton(button));
    }
}
