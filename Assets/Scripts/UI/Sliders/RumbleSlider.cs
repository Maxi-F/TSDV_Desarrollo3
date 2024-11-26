using UnityEngine;
using UnityEngine.UI;

namespace UI.Sliders
{
    public class RumbleSlider : MonoBehaviour
    {
        [SerializeField] private string rumblePrefId;
        [SerializeField] private string modifiedFlagId;
        [SerializeField] private Slider slider;
        
        private void OnEnable()
        {
            float rumblePrefValue = PlayerPrefs.GetFloat(rumblePrefId);

            if (rumblePrefValue != 0)
            {
                slider.value = rumblePrefValue;
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
            PlayerPrefs.SetFloat(rumblePrefId, sliderValue);
        }
    }
}
