using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonDrivenUI : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject thisParentPanel;
    [SerializeField] private bool isDisableThis = false;
    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(() => ClickButton(button));
        }
    }
    private void ClickButton(Button clickedButton)
    {
        for(int i = 0; i<= buttons.Length; i++)
        {
            Button btn = buttons[i];
            GameObject panel = panels[i];
            panel.SetActive(btn == clickedButton);
            if (isDisableThis)
            {
                thisParentPanel.SetActive(false);
            }
        }
    }

}
