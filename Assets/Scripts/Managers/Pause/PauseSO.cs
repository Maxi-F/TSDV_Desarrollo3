using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    [CreateAssetMenu(menuName = "Pause/Data")]
    public class PauseSO : ScriptableObject
    {
        public bool isPaused;
        public UnityEvent<bool> onPause;
        
        public void SetIsPaused(bool value)
        {
            isPaused = value;
            onPause?.Invoke(value);
        }
    }
}
