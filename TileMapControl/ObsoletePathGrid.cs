using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class ObsoletePathGrid : MonoBehaviour
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
    [SerializeField] private Transform placeholderTransforms;



    // Weights
    public TerrainType[] WalkableRegions;
    private LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();


    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();

        // unused..for now
        //foreach(TerrainType region in WalkableRegions)
        //{
        //    walkableMask.value = walkableMask |= region.TerrainMask.value;
        //    walkableRegionsDictionary.Add((int)Mathf.Log(region.TerrainMask.value, 2), region.TerrainHindrance);
        //}

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
    private void FindEdgeOfCells()
    {
        throw new NotImplementedException();
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

        Debug.Log("floor Min X and Y is " + floorMinX +" "+  floorMinY);
        Debug.Log("floor Max X and Y is " + floorMaxX + " " + floorMaxY);

        // i should cut those above out.

        floorSizeX = Mathf.Abs(floorMaxX) + Mathf.Abs(floorMinX);
        floorSizeY = Mathf.Abs(floorMaxY) + Mathf.Abs(floorMinY);
        
        floorCells = new PathNode[floorSizeX, floorSizeY];

        //FindEdgeOfCells();
        //List<Vector3> listofEdge;

        // Vector3 minXOfCells = tileWorldLocations.Find(vector => vector.x == minX);
        
        // tilemap.GetTile(tilemap.WorldToCell(minXOfCells - Vector3.up * 0.25f));
        // var temp = tilemap.GetBoundsLocal(tilemap.WorldToCell(minXOfCells - Vector3.up * 0.25f)).;

        
        List<Vector3> listOfMinX = tileWorldLocations.FindAll(vector => vector.x == minX);
        
        float minXPointsLeastY = listOfMinX.Min((vector => vector.y));
        
        
        var worldMinXPoint = new Vector3(minX, minXPointsLeastY, 0);

        // float minXPointsGreatestY = listOfMinX.Max((vector => vector.y));

        // find separated edge.



        // maximum of minX- maxX, minY-maxY

        // var worldMinYPoint = new Vector3(minX, minXPointsGreatestY, 0);
        
        // TODO: REFACTOR THESE...
        #region
        var tempX = tilemap.WorldToCell(worldMinXPoint - Vector3.up * 0.25f).x;
        
        var tempLocation = tilemap.WorldToCell(worldMinXPoint - Vector3.up * 0.25f);
        int startLocationMinY = tempLocation.y;
        // int startLocationMaxY = tempLocation.y;

        foreach (var bound in tilemap.cellBounds.allPositionsWithin)
        {

            if (bound.x == tempLocation.x && bound.y < startLocationMinY)
            {
                if (tilemap.HasTile(bound))
                {
                    startLocationMinY = bound.y;
                }
                
            }
        }

        // var tempY = tilemap.WorldToCell(worldMaxYPoint - Vector3.up * 0.25f).y;
        //int tempStartPointValue = (int)Mathf.Abs(Mathf.Abs(temp) - Mathf.Abs(floorMinX));
        // tempStartPointValue = (int)Mathf.Abs(Mathf.Abs(temp) - Mathf.Abs(floorMinX));
        int tempStartPointValueX = Mathf.Max((int)Mathf.Abs(Mathf.Abs(tempLocation.x) - Mathf.Abs(floorMinX)),
            (int)Mathf.Abs(Mathf.Abs(tempLocation.x) - Mathf.Abs(floorMaxX)));

        // var temp5 = Mathf.Abs(startLocationMinY) - Mathf.Abs(floorMinY); // 2
        // var temp6 = Mathf.Abs(tempLocation.y) - Mathf.Abs(floorMaxY - 1); // 3
        startLocationMinY = Mathf.Max(Mathf.Abs(Mathf.Abs(startLocationMinY) - Mathf.Abs(floorMinY)),
            Mathf.Abs(Mathf.Abs(tempLocation.y) - Mathf.Abs(floorMaxY - 1)));
        
        // int tempStartPointValueY = Mathf.Abs(startLocationMaxY) - Mathf.Abs(startLocationMinY);
        var tempStartPointValue = Mathf.Max(tempStartPointValueX, startLocationMinY);
        worldMinXPoint -= Vector3.right * tempStartPointValue;


        floorSizeX += tempStartPointValue;
        floorCells = new PathNode[floorSizeX, floorSizeY];


        Vector3 worldPoint = worldMinXPoint;
        #endregion


        // finding edge
        

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
                    // TODO: DEBUG
                    int hindrance = 0;
                    var name = tilemap.GetTile(tilemap.WorldToCell(worldPoint - Vector3.up * 0.25f)).name;
                    if (name.Contains("_Weight"))
                    {
                        hindrance = Int32.Parse(name);
                    }
                    
                    floorCells[x, y] = new PathNode(walkable, worldPoint, x, y, hindrance);
                    InstantiateObjectAtCells(worldPoint, x, y);
                    
                    
                }// add else to UNWALKABLE
                else if(tilemap.HasTile(tilemap.WorldToCell(worldPoint - Vector3.up * 0.25f)) == false)
                {
                    floorCells[x, y] = new PathNode(!walkable, worldPoint, x, y, 0);
                    InstantiateObjectAtCells(worldPoint, x, y);
                }
                worldPoint += (Vector3.right * (0.5f)) + (Vector3.up * (0.25f));

            }
            worldPoint = worldMinXPoint + (Vector3.right * (x + 1) * (0.5f)) - (Vector3.up * (x + 1) * (0.25f));
        }


    }
    public void InstantiateObjectAtCells(Vector3 worldPoint, int x, int y)
    {
        if (placeholderPrefab != null)
        {
            var prefabObject = Instantiate(placeholderPrefab, worldPoint, Quaternion.identity);
            prefabObject.name = x + " " + y;
            prefabObject.transform.parent = placeholderTransforms;
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

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask TerrainMask;
        public int TerrainHindrance;
    }

}
