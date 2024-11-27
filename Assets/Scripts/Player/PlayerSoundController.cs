using System;
using System.Collections;
using Events;
using Events.ScriptableObjects;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerSoundController : MonoBehaviour
    {
        [Header("Motor")]
        [SerializeField] private AK.Wwise.Event motorSound;
        [SerializeField] private AK.Wwise.RTPC motorRTPC;
        [SerializeField] private AnimationCurve motorSoundCurve;
        [SerializeField] private float motorChangeDuration;
        [SerializeField] private float initMotorValue;
        [SerializeField] private float motorDiff;
        [SerializeField] private EventChannelSO<YMovementState> onMovementStateChange;
        [SerializeField] private FloatEventChannelSO onNewMotorValue;
        
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
        private Coroutine _motorValueChangeCoroutine;
        
        private void Start()
        {
            AkSoundEngine.SetRTPCValue(motorRTPC.Id, _motorValue);
        }

        private void OnEnable()
        {
            _motorValue = initMotorValue;
            onMovementStateChange?.onTypedEvent.AddListener(HandleMovementChange);
            onNewMotorValue?.onFloatEvent.AddListener(HandleNewMotorValue);
            motorSound.Post(gameObject);
        }

        private void OnDisable()
        {
            onMovementStateChange?.onTypedEvent.RemoveListener(HandleMovementChange);
            onNewMotorValue?.onFloatEvent.RemoveListener(HandleNewMotorValue);
        }
        
        private void HandleNewMotorValue(float newMotorValue)
        {
            if(_motorValueChangeCoroutine != null)
                StopCoroutine(_motorValueChangeCoroutine);

            _motorValueChangeCoroutine = StartCoroutine(ChangeMotorValue(newMotorValue));
        }

        private IEnumerator ChangeMotorValue(float newMotorValue)
        {
            float startValue = _motorValue;
            float startTime = Time.time;
            float passedTime = 0;

            while (passedTime < motorChangeDuration)
            {
                passedTime = Time.time - startTime;
                float valueChangePercentage = motorSoundCurve.Evaluate(passedTime / motorChangeDuration);
                _motorValue = Mathf.Lerp(startValue, newMotorValue, valueChangePercentage);
                AkSoundEngine.SetRTPCValue(motorRTPC.Id, _motorValue);
                yield return null;
            }

            _motorValue = newMotorValue;
        }

        private void HandleMovementChange(YMovementState movementState)
        {
            AkSoundEngine.SetRTPCValue(motorRTPC.Id,
                movementState == YMovementState.Upwards ? _motorValue + motorDiff :
                movementState == YMovementState.Downwards ? _motorValue - motorDiff : _motorValue);
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
