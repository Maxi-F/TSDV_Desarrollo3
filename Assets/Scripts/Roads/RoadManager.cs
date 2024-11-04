using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;

namespace Roads
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private GameObject startingLastRoad;
        [SerializeField] private int initRoadCount = 7;
        [SerializeField] private int maxRoads = 7;
        [SerializeField] private List<GameObject> initRoads;
        
        [Header("Roads velocity config")]
        [SerializeField] private Vector3 roadsInitVelocity = new Vector3(0f, 0f, -20f);
        [SerializeField] private AnimationCurve roadsAccelerationCurve;
        [SerializeField] private float accelerationDuration;
        
        [Header("Events")]
        [SerializeField] private GameObjectEventChannelSO onRoadDeleteTriggerEvent;
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private Vector3EventChannelSO onNewVelocityEvent;
        [SerializeField] private Vector3EventChannelSO onNewRoadManagerVelocity;
        
        private int _roadCount;
        private List<GameObject> _roads = new List<GameObject>();
        private Vector3 _roadsVelocity;
        private GameObject _lastRoad;

        public void OnEnable()
        {
            _roadsVelocity = roadsInitVelocity;
            _roadCount = initRoadCount;
            _lastRoad = startingLastRoad;
            _roads = new List<GameObject>();
            
            foreach (var initRoad in initRoads)
            {
                initRoad.SetActive(true);
                _roads.Add(initRoad);
            }
            
            HandleNewVelocity(roadsInitVelocity);
            onNewRoadManagerVelocity?.onVectorEvent.AddListener(HandleNewVelocity);
            onRoadDeleteTriggerEvent?.onGameObjectEvent.AddListener(HandleDeleteRoad);
        }

        public void OnDisable()
        {
            onNewRoadManagerVelocity?.onVectorEvent.RemoveListener(HandleNewVelocity);
            onRoadDeleteTriggerEvent?.onGameObjectEvent.RemoveListener(HandleDeleteRoad);
        }

        private void HandleRemoveRoads()
        {
            foreach (var road in _roads.ToList())
            {
                _roads.Remove(road);
                RoadObjectPool.Instance?.ReturnToPool(road);
            }
        }

        public void HandleNewVelocity(Vector3 velocity)
        {
            StartCoroutine(HandleVelocityCoroutine(velocity));
        }

        private IEnumerator HandleVelocityCoroutine(Vector3 endVelocity)
        {
            float timer = 0;
            Vector3 actualVelocity = _roadsVelocity;
            float startingTime = Time.time;

            while (timer < accelerationDuration)
            {
                timer = Time.time - startingTime;

                float velocityTime = roadsAccelerationCurve.Evaluate(timer / accelerationDuration);
                _roadsVelocity = Vector3.Lerp(actualVelocity, endVelocity, velocityTime);
                onNewVelocityEvent?.RaiseEvent(_roadsVelocity);
                yield return null;
            }

            _roadsVelocity = endVelocity;
            onNewVelocityEvent?.RaiseEvent(_roadsVelocity);
        }

        private void HandleDeleteRoad(GameObject road)
        {
            _roadCount--;
            _roads.Remove(road);
            HandleNewRoad();
        }

        private void HandleNewRoad()
        {
            if (_roadCount > maxRoads) return;
            
            RoadEnd roadEnd = _lastRoad.GetComponentInChildren<RoadEnd>();

            GameObject newLastRoad = RoadObjectPool.Instance?.GetPooledObject();
            if (newLastRoad == null)
            {
                Debug.LogError("new last road was null!");
                return;
            }

            newLastRoad.transform.position = roadEnd.GetRoadEnd().position + _roadsVelocity * Time.deltaTime;
            newLastRoad.SetActive(true);
            _roads.Add(newLastRoad);
            
            Movement roadMovement = newLastRoad.GetComponentInChildren<Movement>();
            roadMovement.SetVelocity(_roadsVelocity);
            
            onRoadInstantiatedEvent?.RaiseEvent(newLastRoad);
            
            _lastRoad = newLastRoad;

            _roadCount++;
        }
    }
}
