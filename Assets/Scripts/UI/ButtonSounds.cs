using Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSounds : MonoBehaviour
    { 
        [SerializeField] private AK.Wwise.Event soundEvent; 
        [SerializeField] private EventChannelSO<AK.Wwise.Event> onSoundEvent;
        
        private Button _button;
        public void OnEnable()
        {
            _button ??= GetComponent<Button>();
            
            _button?.onClick.AddListener(RaiseSound);
        }
        public void OnDisable()
        {
            _button?.onClick.RemoveListener(RaiseSound);
        }
        
        private void RaiseSound()
        {
            onSoundEvent.RaiseEvent(soundEvent);
        }
    }
}
