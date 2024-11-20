using System;
using System.Collections;
using Attacks.Swing;
using UnityEngine;
using Utils;

namespace Enemy.Attacks
{
    public class SwingAttack : EnemyController, IEnemyAttack
    {
        [SerializeField] private Swing swing;
        [SerializeField] private Material laserAttackMaterial;

        [Header("Anim Config")] 
        [SerializeField] private float preAttackDelay;
        [SerializeField] private float initialDelay;
        
        private bool _canStartAttack;
        private Material _startBodyMaterial;
        
        public bool CanExecute()
        {
            return true;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(preAttackDelay);
            animationHandler.StartLaserAnimation();
            yield return new WaitForSeconds(initialDelay);
            enemyAgent.ChangeStateToLaser();
            yield return CreateLaserSequence().Execute();
            enemyAgent.ChangeStateToIdle();
        }

        private Sequence CreateLaserSequence()
        {
            Sequence laserSequence = new Sequence();
            
            //laserSequence.AddPreAction(ChangeBodyMaterial(laserAttackMaterial));
            laserSequence.SetAction(StartAttack());
            //laserSequence.AddPostAction(ChangeBodyMaterial(_startBodyMaterial));

            return laserSequence;
        }

        // private IEnumerator ChangeBodyMaterial(Material newMaterial)
        // {
        //     bodyMeshRenderer.material = newMaterial;
        //     yield return null;
        // }
        
        private IEnumerator StartAttack()
        {
            yield return swing.RunSwing();
        }
    }
}
