using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TimerSlider : MonoBehaviour
    {
        [SerializeField] private Gradient _gradient;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _fillImage;

        private float _timer, _maxTimer;

        private void Update()
        {
            _timer += Time.deltaTime;
        }

        [Button]
        public void UpdateColor()
        {
            float fillAmount = _fillImage.fillAmount;
            _fillImage.color = _gradient.Evaluate(fillAmount);
        }

        public void SetValue(float value)
        {
            _timer = value;
            float timerIntensity = _timer / _maxTimer;

            _fillImage.fillAmount = timerIntensity;
            _fillImage.color = _gradient.Evaluate(timerIntensity);
        }
    }
}
