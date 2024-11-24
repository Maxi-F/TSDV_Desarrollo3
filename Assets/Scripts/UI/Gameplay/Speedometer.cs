using System;
using System.Collections;
using Events;
using Events.ScriptableObjects;
using Input;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Gameplay
{
    public enum SpeedometerValues
    {
        Normal,
        Upwards,
        Downwards
    }
    
    public class Speedometer : MonoBehaviour
    {
        [Header("Velocity config")] 
        [SerializeField] private float multiplierFromRoads;
        
        [Header("Flicking Config")]
        [SerializeField] private Vector2 intervalSecondsRange;
        [SerializeField] private float flickDuration;
        
        [Header("Movement Config")]
        [SerializeField] private AnimationCurve downwardsValue;
        [SerializeField] private AnimationCurve upwardsValue;
        [SerializeField] private float movementDuration;
        
        [Header("Text Config")]
        [SerializeField] private TextMeshProUGUI text;

        [Header("Events")] 
        [SerializeField] private Vector3EventChannelSO onNewRoadsVelocityEvent;
        [SerializeField] private EventChannelSO<YMovementState> onYMovementStateChange;
        
        [Header("Metadata")]
        [SerializeField] private PauseSO pauseData;
        
        private bool _isFlicking;
        private bool _isMoving;
        private Coroutine _flickingCoroutine;
        private Coroutine _movementCoroutine;
        private int _textValue;
        private int _initTextValue;
        
        private void OnEnable()
        {
            _isFlicking = false;
            _isMoving = false;
            _textValue = Int32.Parse(text.text);
            _initTextValue = _textValue;
            
            onNewRoadsVelocityEvent?.onVectorEvent.AddListener(HandleNewStableValue);
            onYMovementStateChange?.onTypedEvent.AddListener(HanldeMovementChange);
        }

        private void OnDisable()
        {
            onNewRoadsVelocityEvent?.onVectorEvent.RemoveListener(HandleNewStableValue);
            onYMovementStateChange?.onTypedEvent.RemoveListener(HanldeMovementChange);
        }

        private void HandleNewStableValue(Vector3 newValue)
        {
            _initTextValue = (int)(newValue.z * multiplierFromRoads);
            _textValue = _initTextValue;
            text.text = _textValue.ToString();
        }

        private void HanldeMovementChange(YMovementState value)
        {
            switch (value)
            {
                case YMovementState.Downwards:
                    HandleMovementChange(downwardsValue);
                    break;
                case YMovementState.Upwards:
                    HandleMovementChange(upwardsValue);
                    break;
                case YMovementState.Normal:
                    if(_movementCoroutine != null)
                        StopCoroutine(_movementCoroutine);
                
                    _textValue = _initTextValue;
                    text.text = _textValue.ToString();
                    _isMoving = false;
                    _isFlicking = false;
                    break;
            }
        }

        private void HandleMovementChange(AnimationCurve valueCurve)
        {
            if(_flickingCoroutine != null)
                StopCoroutine(_flickingCoroutine);
            
            if(_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);

            _movementCoroutine = StartCoroutine(HandleMovementCoroutine(valueCurve));
        }

        private IEnumerator HandleMovementCoroutine(AnimationCurve valueCurve)
        {
            float startTime = Time.time;
            float timer = 0;

            while (timer < movementDuration)
            {
                timer = Time.time - startTime;

                float lessValue = valueCurve.Evaluate(timer / movementDuration);
                _textValue = _initTextValue + (int)lessValue;
                text.text = _textValue.ToString();
                
                yield return null;
            }
            
            _isMoving = false;
            _isFlicking = false;
        }

        private void Update()
        {
            if (_isFlicking || _isMoving) return;
            
            if(_flickingCoroutine != null)
                StopCoroutine(_flickingCoroutine);

            _isFlicking = true;
            _flickingCoroutine = StartCoroutine(HandleFlickingCoroutine());
        }

        private IEnumerator HandleFlickingCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(intervalSecondsRange.x, intervalSecondsRange.y));
            
            text.text = (_textValue + 1).ToString();

            yield return new WaitForSeconds(flickDuration);
            
            text.text = _textValue.ToString();
            
            _isFlicking = false;
        }
    }
}
