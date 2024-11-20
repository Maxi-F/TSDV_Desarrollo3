using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// rumbleMultiplier rumbleFlag
namespace Utils
{
    public class RumbleController : MonoBehaviour
    {
        [SerializeField] private string rumblePref;
        [SerializeField] private string rumbleFlagPref;
        [SerializeField] private float defaultRumbleMultiplier = 1.0f;
        private Coroutine _isRumbling;
        
        public void StartRumble(RumbleControllerData rumbleControllerData)
        {
            if (_isRumbling != null)
            {
                StopCoroutine(_isRumbling);
            }

            if (Gamepad.current != null)
            {
                _isRumbling = StartCoroutine(RumbleCoroutine(rumbleControllerData.duration, rumbleControllerData.forceAmount));
            }
        }

        private IEnumerator RumbleCoroutine(float rumbleDuration, float lowFrequency)
        {
            float rumbleMultiplier = PlayerPrefs.GetFloat(rumblePref);
            bool rumbleFlag = PlayerPrefs.GetInt(rumbleFlagPref) == 1;
            float multiplier = rumbleFlag ? rumbleMultiplier : defaultRumbleMultiplier;
            
            Gamepad.current.SetMotorSpeeds(lowFrequency * multiplier, lowFrequency * multiplier);
            yield return new WaitForSecondsRealtime(rumbleDuration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
