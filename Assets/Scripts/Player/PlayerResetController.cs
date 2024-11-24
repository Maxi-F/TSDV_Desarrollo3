using System.Collections;
using Events;
using Health;
using Player.Animation;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerResetController : MonoBehaviour
    {
        [SerializeField] private Vector3 initPosition;
        [SerializeField] private HealthPoints healthPoints;
        [SerializeField] private PlayerDashController dashController;
        [SerializeField] private float secondsUntilDisable;
        [SerializeField] private float explosionSeconds;
        [SerializeField] private GameObject explosionObject;
        [SerializeField] private GameObject model;
        [SerializeField] private PlayerAnimationHandler animatorHandler;
        
        [Header("Events")] 
        [SerializeField] private VoidEventChannelSO onPlayerDeathEvent;
        [SerializeField] private VoidEventChannelSO onGameplayResetEvent;
        [SerializeField] private UnityEvent onExplosion;
        
        private Coroutine _disablePlayerCoroutine;
        
        private void Awake()
        {
            onGameplayResetEvent?.onEvent.AddListener(HandleResetPlayer);
        }
        
        private void OnEnable()
        {
            onPlayerDeathEvent?.onEvent.AddListener(DisablePlayer);
        }

        protected void OnDisable()
        {
            onPlayerDeathEvent?.onEvent.RemoveListener(DisablePlayer);
        }

        private void OnDestroy()
        {
            onGameplayResetEvent?.onEvent.RemoveListener(HandleResetPlayer);
        }

        private void HandleResetPlayer()
        {
            if(_disablePlayerCoroutine != null)
                StopCoroutine(_disablePlayerCoroutine);
            
            animatorHandler.HandleResetAnimator();
            model.gameObject.SetActive(true);
            healthPoints.ResetHitPoints();
            dashController.ResetDash();
            transform.position = initPosition;
            gameObject.SetActive(true);
        }

        private void DisablePlayer()
        {
            if(_disablePlayerCoroutine != null)
                StopCoroutine(_disablePlayerCoroutine);
            
            _disablePlayerCoroutine = StartCoroutine(DisablePlayerCoroutine());
        }

        private IEnumerator DisablePlayerCoroutine()
        {
            yield return new WaitForSeconds(secondsUntilDisable);
            onExplosion?.Invoke();
            explosionObject.gameObject.SetActive(true);
            model.gameObject.SetActive(false);
            yield return new WaitForSeconds(explosionSeconds);
            gameObject.SetActive(false);
        }
    }
}
