using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerpToPlayer : MonoBehaviour
{
    private Vector3 startingPosition;
    public Transform FollowTarget;
    private Vector3 targetPos;
    public float MoveSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowTarget != null)
        {
            targetPos = new Vector3(FollowTarget.position.x, FollowTarget.position.y, transform.position.z);
            Vector3 velocity = (targetPos - transform.position) * MoveSpeed;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);
        }
    }
}
