using System;
using System.Collections;
using Events;
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

        [Header("Events")] [SerializeField] private Vector3EventChannelSO onNewRoadsVelocityEvent;
        
        [Header("Metadata")]
        [SerializeField] private InputHandlerSO inputHandler;
        [SerializeField] private PauseSO pauseData;
        
        private bool _isFlicking;
        private bool _isMoving;
        private Coroutine _flickingCoroutine;
        private Coroutine _movementCoroutine;
        private int _textValue;
        private int _initTextValue;
        private SpeedometerValues _speedometerState;
        
        private void OnEnable()
        {
            _isFlicking = false;
            _isMoving = false;
            _textValue = Int32.Parse(text.text);
            _initTextValue = _textValue;
            _speedometerState = SpeedometerValues.Normal;
            
            inputHandler?.onPlayerMove.AddListener(HandleMovement);
            onNewRoadsVelocityEvent?.onVectorEvent.AddListener(HandleNewStableValue);
        }

        private void OnDisable()
        {
            inputHandler?.onPlayerMove.RemoveListener(HandleMovement);
            onNewRoadsVelocityEvent?.onVectorEvent.RemoveListener(HandleNewStableValue);
        }
        
        private void HandleNewStableValue(Vector3 newValue)
        {
            _initTextValue = (int)(newValue.z * multiplierFromRoads);
            _textValue = _initTextValue;
            text.text = _textValue.ToString();
        }

        private void HandleMovement(Vector2 movement)
        {
            Debug.Log(movement);
            if (movement.y < 0)
            {
                if(_speedometerState != SpeedometerValues.Downwards)
                    HandleMovementChange(downwardsValue);
                _speedometerState = SpeedometerValues.Downwards;
            } else if (movement.y > 0)
            {
                if(_speedometerState != SpeedometerValues.Upwards)
                    HandleMovementChange(upwardsValue);
                _speedometerState = SpeedometerValues.Upwards;
            }
            else
            {
                if(_movementCoroutine != null)
                    StopCoroutine(_movementCoroutine);
                
                _textValue = _initTextValue;
                text.text = _textValue.ToString();
                _speedometerState = SpeedometerValues.Normal;
                _isMoving = false;
                _isFlicking = false;
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
