using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShooter : MonoBehaviour
{
    // for debugging. do not public it after test.
    public float Range = 50f;
    [SerializeField] private float shootSpeed = 2f;
    
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;
    [SerializeField] private GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // shootableMask = Layer
        gunLine = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            var projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = shootRay.direction * shootSpeed;
        }
    }

    private void Aim()
    {
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePosDir = mousePos - transform.position;
        mousePosDir.z = 0f;

        // shootRay.direction = transform.forward;
        shootRay.direction = mousePosDir;

        if (Physics.Raycast(shootRay, out shootHit, Range, shootableMask))
        {
            //EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            //if (enemyHealth != null)
            //{
            //    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            //}
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * Range);
        }
    }
}
