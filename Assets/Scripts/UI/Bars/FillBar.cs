using System;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Bars
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private bool shouldStartHided;
    
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onTakeDamage;
        [SerializeField] private IntEventChannelSO onSumHealth;
        [SerializeField] private IntEventChannelSO onResetDamage;
        [SerializeField] private IntEventChannelSO onInitializeSlider;

        private int _maxValue;
        private bool _wasTriggered = false;

        private void Awake()
        {
            onInitializeSlider?.onIntEvent.AddListener(HandleInit);
        }

        void Start()
        {
            if (shouldStartHided)
            {
                fillImage.gameObject.SetActive(false);
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
            fillImage.fillAmount = 1;
        }
    
        public void HandleInit(int maxValue)
        {
            Debug.Log($"Handle init {maxValue}");
            _maxValue = maxValue;
            fillImage.fillAmount = 1;
        }
        
        public void HandleTakeDamage(int currentHealth)
        {
            if (!_wasTriggered)
            {
                fillImage.gameObject.SetActive(true);
                _wasTriggered = true;
            }
            
            Debug.Log($"Handle take damage {_maxValue} {fillImage.fillAmount}");

            fillImage.fillAmount = (float)currentHealth / (float)_maxValue;
        }
    }
}