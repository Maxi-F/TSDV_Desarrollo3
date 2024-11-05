using Camera;
using UnityEngine;

namespace LevelManagement.Sequences.Data
{
    [CreateAssetMenu(menuName = "LevelData/Start")]
    public class StartLevelSO : ScriptableObject
    {
        public float playersVelocity;
        public float otherPlayersEndZPosition;
        public float playerInitZPosition;
        public CameraSO startCameraData;
        public bool isFirstGameplay;
    }
}
