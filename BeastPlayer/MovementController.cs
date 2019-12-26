using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Animator animator;
    private Collider collider;

    public float movementSpeed = 10f;


    [SerializeField] private Transform dashEffect;
    private Vector3 lastMoveDir;
    private Vector3 backStepDir;
    private float backStepSpeed;

    private enum State
    {
        Idle,
        Running,
        Attacking,
        BackStepping,
    }

    private State state;

    // check can move direction
    Ray checkingRay;


    private void Awake()
    {
        // animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        state = State.Idle;
    }

    private void Update()
    {
        // TODO: Implement Animation and Backstep smoothly.
        switch(state)
        {
            case State.Idle:
                HandleMovement();
                break;
            case State.Running:
                break;
            case State.Attacking:
                break;
            case State.BackStepping:
                break;
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        bool bIdle = horizontalInput == 0 && verticalInput == 0;
        if (bIdle)
        {
            
        }
        Vector3 moveDir = new Vector3(horizontalInput, verticalInput).normalized;
        TryMove(moveDir, movementSpeed * Time.deltaTime);
        

    }

    private bool CanMove(Vector3 dir, float distance)
    {

        checkingRay.origin = transform.position;
        checkingRay.direction = dir;
        RaycastHit hit;
        Physics.Raycast(checkingRay, out hit, distance);
        Collider hitCollider = hit.collider;

        bool testBool = Physics.GetIgnoreLayerCollision(collider.gameObject.layer, hitCollider.gameObject.layer);

        if (hitCollider == null)
        {
            return true;
        }
        else
        {
            return Physics.GetIgnoreLayerCollision(collider.gameObject.layer, hitCollider.gameObject.layer);
        }
        
        

    }

    private bool TryMove(Vector3 moveDir, float distance)
    {
        Vector3 nextMoveDir = moveDir;
        bool bMove = CanMove(moveDir, distance);

        if (!bMove)
        {
            // horizontally
            nextMoveDir = new Vector3(moveDir.x, 0f).normalized;
            bMove = nextMoveDir.x != 0f && CanMove(nextMoveDir, distance);
            if (!bMove)
            {
                // vertically
                nextMoveDir = new Vector3(moveDir.y, 0f).normalized;
                bMove = nextMoveDir.y != 0f && CanMove(nextMoveDir, distance);
            }
        }

        if (bMove)
        {
            lastMoveDir = moveDir;
            transform.position += moveDir * distance;
            return true;
            
        }
        else
        {
            return false;
        }

    }


}
