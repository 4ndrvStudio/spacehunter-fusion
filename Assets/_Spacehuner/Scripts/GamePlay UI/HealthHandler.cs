using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

namespace SH
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _tmpHealth = default;

        [SerializeField] private Transform _healthBar = default;

        [SerializeField] private int _totalHealth = default;

        [SerializeField] private int _currentHealth = default;

        [SerializeField] private float _speed = default;

        private float _percent = default;

        private float _localScaleX = default;

        private void Awake()
        {
            _localScaleX = _healthBar.localScale.x;
        }

        public void Setup(int currentHealth, int totalHealth)
        {
            _totalHealth = totalHealth;
            _currentHealth = totalHealth;
            _healthBar.localScale = new Vector3(_localScaleX, _healthBar.localScale.y);
            _percent = _localScaleX / _totalHealth;
            if(_tmpHealth != null)
                _tmpHealth.SetText($"{_currentHealth}/{_totalHealth}");
            UpdateHealth(totalHealth - currentHealth, null);

        }

        public void UpdateHealth(int damage, Action OnDeath)
        {
            int healthRemain = _currentHealth - damage;
            this._currentHealth = healthRemain;
            if (_tmpHealth != null)
                _tmpHealth.SetText($"{_currentHealth}/{_totalHealth}");
            float localScale = _healthBar.localScale.x - _percent * damage;
            _healthBar.DOScaleX(localScale, _speed);
            if (_tmpHealth != null)
                _tmpHealth.SetText($"{_currentHealth}/{_totalHealth}");

            if (_currentHealth <= 0)
                OnDeath?.Invoke();
        }


        private void FixedUpdate()
        {
            FaceToCamera();
        }

        private void FaceToCamera()
        {
            //_healthBar.LookAt(Camera.main.transform);
            //_tmpHealth.transform.LookAt(Camera.main.transform);
            //_healthBar.Rotate(0, 180f, 0);
        }
    }
}
