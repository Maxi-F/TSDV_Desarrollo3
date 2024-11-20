using UnityEngine;

namespace Enemy
{
    public class EnemySoundController : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.State creditsState;

        public void WonSound()
        {
            AkSoundEngine.SetState(creditsState.GroupId, creditsState.Id);
        }
    }
}
