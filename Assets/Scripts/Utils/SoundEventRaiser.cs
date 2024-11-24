using System;
using Events.ScriptableObjects;
using UnityEngine;

namespace Utils
{
    public class SoundEventRaiser : MonoBehaviour
    {
        [SerializeField] private EventChannelSO<AK.Wwise.Event> onSoundEvent;

        public void OnEnable()
        {
            onSoundEvent?.onTypedEvent.AddListener(RaiseSound);
        }

        public void OnDisable()
        {
            onSoundEvent?.onTypedEvent.RemoveListener(RaiseSound);
        }

        public void RaiseSound(AK.Wwise.Event soundEvent)
        {
            AkSoundEngine.PostEvent(soundEvent.Id, gameObject);
        }
    }
}
