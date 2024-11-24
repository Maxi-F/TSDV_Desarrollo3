using System;
using UnityEngine;

namespace Minion
{
    public class MinionSoundController : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event floatSound;
        [SerializeField] private AK.Wwise.Event hitSound;
        [SerializeField] private AK.Wwise.Event weakSound;
        [SerializeField] private AK.Wwise.Event dieSound;
        [SerializeField] private AK.Wwise.Event attackSound;
        
        private void OnEnable()
        {
            floatSound.Post(gameObject);
        }

        private void OnDisable()
        {
            floatSound.Stop(gameObject);
            dieSound.Stop(gameObject);
            hitSound.Stop(gameObject);
            weakSound.Stop(gameObject);
        }

        public void HitSound(int currentHealth)
        {
            if (currentHealth > 0)
            {
                hitSound.Post(gameObject);
                weakSound.Post(gameObject);
            }
        }

        public void DieSound()
        {
            floatSound.Stop(gameObject);
            weakSound.Stop(gameObject);
            dieSound.Post(gameObject);
        }

        public void AttackSound()
        {
            attackSound.Post(gameObject);
        }
    }
}
