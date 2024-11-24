using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Sliders
{
    public class WwiseSlider : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.RTPC rtpc;
        [SerializeField] private string prefsId;
        [SerializeField] private string modifiedFlagId;
        [SerializeField] private Slider slider;

        [SerializeField] private AK.Wwise.Event sliderSound;
        
        private void OnEnable()
        {
            float masterVolumePref = PlayerPrefs.GetFloat(prefsId);
            bool hasBeenModified = PlayerPrefs.GetInt(modifiedFlagId) == 1;
            
            if (hasBeenModified)
            {
                slider.value = masterVolumePref;
            }
            
            slider.onValueChanged.AddListener(SetNewSliderValue);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(SetNewSliderValue);
        }

        private void SetNewSliderValue(float sliderValue)
        {
            PlayerPrefs.SetInt(modifiedFlagId, 1);
            
            AkSoundEngine.SetRTPCValue(rtpc.Name, sliderValue);
            PlayerPrefs.SetFloat(prefsId, sliderValue);

            AkSoundEngine.PostEvent(sliderSound.Id, gameObject);
        }
    }
}