using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;



public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;
    public BaseState CurrentState { get; private set; }
    public event Action<BaseState> OnStateChanged;

    public void SetState(Dictionary<Type,BaseState> states)
    {
        availableStates = states;
    }

    private void Start()
    {
        // TODO: Delete Start() after implement all states.
        SwitchToNextState(typeof(DroneAttackState));
    }
    private void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = availableStates.Values.First();
        }

        var nextState = CurrentState?.Tick();
        if (nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(Type nextState)
    {
        CurrentState = availableStates[nextState];
        OnStateChanged?.Invoke(CurrentState);
    }
}


