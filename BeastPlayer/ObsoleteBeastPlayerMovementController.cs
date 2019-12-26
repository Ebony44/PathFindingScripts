using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsoleteBeastPlayerMovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    private Rigidbody rb;
    private Animator animator;
    //private Animation myAnimation;
    [Header("MoveVariables")]
    
    [SerializeField] private float movementAngle1;
    [SerializeField] private float movementAngle2;
    [SerializeField] private float movementAngleRaw;
    // for testing.
    [SerializeField] private bool bMoving = true;

    [Header("AttackVariables")]
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private float maxAttackDelay = 2.5f;
    [SerializeField] private float attackMoveSpeedMultiplier = 0.4f;

    private Coroutine attackingCoroutine;
    private Coroutine attackingMovementCoroutine;
    
    [Header("BackstepVariables")]
    [SerializeField] private float backstepDelay = 0.1f;
    [SerializeField] private float maxbackstepDelay = 2.5f;
    [SerializeField] private float backstepMoveSpeedMultiplier = -0.4f;


    private Vector3 targetRotation;

    public struct mathDefined
    {
        public const float PI = 3.14159274F;
        
    }

    private void Awake()
    {
        // rb = GetComponentInChildren<Rigidbody>();
        
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        Debug.Log(gameObject.name);
    }
    private void FixedUpdate()
    {

        Vector3 currentPos = rb.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        float verticalInputRaw = Input.GetAxisRaw("Vertical");


        movementAngle1 = Mathf.Atan2(horizontalInput, verticalInput);
        movementAngleRaw = Mathf.Atan2(horizontalInputRaw, verticalInputRaw);
        Vector3 inputVector = new Vector3(horizontalInput, verticalInput, rb.position.z);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);


        Vector3 inputVectorRaw = new Vector3(horizontalInputRaw, verticalInputRaw, rb.position.z);
        inputVectorRaw = Vector2.ClampMagnitude(inputVectorRaw, 1);

        Vector3 movement = inputVector * movementSpeed;
        Vector3 newPos = currentPos + movement * Time.fixedDeltaTime;

        if (bMoving)
        {
            rb.MovePosition(newPos);
        }
        
        Quaternion rotation = Quaternion.Euler(Mathf.Rad2Deg * movementAngle1, 90, 0);
        Quaternion rotationRaw = Quaternion.Euler(Mathf.Rad2Deg * movementAngleRaw, 90, 0);
        // Quaternion rotation = Quaternion.Euler(Mathf.Rad2Deg * movementAngle1, 0, 0);
        if (horizontalInputRaw != 0 || verticalInputRaw != 0)
        {
            // rb.MoveRotation(rotation);
            transform.localEulerAngles = rotation.eulerAngles;

        }
        else if (horizontalInputRaw == 0 || verticalInputRaw == 0)
        {
            // transform.rotation = rotationRaw;
        }
        if (movementAngle1 < 0)
        {
            // movementAngle1 += 90;
        }

        MovementAnimation(verticalInput, horizontalInput);
        /*
        if (Input.GetMouseButton(0))
        {
            animator.SetInteger("isAttacking", 0);
        }
        if(Input.GetMouseButtonUp(0))
        {
            animator.SetInteger("isAttacking", -1);
        }
        */
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float aimDistanceX = mousePos.x - transform.position.x;
        float aimDistanceY = mousePos.y - transform.position.y;
        float mousePosAngle = Mathf.Atan2(aimDistanceY, aimDistanceX);
        // if aimed point's angle is negative(3, 4)
        if (mousePosAngle < 0f)
        {
            mousePosAngle = mousePosAngle + (Mathf.PI * 2);
        }
        attackDelay += Time.deltaTime;
        backstepDelay += Time.deltaTime;
        Attack(mousePosAngle);
        Backstep(mousePosAngle);
    }


    private void HandleMovement()
    {

    }

    private void MovementAnimation(float horizontal, float vertical)
    {

        bool bIsMoving = horizontal != 0f || vertical != 0f;
        animator.SetBool("isMoving", bIsMoving);
    }

    private void Attack(float mousePosAngle)
    {
        if (Input.GetButton("Fire1") && attackDelay >= maxAttackDelay)
        {
            MovementVelocity(mousePosAngle, attackMoveSpeedMultiplier);
            /*
            float xVelocity = Mathf.Cos(mousePosAngle) * attackMoveSpeedMultiplier;
            float yVelocity = Mathf.Sin(mousePosAngle) * attackMoveSpeedMultiplier;
            
            rb.velocity = new Vector3(xVelocity, yVelocity, 0);
            float newRotationX = Mathf.Atan2(xVelocity, yVelocity);
            rb.rotation = Quaternion.Euler(newRotationX * Mathf.Rad2Deg, 90, 0);
            */
            // Vector3 newPos = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, 0);
            
            Debug.Log("attacking and moving");
            
            int attackAnimIndex = Random.Range(0, 3);
            if (attackAnimIndex == animator.GetInteger("isAttacking"))
            {
                if (attackAnimIndex == 0)
                {
                    attackAnimIndex = Random.Range(1, 3);
                }
                else if (attackAnimIndex == 3)
                {
                    attackAnimIndex = Random.Range(0, 2);
                }
                else
                {
                    attackAnimIndex = Random.Range(0, 3 - attackAnimIndex);
                }
                
            }
            
            animator.SetInteger("isAttacking", attackAnimIndex);
            attackDelay = 0;
            StartCoroutine(AttackSecond());
            
            //attackingCoroutine = StartCoroutine(AttackAnimation());
            //attackingMovementCoroutine = StartCoroutine(AttackMovement(mousePosAngle));

        }
        //if (Input.GetMouseButtonUp(0))
        if (Input.GetButtonUp("Fire1"))
        {
            //StopCoroutine(attackingCoroutine);
            //StopCoroutine(attackingMovementCoroutine);
            // animator.SetInteger("isAttacking", -1);
        }
    }
    private void Backstep(float mousePosAngle)
    {
        if (Input.GetButton("Fire2") && backstepDelay >= maxbackstepDelay)
        {
            MovementVelocity(mousePosAngle, backstepMoveSpeedMultiplier);
            animator.SetInteger("isDodging", 0);
            backstepDelay = 0;
            StartCoroutine(BackstepSecond());
        }
    }
    private IEnumerator AttackSecond()
    {
        // animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking")


        // yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        // animator = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f &&
        animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") == true);
        
        Debug.Log("attacking finished");
        animator.SetInteger("isAttacking", -1);

        /*
        while(true)
        {
            // if (Input.GetButtonUp("Fire1") && attackDelay < maxAttackDelay)
            if (Input.GetButtonUp("Fire1") && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") == false)
            {
                Debug.Log("wait for attack");
                break;
            }
            
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("wait for attack");
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking") == true)
        {
            animator.SetInteger("isAttacking", -1);
        }
        */
    }
    private IEnumerator AttackAnimation()
    {
        while (true)
        {
            int attackAnimIndex = Random.Range(0, 2);
            animator.SetInteger("isAttacking", attackAnimIndex);
            yield return new WaitForSeconds(attackDelay);
        }

    }
    private IEnumerator AttackMovement(float mousePosAngle)
    {
        while (true)
        {
            MovementVelocity(mousePosAngle, attackMoveSpeedMultiplier);
            Debug.Log("attacking and moving");
            // rb.velocity = new Vector3(xVelocity, yVelocity, 0);
            yield return new WaitForSeconds(attackDelay);
            // Vector3 newPos = new Vector3( transform.position.x - xVelocity, transform.position.y - yVelocity, 0);
        }

    }

    private IEnumerator BackstepSecond()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f &&
        animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodging") == true);

        Debug.Log("dodging finished");
        animator.SetInteger("isDodging", -1);
    }
    private IEnumerator BackstepMovement(float mousePosAngle)
    {
        // TODO: implement backstep.
        while (true)
        {
            MovementVelocity(mousePosAngle, backstepMoveSpeedMultiplier);
            // rb.velocity = new Vector3(xVelocity, yVelocity, 0);
            yield return new WaitForSeconds(attackDelay);
        }
        
    }
    private void MovementVelocity(float mousePosAngle, float moveSpeedModifier)
    {
        // TODO: Attack and Backstep's rb.velocity.
        float xVelocity = Mathf.Cos(mousePosAngle) * moveSpeedModifier;
        float yVelocity = Mathf.Sin(mousePosAngle) * moveSpeedModifier;

        Vector3 newPos = new Vector3(transform.position.x + xVelocity, transform.position.y + yVelocity, 0);
        rb.velocity = new Vector3(xVelocity, yVelocity, 0);
        if (moveSpeedModifier < 0f)
        {
            xVelocity = Mathf.Cos(mousePosAngle) * (-moveSpeedModifier);
            yVelocity = Mathf.Sin(mousePosAngle) * (-moveSpeedModifier);
        }
        else
        {

        }
        // rb.MovePosition(newPos);

        float newRotationX = Mathf.Atan2(xVelocity, yVelocity);
        Quaternion rotation = Quaternion.Euler(Mathf.Rad2Deg * movementAngle1, 90, 0);
        transform.localEulerAngles = new Vector3(newRotationX * Mathf.Rad2Deg, 90, 0);
        // rb.rotation = Quaternion.Euler(newRotationX * Mathf.Rad2Deg, 90, 0);
    }

}
