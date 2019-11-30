using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUnit : MonoBehaviour
{

    
    

    public Transform TargetTransform;
    [SerializeField] private float speed = 1f;
    private Vector3[] path;
    private int targetIndex;


    void Start()
    {
        // TargetTransform.Find("Cube (6)");
        // TEST PURPOSE delete this find() after test.
        // TargetTransform = GameObject.Find("Cube (6)").transform;
        // TransformChanged += RequestForChangedTransform;
        PathRequestManager.RequestPath(transform.position, TargetTransform.position, OnPathFound);
        // if position is changed after success... -> reuqestpath again.
    }
    public void RequestForChangedTransform()
    {
        
    }
    private void OnDisable()
    {
        
        
        //????
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                ++targetIndex;
                if(targetIndex >= path.Length)
                {
                    Debug.Log("path count " + path.Length);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }



    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i<path.Length;++i)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.1f);
                if ( i ==targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}
