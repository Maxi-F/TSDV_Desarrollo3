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
        [SerializeField] private string hitSound;
        [SerializeField] private string hitVoice;

        [Header("Attack Sounds")] 
        [SerializeField] private string attackSound;
        [SerializeField] private string attackVoice;

        [Header("Dash sounds")] 
        [SerializeField] private string dashSound;

        [Header("Game Over state")] 
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
            Debug.Log($"HERE {movementState}");
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
            AkSoundEngine.PostEvent(hitSound, gameObject);
            AkSoundEngine.PostEvent(hitVoice, gameObject);
        }

        public void AttackSound()
        {
            AkSoundEngine.PostEvent(attackSound, gameObject);
            AkSoundEngine.PostEvent(attackVoice, gameObject);
        }

        public void DashSound()
        {
            AkSoundEngine.PostEvent(dashSound, gameObject);
        }

        public void DeathSound()
        {
            AkSoundEngine.SetState(gameOverState.GroupId, gameOverState.Id);
        }
    }
}
