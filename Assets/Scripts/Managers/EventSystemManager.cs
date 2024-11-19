using System;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemManager : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onNewPreselectedObjectEvent;
        private EventSystem _eventSystem;
        
        private void OnEnable()
        {
            _eventSystem ??= GetComponent<EventSystem>();
            
            onNewPreselectedObjectEvent?.onGameObjectEvent.AddListener(HandleNewPreselectedObject);
        }

        private void OnDisable()
        {
            onNewPreselectedObjectEvent?.onGameObjectEvent.RemoveListener(HandleNewPreselectedObject);
        }

        private void HandleNewPreselectedObject(GameObject newObject)
        {
            _eventSystem.SetSelectedGameObject(newObject);
        }
    }
}
