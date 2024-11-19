using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DashBar : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onDashUsedEvent;
        [SerializeField] private FloatEventChannelSO onDashRechargeEvent;
        [SerializeField] private VoidEventChannelSO onDashRechargedEvent;
        [SerializeField] private Slider slider;
        
        private void OnEnable()
        {
            slider.value = 1;
            
            onDashUsedEvent.onEvent.AddListener(HandleDashUsed);
            onDashRechargedEvent.onEvent.AddListener(HandleDashRecharged);
            onDashRechargeEvent.onFloatEvent.AddListener(HandleDashRecharge);
        }


        private void OnDisable()
        {
            onDashUsedEvent.onEvent.RemoveListener(HandleDashUsed);
            onDashRechargedEvent.onEvent.RemoveListener(HandleDashRecharged);
            onDashRechargeEvent.onFloatEvent.RemoveListener(HandleDashRecharge);
        }
        
        private void HandleDashRecharge(float percentage)
        {
            slider.value = percentage;
        }
        
        private void HandleDashRecharged()
        {
            slider.value = 1;
        }

        private void HandleDashUsed()
        {
            slider.value = 0;
        }
    }
}
