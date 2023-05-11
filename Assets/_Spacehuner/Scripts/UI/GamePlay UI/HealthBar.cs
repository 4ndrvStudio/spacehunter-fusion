using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace SH.Multiplayer
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _sliderHealth = default;

        //[SerializeField] private TextMeshProUGUI _tmpHealth = default;

        [SerializeField] private int _maxHealth = default;

        [SerializeField] private int _currentHealth = default;

        public int Health => _currentHealth;

        public void Setup(int currentHealth)
        {
            _maxHealth = currentHealth;
            _currentHealth = currentHealth;
            _sliderHealth.maxValue = _maxHealth;
            _sliderHealth.value = _maxHealth;
           // _tmpHealth.SetText($"{_currentHealth}/{_maxHealth}");
        }

        public void UpdateHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
            _sliderHealth.DOValue(_currentHealth, 0.2f).SetEase(Ease.Flash);
            //_tmpHealth.SetText($"{_currentHealth}/{_maxHealth}");

        }

        private void LateUpdate()
        {
            transform.LookAt(CameraManager.Instance.GetTransform());
            transform.Rotate(0, 180, 0);
        }
    }
}
