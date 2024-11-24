using System;
using Events.ScriptableObjects;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerSoundController : MonoBehaviour
    {
        [Header("Pause Data")] 
        [SerializeField] private PauseSO pauseData;
        
        [Header("Motor")]
        [SerializeField] private AK.Wwise.Event motorSound;
        [SerializeField] private AK.Wwise.RTPC motorRTPC;
        [SerializeField] private float initMotorValue;
        [SerializeField] private float motorDiff;
        [SerializeField] private EventChannelSO<YMovementState> onMovementStateChange;
        
        [Header("Hit Sounds")]
        [SerializeField] private AK.Wwise.Event hitSound;
        [SerializeField] private AK.Wwise.Event hitVoice;

        [Header("Attack Sounds")] 
        [SerializeField] private AK.Wwise.Event attackSound;
        [SerializeField] private AK.Wwise.Event attackVoice;

        [Header("Dash sounds")] 
        [SerializeField] private AK.Wwise.Event dashSound;
        
        [Header("Game Over state")]
        [SerializeField] private AK.Wwise.Event dieSound;
        [SerializeField] private AK.Wwise.State gameOverState;

        private float _motorValue;

        private void Start()
        {
            AkSoundEngine.SetRTPCValue(motorRTPC.Id, _motorValue);
        }

        private void OnEnable()
        {
            _motorValue = initMotorValue;
            pauseData?.onPause.AddListener(HandlePause);
            onMovementStateChange?.onTypedEvent.AddListener(HandleMovementChange);
            motorSound.Post(gameObject);
        }

        private void OnDisable()
        {
            pauseData?.onPause.RemoveListener(HandlePause);
            onMovementStateChange?.onTypedEvent.RemoveListener(HandleMovementChange);
        }

        private void HandleMovementChange(YMovementState movementState)
        {
            AkSoundEngine.SetRTPCValue(motorRTPC.Id,
                movementState == YMovementState.Upwards ? _motorValue + motorDiff :
                movementState == YMovementState.Downwards ? _motorValue - motorDiff : _motorValue);
        }

        private void HandlePause(bool isPaused)
        {
            if (isPaused)
                motorSound.Stop(gameObject);
            else
                motorSound.Post(gameObject);
        }

        public void HitSound()
        {
            hitSound.Post(gameObject);
            hitVoice.Post(gameObject);
        }

        public void AttackSound()
        {
            attackSound.Post(gameObject);
            attackVoice.Post(gameObject);
        }

        public void DashSound()
        {
            dashSound.Post(gameObject);
        }

        public void DeathSound()
        {
            AkSoundEngine.SetState(gameOverState.GroupId, gameOverState.Id);
        }

        public void ExplosionSound()
        {
            dieSound.Post(gameObject);
        }
    }
}
