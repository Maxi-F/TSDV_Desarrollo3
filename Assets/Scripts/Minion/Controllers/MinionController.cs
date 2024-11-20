using System;
using Health;
using UnityEngine;

namespace Minion.Controllers
{
    [RequireComponent(typeof(MinionAgent))]
    public abstract class MinionController : MonoBehaviour
    {
        [HideInInspector] public GameObject target;
        protected MinionAgent minionAgent;

        protected HealthPoints _healthPoints;
        protected Collider _collider;

        public void LookAtTarget()
        {
            Vector3 targetPos = target.transform.position;
            targetPos.y = minionAgent.GetModel().transform.position.y;
            minionAgent.GetModel().transform.LookAt(targetPos, Vector3.up);
        }

        protected virtual void OnEnable()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            _collider ??= GetComponent<Collider>();
            minionAgent ??= GetComponent<MinionAgent>();

            target = minionAgent.GetPlayer();
        }
    }
}