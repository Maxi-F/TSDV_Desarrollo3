using System.Collections;
using Enemy.Shield;
using Events.ScriptableObjects;
using Health;
using ParryProjectile;
using UnityEngine;
using UnityEngine.Events;

namespace Attacks.ParryProjectile
{
    public class ParryBomb : MonoBehaviour, IDeflectable
    {
        [SerializeField] private float deflectionCooldownStartTime = 0.2f;
        
        [Header("Second Force Properties")]
        [SerializeField] private float secondForceAcceleration;

        [SerializeField] private float secondsInFollowForce;

        [Header("Damage properties")]
        [SerializeField] private int damage = 5;

        [SerializeField] private int shieldDamage = 1;

        [Header("Parry Properties")] [SerializeField]
        private float startVelocityInParry;

        [Header("Events")]
        [SerializeField] private EventChannelSO<bool> onParryFinished;
        [SerializeField] private EventChannelSO<bool> onParried;

        [Header("Internal Events")] 
        [SerializeField] private UnityEvent onDestroyParryBombEvent;
        
        public bool IsActive { get; set; }
        
        private GameObject _firstObjectToFollow;
        private Vector3 _targetPosition;
        private bool _isTargetingPlayer;
        private Rigidbody _rigidbody;
        private ParryProjectileFirstForce _parryProjectileConfig;

        private float _timeApplyingFollowForce = 0f;
        private bool _isStarted = false;
        private bool _canCollission;
        private bool _canDeflect;
        private float _followForceVelocity;

        private Vector3 _direction;
        
        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
             IsActive = true;
             _canCollission = true;
             _canDeflect = false;
            _isTargetingPlayer = true;

            StartCoroutine(HandleDeflectionCooldown());
        }

        private void FixedUpdate()
        {
            if (!_isStarted)
            {
                StartCoroutine(ApplyAngularForce());
                _isStarted = true;
            }
        }
        
        public void SetFirstForce(ParryProjectileFirstForce config)
        {
            _parryProjectileConfig = config;
            SetTargetPosition(config.finalPosition);
        }
        
        private IEnumerator HandleDeflectionCooldown()
        {
            yield return new WaitForSeconds(deflectionCooldownStartTime);
            _canDeflect = true;
        }

        private IEnumerator ApplyAngularForce()
        {
            _rigidbody.AddForce(_parryProjectileConfig.startImpulse, ForceMode.Impulse);

            float timeApplyingAngularForce = 0f;

            while (timeApplyingAngularForce < _parryProjectileConfig.secondsInAngularVelocity)
            {
                _rigidbody.AddForce(_parryProjectileConfig.angularForce, ForceMode.Acceleration);

                timeApplyingAngularForce += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            _followForceVelocity = (_rigidbody.velocity.magnitude * Time.deltaTime) / Time.fixedDeltaTime;
            StartCoroutine(ApplyTargetedForce(_targetPosition));
        }

        private IEnumerator ApplyTargetedForce(Vector3 targetPosition)
        {
            SetDirection(_targetPosition);

            while (_timeApplyingFollowForce < secondsInFollowForce)
            {
                transform.position += _direction * (_followForceVelocity * Time.deltaTime);

                _timeApplyingFollowForce += Time.deltaTime;
                _followForceVelocity += secondForceAcceleration * Time.deltaTime;

                yield return null;
            }

            if (_canCollission)
            {
                _canCollission = false;     
                onParryFinished.RaiseEvent(false);
                onDestroyParryBombEvent?.Invoke();
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canCollission) return;
            
            if (other.CompareTag("Enemy") && !_isTargetingPlayer)
            {
                ShieldController enemy = other.GetComponentInChildren<ShieldController>();

                if (enemy.TryDestroyShield(shieldDamage))
                {
                    _canCollission = false;
                    onParryFinished.RaiseEvent(true);
                    onDestroyParryBombEvent?.Invoke();
                    return;
                }

                other.GetComponent<EnemyAgent>().ChangeStateToBombParry();
                Deflect(_firstObjectToFollow);
                return;
            }

            if (other.CompareTag("Player") && _isTargetingPlayer)
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();

                _canCollission = false;
                damageTaker.TryTakeDamage(damage);
                onParryFinished.RaiseEvent(false);
                onDestroyParryBombEvent?.Invoke();
            }
        }

        public void SetFirstObjectToFollow(GameObject newFirstObjectToFollow)
        {
            _firstObjectToFollow = newFirstObjectToFollow;
            _isTargetingPlayer = true;
        }

        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition = position;
        }

        public void Deflect(GameObject newObjectToFollow)
        {
            if (!_canDeflect) return;
            
            _timeApplyingFollowForce = 0f;
            _rigidbody.velocity = GetDirection(newObjectToFollow.transform.position) * startVelocityInParry;

            _isTargetingPlayer = !_isTargetingPlayer;
            onParried?.RaiseEvent(_isTargetingPlayer);
            SetTargetPosition(newObjectToFollow.transform.position);
            SetDirection(_targetPosition);
        }

        private void SetDirection(Vector3 position)
        {
            _direction = GetDirection(position);
        }

        private Vector3 GetDirection(Vector3 to)
        {
            Vector3 direction = (to - gameObject.transform.position).normalized;

            return direction;
        }

        private void OnDrawGizmos()
        {
            if (_rigidbody != null)
                Gizmos.DrawLine(transform.position, _rigidbody.velocity);
        }
    }
}