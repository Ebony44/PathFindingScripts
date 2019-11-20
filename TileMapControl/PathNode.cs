using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode>
{
    public bool Walkable;
    public Vector3 WorldPosition;

    public int GridX { get; set; }
    public int GridY { get; set; }

    public int GCost { get; set; }
    public int HCost { get; set; }

    public PathNode Parent { get; set; }


    public PathNode (bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }
    public int HeapIndex { get; set; }
    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if(compare == 0) // 2 fcost are equal.
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }
}
