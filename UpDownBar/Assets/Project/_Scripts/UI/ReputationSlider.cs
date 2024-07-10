using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ReputationSlider : MonoBehaviour
    {
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        [Button]
        public void UpdateColor()
        {
            _slider.fillRect.GetComponent<Image>().color = _gradient.Evaluate(_slider.value / _slider.maxValue);
            ParticleImage handle = _slider.handleRect.GetComponent<ParticleImage>();
            handle.startColor = _gradient.Evaluate(_slider.value / _slider.maxValue);
        }

        public void SetValue(float value)
        {
            _slider.value = value;
            _slider.fillRect.GetComponent<Image>().color = _gradient.Evaluate(value);
        }
    }
}
