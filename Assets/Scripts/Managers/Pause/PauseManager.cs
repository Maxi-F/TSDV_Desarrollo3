using Events;
using Events.ScriptableObjects;
using Input;
using UnityEngine;

namespace Managers.Pause
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;
        [SerializeField] private PauseSO pauseData;
        
        [Header("Events")]
        [SerializeField] private BoolEventChannelSO onHandlePauseEvent;
        [SerializeField] private VoidEventChannelSO onCinematicStarted;
        [SerializeField] private VoidEventChannelSO onCinematicEnded;
        
        private float _lastTimeScale;
        private bool _isInCinematic;
        private void OnEnable()
        {
            pauseData.isPaused = false;
            _isInCinematic = false;

            onCinematicStarted?.onEvent.AddListener(HandleInCinematic);
            onCinematicEnded?.onEvent.AddListener(HandleOutOfCinematic);
            onHandlePauseEvent?.onTypedEvent.AddListener(HandlePause);
            inputHandler?.onPauseToggle.AddListener(HandlePause);
        }

        private void OnDisable()
        {
            onCinematicStarted?.onEvent.RemoveListener(HandleInCinematic);
            onCinematicEnded?.onEvent.RemoveListener(HandleOutOfCinematic);
            onHandlePauseEvent?.onTypedEvent.RemoveListener(HandlePause);
            inputHandler?.onPauseToggle.RemoveListener(HandlePause);
        }
        
        private void HandleOutOfCinematic()
        {
            _isInCinematic = false;
        }

        private void HandleInCinematic()
        {
            _isInCinematic = true;
        }

        private void HandlePause(bool value)
        {
            pauseData.isPaused = value;

            if (pauseData.isPaused)
            {
                _lastTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = _lastTimeScale;
            }
        }

        private void HandlePause()
        {
            if(!pauseData.isPaused && !_isInCinematic)
                onHandlePauseEvent.RaiseEvent(true);
        }
    }
}
