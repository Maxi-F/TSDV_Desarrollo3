using System.Collections.Generic;
using UnityEngine;

namespace Roads.Walls.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WallConfig", menuName = "Roads/Walls/MiddleWallConfig", order = 0)]
    public class MiddleWallConfigSO : ScriptableObject
    {
        public List<WallSO> walls;
    }
}