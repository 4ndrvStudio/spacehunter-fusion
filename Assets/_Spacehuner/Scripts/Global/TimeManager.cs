using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public long CurrentSeconds => DateTimeOffset.Now.ToUnixTimeSeconds();
}
