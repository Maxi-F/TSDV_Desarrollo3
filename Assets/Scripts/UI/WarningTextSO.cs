using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "UI/Warning")]
    public class WarningTextSO : ScriptableObject
    {
        public float duration;
        public float intervalSeconds;
    }
}
