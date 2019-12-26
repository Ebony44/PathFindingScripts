using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DroneAttack : MonoBehaviour, IAttack
{
    Transform TargetTransform { get; set; }
    [SerializeField] private float damageValue = 5f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float maxAttackDelay = 2f;
    [SerializeField] GameObject projectileprefab;

    public event Action OnDied = delegate { };

    void Start()
    {
        
    }
    

    
    void Update()
    {
        if (TargetTransform == null)
        {
            TargetTransform = FindObjectOfType<ObsoleteBeastPlayerMovementController>().transform;
            
        }
        attackDelay += Time.deltaTime;
        Attack();
        
    }

    public void Attack()
    {
        // target is in range and attacker is ready to fire.
        if (Vector3.Distance(transform.position, TargetTransform.position) <= attackRange && attackDelay >= maxAttackDelay)
        {
            var projectile = Instantiate(projectileprefab, transform.position, Quaternion.identity);
            Vector3 direction = TargetTransform.position - transform.position;
            projectile.GetComponent<Rigidbody>().velocity = direction;
            attackDelay = 0;
        }
        
    }

    public void GetAwayFromTarget(Vector3 targetPos)
    {
        throw new NotImplementedException();
    }

    public void TravelToTarget(Vector3 targetPos)
    {
        throw new NotImplementedException();
    }
}
