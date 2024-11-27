using UnityEngine;

namespace Sounds
{
    public class SfxStopper : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event stopAllSfxEvent;

        public void StopAllSfxs()
        {
            stopAllSfxEvent.Post(gameObject);
        }
    }
}
