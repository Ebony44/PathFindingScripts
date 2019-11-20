using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackState : BaseState
{
    private Transform mPlayerTransform;
    private bool bPlayerAlive = true;
    private DroneStateController mDrone;
    private LineRenderer mLineRenderer;
    private DroneAttack mDroneAttack;
    public DroneAttackState(DroneStateController drone) : base(drone.gameObject)
    {
        
        mDrone = drone;
        
        // mLineRenderer = drone.GetComponent<LineRenderer>();
        mLineRenderer = drone.LineRenderer;
        mPlayerTransform = drone.Target.transform;
        

        
    }
    public override Type Tick()
    {
        if (mPlayerTransform == null && bPlayerAlive == false)
        {
            return typeof(WanderState);
        }
        // TODO: delete after implement playerposition finding method.
        if(mPlayerTransform == null)
        {
            mPlayerTransform = mDrone.Target.transform;
        }
        if (mPlayerTransform != null)
        {
            // mDroneAttack.Attack();
            Fire(mPlayerTransform);
        }
        return null;
        // return typeof(WanderState);
    }

    private void Fire(Transform playerTransform)
    {
        if (mLineRenderer != null)
        {
            
        }
        mLineRenderer.SetPosition(0, mDrone.transform.position);
        mLineRenderer.SetPosition(1, playerTransform.position);


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
