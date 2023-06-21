using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WeaponDissolve : MonoBehaviour
{
    public bool AvoidDissolved;

    public bool IsDissolved;

    [SerializeField] private float _noiseStrength = 0.25f;

    [SerializeField] private float _height;
    [SerializeField] private float _endHeight;
    [SerializeField] private float _objectHeight;
    [SerializeField] private float _dissolveSpeed;


    private Material _material;
    [SerializeField] private bool _startDissovle;
    [SerializeField] private bool _startUnDissovle;
    [SerializeField] private bool _resetHeight;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _height = _objectHeight;
    }

    private void FixedUpdate()
    {
        if(AvoidDissolved == true) return;

        if (_startDissovle)
        {
            _height -= Time.fixedDeltaTime * _dissolveSpeed;
            SetHeight(_height);

            if (_height < _endHeight)
            {
                _startDissovle = false;
                IsDissolved = true;
                _height = _objectHeight;
            }
        }

        if (_startUnDissovle)
        {
            _height += Time.fixedDeltaTime * _dissolveSpeed;
            SetHeight(_height);

            if (_height > _objectHeight)
            {
                _startUnDissovle = false;
                IsDissolved = true;
                _height = _objectHeight;
            }
        }

        if (_resetHeight)
        {
            _startDissovle = false;
            SetHeight(_objectHeight);
            _resetHeight = false;
            IsDissolved = false;

        }

    }

    public void DissolveWeapon() {
        _height = _objectHeight;
        _startDissovle = true;
         _startUnDissovle =false;
    }

    public void UnDissolveWeapon() {
        _height = _endHeight;
        _startUnDissovle =true;
        _startDissovle = false;
    }

    public void ActiveWeapon() => _resetHeight = true;
    public void HideWeapon() => SetHeight(_endHeight);


    private void SetHeight(float height)
    {
        _material.SetFloat("_CutoffHeight", height);
        _material.SetFloat("_NoiseStrength", _noiseStrength);
    }

}
