using UnityEngine;

namespace Attacks.FallingAttack
{
    [CreateAssetMenu(fileName = "FallingAttackConfig", menuName = "Enemy/Attacks/FallingAttackConfig", order = 0)]
    public class FallingBlockSO : ScriptableObject
    {
        public Vector3 initPosition;
        public GameObject fallingBlockPrefab;
    }
}
