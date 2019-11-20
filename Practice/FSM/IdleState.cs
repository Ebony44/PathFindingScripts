using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class IdleState : BaseState
{
    private DroneStateController mDrone;
    private float idleTime = 0f;

    public IdleState(DroneStateController drone) : base(drone.gameObject)
    {
        this.mDrone = drone;
    }

    public override Type Tick()
    {
        // TODO: at random time, switch idle - wander until find the player.
        // idleTime += UnityEngine.Random.Range(0, Time.time)
        return typeof(WanderState);
    }
}