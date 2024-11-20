using ObjectPool.Runtime;
using UnityEngine;

namespace Attacks.FallingAttack
{
    public class FallingBlockFactory : IFactory<FallingBlockSO>
    {
        private FallingBlockSO _fallingBlockConfig;

        private int _index = 0;
        
        public void SetConfig(FallingBlockSO config)
        {
            _fallingBlockConfig = config;
        }

        public GameObject CreateObject()
        {
            GameObject fallingBlockInstance = Object.Instantiate(_fallingBlockConfig.fallingBlockPrefabs[_index]);
            fallingBlockInstance.transform.position = _fallingBlockConfig.initPosition;

            _index++;
            if (_index >= _fallingBlockConfig.fallingBlockPrefabs.Length) _index = 0;
            return fallingBlockInstance;
        }
    }
}
