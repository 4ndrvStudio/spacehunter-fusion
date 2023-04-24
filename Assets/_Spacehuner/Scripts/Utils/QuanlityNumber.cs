using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuanlityNumber : MonoBehaviour
{
    public static QuanlityNumber instance;
    public TextMeshProUGUI quanlity;

    private void Awake()
    {
        if (instance != null)
        {

            return;
        }
        instance = this;
    }
    public void AddQuanlity(int numb)
    {
        quanlity.text = numb.ToString();
    }
}
