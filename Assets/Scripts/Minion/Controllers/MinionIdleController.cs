using System;
using System.Collections;
using Events;
using Events.ScriptableObjects;
using Minion.Manager;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionIdleController : MinionController
    {
        public void Enter()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}