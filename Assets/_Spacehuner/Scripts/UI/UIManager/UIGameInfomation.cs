using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameInfomation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmpInfo = default;

    private float _pollingTime = 1f;
    private float _time;
    private int _frameCount;

    private long _ping = 0;

    private float _battery = 0;


    private void Start()
    {
        Application.targetFrameRate = 60;
        _battery = GetBattery;
        StartCoroutine(GetPing());
    }

    private void OnDestroy()
    {
        StopCoroutine(GetPing());
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _frameCount++;
        if (_time >= _pollingTime)
        {
            int frameRate = Mathf.RoundToInt(_frameCount / _time);
            string msg = $"Battery: {_battery}%  {frameRate} FPS";
            if (_ping != 0)
                msg += $"  {_ping} ms";
            _tmpInfo.SetText(msg);
            _time -= _pollingTime;
            _frameCount = 0;
        }
    }

    private IEnumerator GetPing()
    {
        while(true)
        {
            yield return new WaitForSeconds(60);
            _battery = GetBattery;
        }
    }

    private int GetBattery => Mathf.RoundToInt(SystemInfo.batteryLevel * 100);

    public void SetPing(long ping) => _ping = ping;
}
