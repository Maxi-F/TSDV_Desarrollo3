using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Sliders
{
    [RequireComponent(typeof(Slider))]
    public class SliderSound : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event sliderSound;
        [SerializeField] private float soundCooldown = 0.1f;
        
        private Slider _slider;
        private bool _shouldMakeSound;
        private Coroutine _startSoundCoroutine;
        
        private void OnEnable()
        {
            _slider ??= GetComponent<Slider>();
            _shouldMakeSound = true;    
            _slider?.onValueChanged.AddListener(HandleSliderSound);
        }

        private void OnDisable()
        {
            _slider?.onValueChanged.RemoveListener(HandleSliderSound);
        }
        
        private void HandleSliderSound(float value)
        {
            if (_shouldMakeSound)
            {
                sliderSound.Post(gameObject);
                _shouldMakeSound = false;
            }
            
            if(_startSoundCoroutine != null)
                StopCoroutine(_startSoundCoroutine);

            _startSoundCoroutine = StartCoroutine(HandleCanMakeSound());
        }

        private IEnumerator HandleCanMakeSound()
        {
            yield return new WaitForSeconds(soundCooldown);
            _shouldMakeSound = true;
        }
    }
}
