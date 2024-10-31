using ObjectPool.Runtime;
using Roads.Walls.ScriptableObjects;
using UnityEngine;

namespace Roads.Walls
{
    public class CornerWallFactory : IFactory<CornerWallsConfigSO>
    {
        private CornerWallsConfigSO _config;
        private int _index = 0;

        public void SetConfig(CornerWallsConfigSO config)
        {
            _config = config;
        }

        public GameObject CreateObject()
        {
            GameObject newLastWall = Object.Instantiate(_config.walls[_index].wallObject);

            _index++;
            if (_index >= _config.walls.Count) _index = 0;

            return newLastWall;
        }
    }
}