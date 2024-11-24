using UnityEngine;

namespace Events.ScriptableObjects
{
    public enum YMovementState
    {
        Normal,
        Upwards,
        Downwards
    }
    
    [CreateAssetMenu(menuName = "Events/YMovementState")]
    public class YMovementStateEvent : EventChannelSO<YMovementState>
    {
    }
}
