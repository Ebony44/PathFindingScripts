using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathGrid : MonoBehaviour
{
    private Tilemap tilemap;

    private int floorSizeX;
    private int floorSizeY;

    private PathNode[,] floorCells;
    [SerializeField] private float CollisionDetectionExtents =  0.25f; //  should be individual cell size / 2

    [SerializeField] private GameObject placeholderPrefab;
    [SerializeField] private GameObject placeholderPrefabParent;

    public List<PathNode> Path;

    public int MaxSize
    {
        get { return floorSizeX * floorSizeY; }
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        CreateGrid();
    }

    private void CreateGrid()
    {
        int floorMinX = tilemap.cellBounds.xMin;
        int floorMaxX = tilemap.cellBounds.xMax;
        int floorMinY = tilemap.cellBounds.yMin;
        int floorMaxY = tilemap.cellBounds.yMax;

        floorSizeX = Mathf.Abs(floorMaxX) + Mathf.Abs(floorMinX);
        floorSizeY = Mathf.Abs(floorMaxY) + Mathf.Abs(floorMinY);

        floorCells = new PathNode[floorSizeX, floorSizeY];

        // grab start point for grid.
        Vector3Int startPoint = Vector3Int.zero;
        
        foreach(var vector in tilemap.cellBounds.allPositionsWithin)
        {
            if (vector.x == floorMinX && vector.y == floorMinY)
            {
                startPoint = vector;
            }
        }
        

        // from startPoint, make it floorCells...
        
        for (int x =0; x< floorSizeX;++x)
        {
            for (int y = 0; y< floorSizeY; ++y)
            {
                Vector3 startPointAtWorld = tilemap.CellToWorld(startPoint);
                if (tilemap.HasTile(startPoint))
                {
                    // check walkable
                    // 1. around cell, is there any collider?
                    // TODO: 2. get name and parse it.

                    bool bWalkable = !Physics.CheckBox(startPointAtWorld, Vector3.one * CollisionDetectionExtents, Quaternion.identity);

                    // TODO: Get hindrance value from name.
                    // GetHindrance();
                    int hindrance = 0;
                    GetHindrance(startPoint,out hindrance);
                    
                    floorCells[x, y] = new PathNode(bWalkable, startPointAtWorld, x, y, hindrance);
                    InstantiateObjectAtCells(startPointAtWorld, x, y);
                }
                else if(!tilemap.HasTile(startPoint))
                {
                    // no tile on it. Don't i need to return null???
                    floorCells[x, y] = new PathNode(false, startPointAtWorld, x, y, 0);
                    InstantiateObjectAtCells(startPointAtWorld, x, y);
                }
                startPoint.y += 1;

            }
            startPoint.y = floorMinY;
            startPoint.x += 1;
        }
    }
    private int GetHindrance(Vector3Int startPoint,out int hindrance)
    {
        hindrance = 0;
        var name = tilemap.GetTile(startPoint).name;
        if (name.Contains("_Weight"))
        {
            name = name.Substring(name.IndexOf("_Weight_") + 8);
            Int32.TryParse(name, result: out hindrance);
            return hindrance;
        }
        return 0;
    }
    private void InstantiateObjectAtCells(Vector3 startPointAtWorld, int x, int y)
    {

        if (placeholderPrefab != null && placeholderPrefabParent != null)
        {
            var prefabObject = Instantiate(placeholderPrefab, startPointAtWorld + Vector3.up * 0.25f, Quaternion.identity);
            prefabObject.name = x + " " + y;
            prefabObject.transform.parent = placeholderPrefabParent.transform;
        }
    }


    public List<PathNode> GetNeighbour(PathNode currentNode)
    {
        List<PathNode> neighbour = new List<PathNode>();
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = currentNode.GridX + x;
                int checkY = currentNode.GridY + y;

                if (checkX >= 0 && checkY >= 0 && checkY < floorSizeX && checkX < floorSizeY)
                {
                    neighbour.Add(floorCells[checkX, checkY]);
                }
            }
        }

        return neighbour;


    }

    public PathNode NodeFromWorldPoint(Vector3 worldPos)
    {
        foreach(var node in floorCells)
        {
            if (node == null)
            {
                continue;
            }
            var nodePos = tilemap.WorldToCell(node.WorldPosition);
            Vector3Int worldPosAtCell = tilemap.WorldToCell(worldPos);
            if (nodePos.Equals(worldPosAtCell))
            {
                return floorCells[node.GridX, node.GridY];
            }
        }

        // no Node found at that point.
        return null;
    }
}
