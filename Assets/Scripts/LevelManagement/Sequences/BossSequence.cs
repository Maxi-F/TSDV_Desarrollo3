using System.Collections;
using Attacks.FallingAttack;
using Enemy;
using UnityEngine;
using Utils;

namespace LevelManagement.Sequences
{
    public class BossSequence : MonoBehaviour
    {
        [SerializeField] private FallingBlockSpawner fallingBlockSpawner;
        [SerializeField] private EnemyConfigSO enemyConfig;
        [Header("Game Objects")]
        [SerializeField] private GameObject boss;

        private AK.Wwise.State bossMusicState;
        
        public void SetupSequence(BossData bossData)
        {
            bossMusicState = bossData.minionsToBossState;
            boss.SetActive(false);
            boss.transform.position = enemyConfig.defaultPosition;
            fallingBlockSpawner.SetFallingAttackData(bossData.fallingAttackData);
        }
        
        public IEnumerator StartBossBattle()
        {
            Sequence sequence = new Sequence();

            sequence.SetAction(BossBattleAction());

            return sequence.Execute();
        }
        
        private IEnumerator BossBattleAction()
        {
            yield return null;
            AkSoundEngine.SetState(bossMusicState.GroupId, bossMusicState.Id);
            boss.SetActive(true);
        }

        public void ClearSequence()
        {
            boss.SetActive(false);
        }
    }
}
