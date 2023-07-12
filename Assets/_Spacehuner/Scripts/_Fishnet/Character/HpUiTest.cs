using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpUiTest : MonoBehaviour
{
    public static HpUiTest Instance;

    [SerializeField] private TextMeshProUGUI _hpText;

    void Start() {
        if(Instance == null) 
            Instance = this;
    }

    public void SetTest(string text) {
        _hpText.text = "HP: " + text;
    }
}
