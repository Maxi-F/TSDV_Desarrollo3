using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class RumbleController : MonoBehaviour
    {
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
            Gamepad.current.SetMotorSpeeds(lowFrequency, lowFrequency);
            yield return new WaitForSecondsRealtime(rumbleDuration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
