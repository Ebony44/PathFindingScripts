using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class PathGrid : MonoBehaviour
{
    public List<Vector3> tileWorldLocations = new List<Vector3>();

    [SerializeField] private float nodeRadius = 2f;
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Tilemap tilemap;
    private PathNode[,] floorCells;

    [SerializeField] private int floorSizeX;
    [SerializeField] private int floorSizeY;



    // delete it after test.

    [SerializeField] private GameObject placeholderPrefab;
    [SerializeField] private Transform placeholderTransform;

    

    //



    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        GrabWorldLocationToList();
        CreateGrid();
        // NodeFromWorldPoint(placeholderTransform.position);
        GetName();
        
    }
    public int MaxSize
    {
        get { return floorSizeX * floorSizeY; }
        
    }

    public List<PathNode> Path;
    private void OnDrawGizmos()
    {
        if(Path != null)
        {
            if (Path.First() != null && Path.Last() != null)
            {
                Gizmos.DrawLine(Path.First().WorldPosition, Path.Last().WorldPosition);
            }
        }
        
    }

    private void GrabWorldLocationToList()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {

            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            Vector3 place = tilemap.CellToWorld(localPlace);
            place.y += 0.25f;
            if (tilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);

            }
            
        }
    }

    private void CreateGrid()
    {
        int floorMinX = tilemap.cellBounds.xMin;
        int floorMaxX = tilemap.cellBounds.xMax;
        int floorMinY = tilemap.cellBounds.yMin;
        int floorMaxY = tilemap.cellBounds.yMax;
        var minY = tileWorldLocations.Min(vector => vector.y);
        var maxY = tileWorldLocations.Max(vector => vector.y);
        var minX = tileWorldLocations.Min(vector => vector.x);
        var maxX = tileWorldLocations.Max(vector => vector.x);
        // i should cut those above out.

        floorSizeX = Mathf.Abs(floorMaxX) + Mathf.Abs(floorMinX);
        floorSizeY = Mathf.Abs(floorMaxY) + Mathf.Abs(floorMinY);
        
        floorCells = new PathNode[floorSizeX, floorSizeY];


        
        List<Vector3> listOfMinX = tileWorldLocations.FindAll(vector => vector.x == minX);
        float minXPointsleastY = listOfMinX.Min((vector => vector.y));
        var worldMinPoint = new Vector3(minX, minXPointsleastY, 0);


        Vector3 worldPoint = worldMinPoint;

        for (int x = 0; x < floorSizeX; ++x)
        {
            for (int y = 0; y < floorSizeY; ++y)
            {
                // + (Vector3.right * (0.5f)) + (Vector3.up * (0.25f));
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                var testVariable = tilemap.WorldToCell(worldPoint - Vector3.up * 0.25f);
                bool testBool = tilemap.HasTile(testVariable);
                if (tilemap.HasTile(tilemap.WorldToCell(worldPoint - Vector3.up * 0.25f)))
                {
                    floorCells[x, y] = new PathNode(walkable, worldPoint, x, y);
                }// add else to UNWALKABLE
                else if(tilemap.HasTile(tilemap.WorldToCell(worldPoint - Vector3.up * 0.25f)) == false)
                {
                    floorCells[x, y] = new PathNode(!walkable, worldPoint, x, y);
                }
                worldPoint += (Vector3.right * (0.5f)) + (Vector3.up * (0.25f));

            }
            worldPoint = worldMinPoint + (Vector3.right * (x + 1) * (0.5f)) - (Vector3.up * (x + 1) * (0.25f));
        }

        // for test purpose. DELETE it afet test.
        foreach (var node in floorCells)
        {
            if (node != null)
            {
                // var prefab = Instantiate(placeholderPrefab, node.WorldPosition, Quaternion.identity);
                
                // Debug.draw
            }

        }

    }

    public List<PathNode> GetNeighbour(PathNode currentNode)
    {
        List<PathNode> neighbour = new List<PathNode>();
        for(int x = -1; x<=1;++x)
        {
            for (int y = -1; y<= 1; ++y)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                
                int checkX = currentNode.GridX + x;
                int checkY = currentNode.GridY + y;

                if (checkX >= 0 && checkY >= 0 && checkY < floorSizeX && checkX < floorSizeY)
                {
                    neighbour.Add(floorCells[checkX,checkY]);
                }
            }
        }

        return neighbour;
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPos)
    {
        // float NearX = worldPos.x
        // tileWorldLocations.
        // return null;

        // Hard corded for now, TODO: modify it.

        // cellSizeX = 0.5f;
        // cellSizeY = 0.5f;
        // float percentX = 

        // 1. 0,0(world's right utmost point) + worldPos.
        // var cellPos = floorCells[0, 0].WorldPosition + worldPos;
        var nearest = tileWorldLocations.Aggregate((current, next) => (current - worldPos).magnitude < (next - worldPos).magnitude ? current : next);
        foreach(var node in floorCells)
        {
            if (node == null)
            {
                continue;
            }
            var nodePos = node.WorldPosition;
            if(node.WorldPosition.x == nearest.x && node.WorldPosition.y == nearest.y)
            {
                
                return floorCells[node.GridX, node.GridY];
                
            }
        }
        // Instantiate(placeholderPrefab, nearest, Quaternion.identity);

        Debug.Log("start or last is null....");
        return null;
    }

    public void GetName()
    {
        var pos = tileWorldLocations.First();
        pos.y -= 0.25f;
        var newPos = tilemap.WorldToCell(pos);
        Vector3Int localPlace = new Vector3Int(newPos.x, newPos.y, newPos.z);

        Vector3 place = tilemap.CellToWorld(localPlace);
        TileBase tilebase = tilemap.GetTile(newPos);
        Debug.Log(tilebase.name);
    }
}
