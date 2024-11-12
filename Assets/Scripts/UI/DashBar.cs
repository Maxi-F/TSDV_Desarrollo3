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
        [SerializeField] private Image fillImage;
        
        private void OnEnable()
        {
            fillImage.fillAmount = 1;
            
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
            fillImage.fillAmount = percentage;
        }
        
        private void HandleDashRecharged()
        {
            fillImage.fillAmount = 1;
        }

        private void HandleDashUsed()
        {
            fillImage.fillAmount = 0;
        }
    }
}
