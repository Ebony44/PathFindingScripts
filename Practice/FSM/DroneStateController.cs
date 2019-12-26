using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStateController : MonoBehaviour
{
    public Transform Target { get; private set; }
    [SerializeField]
    public LineRenderer LineRenderer { get; private set; }
    public DroneAttack DroneAttack { get; private set; }
    [SerializeField] private Team team;
    [SerializeField] private GameObject laserVisual;


    public StateMachine stateMachine => GetComponent<StateMachine>();
    
    void Awake()
    {
        // TODO: delete SetTarget(FindObjectOfType<BeastPlayerMovementController>().transform)
        // after implement Wander -> Chase
        if (Target == null)
        {
            SetTarget(FindObjectOfType<ObsoleteBeastPlayerMovementController>().transform);
        }
        
        SetLineRenderer(GetComponent<LineRenderer>());
        SetDroneAttack(GetComponent<DroneAttack>());
        InitializeStateMachine();

    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
    public void SetLineRenderer(LineRenderer lineRenderer)
    {
        LineRenderer = lineRenderer;
    }
    public void SetDroneAttack(DroneAttack droneAttack)
    {
        DroneAttack = droneAttack;
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState (drone: this) },
            // { typeof(WanderState), new WanderState (drone: this) },
            // { typeof(ChaseState), new ChaseState (drone: this) },
            { typeof(DroneAttackState), new DroneAttackState (drone: this) }
        };

        GetComponent<StateMachine>().SetState(states);
    }
    

    // Update is called once per frame
    void Update()
    {
        // stateMachine.CurrentState.Tick();
    }
}

public enum Team
{
    PlayerTeam,
    EnemyTeam,
    NeutralTeam
}
public enum EnemyState
{
    Idle,
    Wander,
    Chase,
    Attack
}
