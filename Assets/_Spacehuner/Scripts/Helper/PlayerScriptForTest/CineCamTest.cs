using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineCamTest : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cine;
    [SerializeField] private Joystick _rotateJoy;
    [Range(0f, 100f)]
    [SerializeField] private float _speedX;
    [Range(0f, 100f)]
    [SerializeField] private float _speedY;


    // Update is called once per frame
    void Update()
    {
        _cine.m_XAxis.Value += _rotateJoy.Horizontal*_speedX * Time.deltaTime;
        _cine.m_YAxis.Value -= _rotateJoy.Vertical*_speedY * Time.deltaTime ;
    }
}
