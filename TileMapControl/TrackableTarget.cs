using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableTarget : MonoBehaviour
{
    public event Action TransformChanged;
    private Transform targetTransform;
    public Transform TargetTransform
    {
        get
        {
            return targetTransform;
        }
        set
        {
            if (targetTransform == value)
            {
                return;
            }
            Transform oldTransform = targetTransform;
            targetTransform = value;
            if (TransformChanged != null)
            {
                TransformChanged();
            }
            //TransformChanged?.Invoke();

        }
    }


    public void RequestForChangedTransform()
    {
        Debug.Log("event called");
        // PathRequestManager.RequestPath(transform.position, TargetTransform.position, OnPathFound);
    }
    private void OnDisable()
    {

        TransformChanged -= RequestForChangedTransform;
        //????
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
