using System.Collections;
using System.Collections.Generic;
using FSM;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAgent : Agent
{
    [SerializeField] private HealthPoints healthPoints;

    [Header("Internal Events")]
    [SerializeField] private ActionEventsWrapper idleEvents;
    [SerializeField] private ActionEventsWrapper moveEvents;
    [SerializeField] private ActionEventsWrapper laserEvents;
    [SerializeField] private ActionEventsWrapper bombThrowEvents;
    [SerializeField] private ActionEventsWrapper bombParryEvents;
    [SerializeField] private ActionEventsWrapper debrisThrowEvents;

    private State _idleState;
    private State _weakenedState;
    private State _laserState;
    private State _bombThrowState;
    private State _bombParryState;
    private State _debrisThrowState;

    public void ChangeStateToIdle()
    {
        Fsm.ChangeState(_idleState);
    }

    public void ChangeStateToWeakened()
    {
        Fsm.ChangeState(_weakenedState);
    }

    public void ChangeStateToLaser()
    {
        Fsm.ChangeState(_laserState);
    }

    public void ChangeStateToBombThrow()
    {
        Fsm.ChangeState(_bombThrowState);
    }

    public void ChangeStateToBombParry()
    {
        Fsm.ChangeState(_bombParryState);
    }

    public void ChangeStateToDebrisThrow()
    {
        Fsm.ChangeState(_debrisThrowState);
    }

    protected override List<State> GetStates()
    {
        #region States

        _idleState = new State();
        _idleState.EnterAction += idleEvents.ExecuteOnEnter;
        _idleState.UpdateAction += idleEvents.ExecuteOnUpdate;
        _idleState.ExitAction += idleEvents.ExecuteOnExit;

        _weakenedState = new State();
        _weakenedState.EnterAction += moveEvents.ExecuteOnEnter;
        _weakenedState.UpdateAction += moveEvents.ExecuteOnUpdate;
        _weakenedState.ExitAction += moveEvents.ExecuteOnExit;

        _laserState = new State();
        _laserState.EnterAction += laserEvents.ExecuteOnEnter;
        _laserState.UpdateAction += laserEvents.ExecuteOnUpdate;
        _laserState.ExitAction += laserEvents.ExecuteOnExit;

        _bombThrowState = new State();
        _bombThrowState.EnterAction += bombThrowEvents.ExecuteOnEnter;
        _bombThrowState.UpdateAction += bombThrowEvents.ExecuteOnUpdate;
        _bombThrowState.ExitAction += bombThrowEvents.ExecuteOnExit;

        _bombParryState = new State();
        _bombParryState.EnterAction += bombParryEvents.ExecuteOnEnter;
        _bombParryState.UpdateAction += bombParryEvents.ExecuteOnUpdate;
        _bombParryState.ExitAction += bombParryEvents.ExecuteOnExit;

        _debrisThrowState = new State();
        _debrisThrowState.EnterAction += debrisThrowEvents.ExecuteOnEnter;
        _debrisThrowState.UpdateAction += debrisThrowEvents.ExecuteOnUpdate;
        _debrisThrowState.ExitAction += debrisThrowEvents.ExecuteOnExit;

        #endregion

        Transition idleToWeakenedTransition = new Transition(_idleState, _weakenedState);
        _idleState.AddTransition(idleToWeakenedTransition);

        Transition idleToLaserTransition = new Transition(_idleState, _laserState);
        _laserState.AddTransition(idleToLaserTransition);

        Transition laserToIdleTransition = new Transition(_laserState, _idleState);
        _laserState.AddTransition(laserToIdleTransition);

        Transition idleToBombThrowTransition = new Transition(_idleState, _bombThrowState);
        _laserState.AddTransition(idleToBombThrowTransition);

        Transition bombThrowToIdleTransition = new Transition(_bombThrowState, _idleState);
        _laserState.AddTransition(bombThrowToIdleTransition);

        Transition idleToBombParryTransition = new Transition(_idleState, _bombParryState);
        _laserState.AddTransition(idleToBombParryTransition);

        Transition bombParryToIdleTransition = new Transition(_bombParryState, _idleState);
        _laserState.AddTransition(bombParryToIdleTransition);

        Transition bombParryToWeakenedTransition = new Transition(_bombParryState, _weakenedState);
        _laserState.AddTransition(bombParryToWeakenedTransition);

        Transition weakenedToIdleTransition = new Transition(_weakenedState, _idleState);
        _weakenedState.AddTransition(weakenedToIdleTransition);

        return new List<State>()
        {
            _idleState,
            _weakenedState,
            _laserState,
            _bombThrowState,
            _bombParryState,
            _debrisThrowState
        };
    }
}