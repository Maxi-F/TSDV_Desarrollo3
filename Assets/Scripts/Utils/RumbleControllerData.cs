using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(menuName = "Rumble data")]
    public class RumbleControllerData : ScriptableObject
    {
        public float duration;
        public float forceAmount;
    }
}