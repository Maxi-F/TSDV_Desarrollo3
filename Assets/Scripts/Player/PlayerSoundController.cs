using System;
using UnityEngine;

namespace Player
{
    public class PlayerSoundController : MonoBehaviour
    {
        [SerializeField] private string motorSound;
        
        [Header("Hit Sounds")]
        [SerializeField] private string hitSound;
        [SerializeField] private string hitVoice;

        [Header("Attack Sounds")] 
        [SerializeField] private string attackSound;
        [SerializeField] private string attackVoice;

        [Header("Dash sounds")] 
        [SerializeField] private string dashSound;

        [Header("Game Over state")] [SerializeField]
        private AK.Wwise.State gameOverState;
        
        private void OnEnable()
        {
            AkSoundEngine.PostEvent(motorSound, gameObject);
        }

        public void HitSound()
        {
            AkSoundEngine.PostEvent(hitSound, gameObject);
            AkSoundEngine.PostEvent(hitVoice, gameObject);
        }

        public void AttackSound()
        {
            AkSoundEngine.PostEvent(attackSound, gameObject);
            AkSoundEngine.PostEvent(attackVoice, gameObject);
        }

        public void DashSound()
        {
            AkSoundEngine.PostEvent(dashSound, gameObject);
        }

        public void DeathSound()
        {
            AkSoundEngine.SetState(gameOverState.GroupId, gameOverState.Id);
        }
    }
}
