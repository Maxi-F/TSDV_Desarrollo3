using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerSoundController : MonoBehaviour
    {
        [Header("Pause Data")] 
        [SerializeField] private PauseSO pauseData;
        
        [Header("Motor sounds")]
        [SerializeField] private AK.Wwise.Event motorSound;
        
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
            pauseData?.onPause.AddListener(HandlePause);
            motorSound.Post(gameObject);
        }

        private void OnDisable()
        {
            pauseData?.onPause.RemoveListener(HandlePause);
        }

        private void HandlePause(bool isPaused)
        {
            Debug.Log("here?");
            if (isPaused)
                motorSound.Stop(gameObject);
            else
                motorSound.Post(gameObject);
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
