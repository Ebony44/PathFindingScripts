using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PathRequestManager : MonoBehaviour
{
    public Queue<PathRequest> PathReuqestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;

    public static PathRequestManager Instance { get; set; }
    PathFindingSystem pathFinding;

    bool bProcessingPath;

    private void Awake()
    {
        Instance = this;
        pathFinding = GetComponent<PathFindingSystem>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        Instance.PathReuqestQueue.Enqueue(newRequest);
        Instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if(!bProcessingPath && PathReuqestQueue.Count > 0)
        {
            currentPathRequest = PathReuqestQueue.Dequeue();
            bProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.PathStart, currentPathRequest.PathEnd);
        }
    }

    public void FinishProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.Callback(path, success);
        bProcessingPath = false;
        TryProcessNext();
    }

    public struct PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> Callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }

    }
}
