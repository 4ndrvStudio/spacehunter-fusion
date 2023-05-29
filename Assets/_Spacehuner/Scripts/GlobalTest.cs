using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTest : MonoBehaviour
{
    public static GlobalTest Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
