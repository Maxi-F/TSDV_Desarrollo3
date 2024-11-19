using System;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Bars
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private bool shouldStartHided;
    
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onTakeDamage;
        [SerializeField] private IntEventChannelSO onSumHealth;
        [SerializeField] private IntEventChannelSO onResetDamage;
        [SerializeField] private IntEventChannelSO onInitializeSlider;
        
        private bool _wasTriggered = false;

        private void Awake()
        {
            onInitializeSlider?.onIntEvent.AddListener(HandleInit);
        }

        void Start()
        {
            if (shouldStartHided)
            {
                slider.gameObject.SetActive(false);
                _wasTriggered = false;
            }

            onSumHealth?.onIntEvent.AddListener(HandleTakeDamage);
            onTakeDamage?.onIntEvent.AddListener(HandleTakeDamage);
            onResetDamage?.onIntEvent.AddListener(HandleReset);
        }

        private void OnDestroy()
        {
            onSumHealth?.onIntEvent?.RemoveListener(HandleTakeDamage);
            onTakeDamage?.onIntEvent.RemoveListener(HandleTakeDamage);
            onInitializeSlider?.onIntEvent.RemoveListener(HandleInit);
            onResetDamage?.onIntEvent.RemoveListener(HandleReset);
        }

        public void HandleReset(int currentHp)
        {
            slider.value = currentHp;
        }
    
        public void HandleInit(int maxValue)
        {
            Debug.Log($"Handle init {maxValue}");
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
        
        public void HandleTakeDamage(int currentHealth)
        {
            if (!_wasTriggered)
            {
                slider.gameObject.SetActive(true);
                _wasTriggered = true;
            }
            Debug.Log($"Handle take damage {slider.maxValue} {slider.value}");
            slider.value = currentHealth;
        }
    }
}