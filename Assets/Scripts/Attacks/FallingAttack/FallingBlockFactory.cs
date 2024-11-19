using ObjectPool.Runtime;
using UnityEngine;

namespace Attacks.FallingAttack
{
    public class FallingBlockFactory : IFactory<FallingBlockSO>
    {
        private FallingBlockSO _fallingBlockConfig;
    
        public void SetConfig(FallingBlockSO config)
        {
            _fallingBlockConfig = config;
        }

        public GameObject CreateObject()
        {
            GameObject fallingBlockInstance = Object.Instantiate(_fallingBlockConfig.fallingBlockPrefab);
            fallingBlockInstance.transform.position = _fallingBlockConfig.initPosition;

            return fallingBlockInstance;
        }
    }
}
