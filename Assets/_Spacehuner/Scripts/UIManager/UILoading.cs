using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject _content = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private TextMeshProUGUI _tmpPercent = null;
    [SerializeField] private TextMeshProUGUI _tmpLoading = null;

    public void Show(int progress, string loading = "Loading...")
    {
        if(!_content.activeInHierarchy)
            _content.SetActive(true);
        _slider.value = progress;
        _tmpPercent.SetText($"{progress}%");
        _tmpLoading.SetText(loading);
    }

    public void Hide()
    {
        _content.SetActive(false);
    }

}
