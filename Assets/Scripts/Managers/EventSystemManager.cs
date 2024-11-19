using System;
using Events;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemManager : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onNewPreselectedObjectEvent;
        [SerializeField] private InputHandlerSO inputHandler;
        
        private EventSystem _eventSystem;
        private GameObject _lastPreselectedObject;
        
        private void OnEnable()
        {
            _eventSystem ??= GetComponent<EventSystem>();
            
            inputHandler?.onNavigation.AddListener(HandlePreselectFirstObject);
            onNewPreselectedObjectEvent?.onGameObjectEvent.AddListener(HandleNewPreselectedObject);
        }

        private void OnDisable()
        {
            inputHandler?.onNavigation.RemoveListener(HandlePreselectFirstObject);
            onNewPreselectedObjectEvent?.onGameObjectEvent.RemoveListener(HandleNewPreselectedObject);
        }

        private void HandlePreselectFirstObject()
        {
            if (_eventSystem.currentSelectedGameObject == null)
            {
                _eventSystem.SetSelectedGameObject(_lastPreselectedObject);
            }
        }

        private void HandleNewPreselectedObject(GameObject newObject)
        {
            _eventSystem.SetSelectedGameObject(newObject);
            _lastPreselectedObject = newObject;
        }
    }
}
