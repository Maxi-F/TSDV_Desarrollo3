using System.Collections.Generic;
using UnityEngine;

namespace Roads.Walls.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WallConfig", menuName = "Roads/Walls/CornerWallConfig", order = 0)]
    public class CornerWallsConfigSO : ScriptableObject
    {
        public List<WallSO> walls;
    }
}