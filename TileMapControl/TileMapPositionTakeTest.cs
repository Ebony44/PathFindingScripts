using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

using System.Diagnostics;

public class TileMapPositionTakeTest : MonoBehaviour
{
    private PathRequestManager requestManager;
    [SerializeField] private Grid grid;
    private Tilemap tilemap;
    public List<Tilemap> obstacleLayers; // all layers that contain objects

    [SerializeField] private Vector2 floorSize;

    //public int scanStartX;
    //public int scanStartY;
    //public int scanFinishX;
    //public int scanFinishY;

    //public List<GameObject> UnsortedNodes = new List<GameObject>();
    //public HashSet<GameObject> Sortednodes = new HashSet<GameObject>();
    //public GameObject nodePrefab;

    // private TileBase[] tileArray;

    [SerializeField] private TileBase tileBase;

    [SerializeField] private float nodeRadius = 2f;
    [SerializeField] private LayerMask unwalkableMask;

    [SerializeField] private GameObject placeholderPrefab;

    public List<Vector3> tileWorldLocations = new List<Vector3>();

    private Vector3Int[] unwalkableCells;

    // public Action<>

    private PathNode[,] floorCells;
    private PathGrid pathGrid;

    public Transform PathFinder;
    public Transform Target;

    private enum eCellSetting
    {
        CELL_MAX_COUNT = 8000
    }

    // Start is called before the first frame update
    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();

        tilemap = GetComponent<Tilemap>();
        pathGrid = GetComponent<PathGrid>();
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        // tileArray = tilemap.GetTilesBlock(bounds);

        // Debug.Log("bound x: " + bounds.size.x + " bound y: " + bounds.size.y);
        #region
        //for (int x = 0; x < bounds.size.x; ++x)
        //{
        //    for (int y = 0; y < bounds.size.y; ++y)
        //    {
        //        TileBase tile = tileArray[x + y];// * bounds.size.x];
        //        if (tile != null)
        //        {
        //            Debug.Log("x: " + x + " y: " + y + " tile: " + tile.name);
        //            var placehoderPosition = grid.WorldToCell(new Vector3(y, x, 0));
        //            var placehoderPosition2 = grid.CellToWorld(new Vector3Int(x, y, 0));
        //            Instantiate(placeholderPrefab, placehoderPosition2, Quaternion.identity);
        //            // Debug.
        //            // Gizmos.DrawCube(new Vector3(y, x, 0), tilemap.cellSize);
        //        }
        //        else
        //        {
        //            // Debug.Log("x: " + x + "y: " + y + " tile: (null)");
        //        }
        //    }
        //}
        #endregion



    }
    void Update()
    {
        ChangeColorOnClick();
        if (PathFinder != null && Target != null)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                // FindPath(PathFinder.position, Target.position);
            }

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.DrawLine(pathGrid.Path.First().WorldPosition, pathGrid.Path.Last().WorldPosition);
            foreach (PathNode node in pathGrid.Path)
            {
                Instantiate(placeholderPrefab, node.WorldPosition, Quaternion.identity);
            }
            //Instantiate(placeholderPrefab, startNode.WorldPosition, Quaternion.identity);
        }


    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    private void GrabNodes()
    {
        /*
        // var tileWorldLocations = new List<Vector3>();
        // TODO: Figure out how many cells will contain in 1 Scene. currently 8k.
        // well, if it's lacking, it will AUTOMATICALLY resize..but it's not performance wise :x
        
        tileWorldLocations.Capacity = (int) eCellSetting.CELL_MAX_COUNT;
        // Vector3Int floorHeight = tilemap.cellBounds.allPositionsWithin;
        int floorMinX = tilemap.cellBounds.xMin;
        int floorMaxX = tilemap.cellBounds.xMax;
        int floorMinY = tilemap.cellBounds.yMin;
        int floorMaxY = tilemap.cellBounds.yMax;
        Debug.Log(tilemap.cellBounds.xMin + " and " + tilemap.cellBounds.xMax);
        Debug.Log(tilemap.cellBounds.yMin + " and " + tilemap.cellBounds.yMax);
        Debug.Log(tilemap.cellBounds.min + " and " + tilemap.cellBounds.max);
        Debug.Log(tilemap.CellToWorld (tilemap.cellBounds.min) + " and " + tilemap.CellToWorld( tilemap.cellBounds.max));
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            
            Vector3 place = tilemap.CellToWorld(localPlace);
            place.y += 0.25f;
            if (tilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);

            }
            // if (tilemap.WorldToCell(localPlace) ==  )
            // var floorMinX =
        }

        // this one for testing. i already checked it, so it's just ARCHIVE.
        foreach (var location in tileWorldLocations)
        {
            
            // Instantiate(placeholderPrefab, location, Quaternion.identity);
        }

        // tilemap.GetTile(grid.WorldToCell(Vector3.zero))

        // for ()
        
        var minY = tileWorldLocations.Min(vector => vector.y);
        var maxY = tileWorldLocations.Max(vector => vector.y);
        var minX = tileWorldLocations.Min(vector => vector.x);
        var maxX = tileWorldLocations.Max(vector => vector.x);
        Debug.Log("minY is : " + minY + " and maxY is:  " + maxY + " also minmaxX is : " + minX + " " + maxX);

        foreach(Vector3 vector in tileWorldLocations)
        {
            if (Physics.CheckSphere(vector, nodeRadius,unwalkableMask))
            {
                
                // TODO: check it's walkable or not.

                // Debug.Log("something is unwalkable!");
                // var placeholder = Instantiate(placeholderPrefab, vector, Quaternion.identity);
                // placeholder.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            
            // pathNodes[vector.x,vector.y] = new PathNode()
        }

        // X axis while
        Vector3? xAxisVector = null;
        xAxisVector = new Vector3(minX, minY, 0);
        Vector3? yAxisVector = null;
        yAxisVector = new Vector3(minX, minY, 0);
        int xIndex = 0;
        int yIndex = 0;
        var worldFloorPoint = new Vector3(minX, minY, 0);
        var listOfMinX = tileWorldLocations.FindAll(vector => vector.x == minX);
        var leastY = listOfMinX.Min((vector => vector.y));
        worldFloorPoint = new Vector3(minX, leastY, 0);
        //

        var floorSizeX = Mathf.Abs(floorMaxX) + Mathf.Abs(floorMinX);   //Mathf.Abs(Mathf.Max(minX, maxX) / Mathf.Min(minX, maxX));
        var floorSizeY = Mathf.Abs(floorMaxY) + Mathf.Abs(floorMinY);   //Mathf.Abs(Mathf.Max(minY, maxY) / Mathf.Min(minY, maxY));
        floorCells = new PathNode[floorSizeX, floorSizeY];
        Vector3 worldPoint = worldFloorPoint;
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
                }
                worldPoint += (Vector3.right * (0.5f)) + (Vector3.up * (0.25f));

            }
            worldPoint = worldFloorPoint + (Vector3.right * (x + 1) * (0.5f)) - (Vector3.up * (x + 1) * (0.25f));
        }

        // every segment, x: +0.5, y: +0.25
        #region
        //Vector3 worldPoint = worldFloorPoint;
        //while (xAxisVector != null)
        //{
        //    // ++xIndex;
        //    // Vector3 worldPoint = worldFloorPoint + Vector3.right * (0.5f) + Vector3.up * (0.25f);
        //    // tilemap.HasTile(tilemap.WorldToCell(worldPoint)
        //    // while (yAxisVector != null)

        //    while (tilemap.HasTile(tilemap.WorldToCell(worldPoint)) == false)
        //    {
        //        worldPoint = worldFloorPoint + Vector3.right * (0.5f) + Vector3.up * (0.25f);
        //        yAxisVector = worldPoint;
        //        bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
        //        floorCells[xIndex, yIndex] = new PathNode(walkable, worldPoint, xIndex, yIndex);
        //        ++yIndex;
        //    }
        //    // worldPoint = 
        //}
        #endregion
        // for Testing 2
        foreach(var node in floorCells)
        {
            if (node != null)
            {
                Instantiate(placeholderPrefab, node.WorldPosition, Quaternion.identity);
            }
            
        }
        */
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPos)
    {
        int x = tilemap.WorldToCell(worldPos).x;
        int y = tilemap.WorldToCell(worldPos).y;

        return floorCells[x, y];
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        PathNode startNode = pathGrid.NodeFromWorldPoint(startPos);
        PathNode targetNode = pathGrid.NodeFromWorldPoint(targetPos);

        if (startNode.Walkable && targetNode.Walkable)
        {

        }

        //Instantiate(placeholderPrefab, startNode.WorldPosition, Quaternion.identity);
        //Instantiate(placeholderPrefab, targetNode.WorldPosition, Quaternion.identity);

        // Debug.Log(startNode.WorldPosition + " " + targetNode.WorldPosition);

        // List<PathNode> openSet = new List<PathNode>();
        PathHeap<PathNode> openSet = new PathHeap<PathNode>(pathGrid.MaxSize);
        HashSet<PathNode> closedSet = new HashSet<PathNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // PathNode currentNode = openSet[0];
            PathNode currentNode = openSet.RemoveFirstItem();
            //for (int i = 1; i < openSet.Count; ++i)
            //{
            //    // it's not optimized. do not use it as raw.
            //    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost)
            //    {
            //        if (openSet[i].HCost < currentNode.HCost)
            //        {
            //            currentNode = openSet[i];
            //        }
            //    }
            //}
            //// now check open and closed set...

            //openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + " ms");

                pathSuccess = true;

                //RetracePath(startNode, targetNode);
                //return;
                break;
            }

            foreach (PathNode neighbour in pathGrid.GetNeighbour(currentNode))
            {
                if (neighbour == null)
                {
                    continue;
                }
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || openSet.Contains(neighbour) == false)
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (openSet.Contains(neighbour) == false)
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        // List<PathNode> tempList = openSet;
        // HashSet<PathNode> tempList2 = closedSet;
        
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishProcessingPath(waypoints, pathSuccess);


    }

    private Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        
        Vector3[] waypoints = SimplifyPath(path);
        path.Reverse();
        pathGrid.Path = path;


        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        // TODO: Debug that it's working properly. highly doubted vector2.zero or 
        // x(i-1) - x(i), y(i-1) - y(i) will work on my isometric cells.
        
        // 0,0 1,1 (same direction, on straight line) 
        // 0,0 0,1 (or 1,0) (same direciotn, on diagonal line)

        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i<path.Count;++i)
        {
            Vector2 directionNew = new Vector2(path[i-1].GridX - path[i].GridX , path[i - 1].GridY - path[i].GridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();

    }

    private int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        // TODO: Must DEBUG.

        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);
        // cell 4,4(adjacent diagonal) - cell 5,4(start)
        // dstX is 1, dstY is 0 -> diagonal Cost... 6...

        // cell 4,3(adjacent straight) - cell 5.4(start)
        // dstX is 1, dstY is 1 -> straight Cost ...10....

        // cell 4,4(adjacent of start), cell 3,2(target)
        // X is 1, Y is 2

        int straightCost = 10; // 1.
        int diagonalCost = 6; // approximately 0.559....:x...

        if (dstX == dstY)
        {
            // only moving straight.
            return straightCost * dstX;
        }
        if (dstX > dstY)
        {
            //diagonal + straight
            return straightCost * dstY + diagonalCost * (dstX - dstY);
        }
        else
        {
            //diagonal + straight
            return straightCost * dstX + diagonalCost * (dstY - dstX);
        }





    }



    #region
    //int gridBoundX = 0;
    //int gridBoundY = 0;
    //private void CreateNode()
    //{
    //    int gridX = 0; 
    //    // use these to work out the size and where each node should e in the 2d array 
    //    // we'll use to store our nodes so we can work out neighbours and get paths.
    //    int girdY = 0;
    //    for (int x = scanStartX; x < scanFinishX; x++)
    //    {
    //        for (int y = scanStartY; y < scanFinishY; y++)
    //        {
    //            //go through our world bounds in increments of 1
    //            TileBase tb = floor.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords
    //            if (tb == null)
    //            {
    //            }
    //            else
    //            {
    //                //if we do we go through the obstacle layers and check if there is also a tile at those coords if so we set founObstacle to true
    //                bool foundObstacle = false;
    //                foreach (Tilemap t in obstacleLayers)
    //                {
    //                    TileBase tb2 = t.GetTile(new Vector3Int(x, y, 0));

    //                    if (tb2 == null)
    //                    {

    //                    }
    //                    else
    //                    {
    //                        foundObstacle = true;
    //                    }

    //                    //if we want to add an unwalkable edge round our unwalkable nodes then we use this to get the neighbours and make them unwalkable
    //                    #region
    //                    //if (unwalkableNodeBorder > 0)
    //                    //{
    //                    //    List<TileBase> neighbours = getNeighbouringTiles(x, y, t);
    //                    //    foreach (TileBase tl in neighbours)
    //                    //    {
    //                    //        if (tl == null)
    //                    //        {

    //                    //        }
    //                    //        else
    //                    //        {
    //                    //            foundObstacle = true;
    //                    //        }
    //                    //    }
    //                    //}
    //                    #endregion
    //                }

    //                if (foundObstacle == false)
    //                {
    //                    //if we havent found an obstacle then we create a walkable node and assign its grid coords
    //                    GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(x + 0.5f + gridBase.transform.position.x, y + 0.5f + gridBase.transform.position.y, 0), Quaternion.Euler(0, 0, 0));
    //                    WorldTile wt = node.GetComponent<WorldTile>();
    //                    wt.gridX = gridX;
    //                    wt.gridY = gridY;
    //                    foundTileOnLastPass = true; //say that we have found a tile so we know to increment the index counters
    //                    unsortedNodes.Add(node);

    //                    node.name = "NODE " + gridX.ToString() + " : " + gridY.ToString();

    //                }
    //                else
    //                {
    //                    //if we have found an obstacle then we do the same but make the node unwalkable
    //                    GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(x + 0.5f + gridBase.transform.position.x, y + 0.5f + gridBase.transform.position.y, 0), Quaternion.Euler(0, 0, 0));
    //                    //we add the gridBase position to ensure that the nodes are ontop of the tile they relate too
    //                    node.GetComponent<SpriteRenderer>().color = Color.red;
    //                    WorldTile wt = node.GetComponent<WorldTile>();
    //                    wt.gridX = gridX;
    //                    wt.gridY = gridY;
    //                    wt.walkable = false;
    //                    foundTileOnLastPass = true;
    //                    unsortedNodes.Add(node);
    //                    node.name = "UNWALKABLE NODE " + gridX.ToString() + " : " + gridY.ToString();


    //                }
    //                gridY++; //increment the y counter


    //                if (gridX > gridBoundX)
    //                { //if the current gridX/gridY is higher than the existing then replace it with the new value
    //                    gridBoundX = gridX;
    //                }

    //                if (gridY > gridBoundY)
    //                {
    //                    gridBoundY = gridY;
    //                }
    //            }
    //        }
    //        if (foundTileOnLastPass == true)
    //        {//since the grid is going from bottom to top on the Y axis on each iteration of the inside loop, if we have found tiles on this iteration we increment the gridX value and 
    //         //reset the y value
    //            gridX++;
    //            gridY = 0;
    //            foundTileOnLastPass = false;
    //        }
    //    }
    //}

    #endregion

    // Update is called once per frame
    
    private void cellWalkableCheck()
    {
        // TODO: check Profiler and if it makes performance issue.
        // 1. run this method in Destructible and Obstacle(wall) class.
        // 2. make it Action - Event, so when it's changed, listen that and run method.

        // do NOT every grid check it.
        // tilemap.GetUsedTilesCount
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = grid.WorldToCell(mousePos);
        // Physics.CheckCapsule(cellPos,)
        if (Physics.CheckSphere(cellPos, nodeRadius, unwalkableMask) == true)
        {
            unwalkableCells[unwalkableCells.Length] = cellPos;
        }

    }

    private void ChangeColorOnClick()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            mousePos.z = 0f;
            //Debug.Log("mouse Position is: " + mousePos);
            //Debug.Log("mouse Position 2d is: " + mousePos2);

            Vector3Int cellPos = grid.WorldToCell(mousePos);
            // Vector3Int cellPos = new Vector3Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);


            tileBase = tilemap.GetTile(cellPos);
            // Collider colliderChecker = tileBase.GetTileData.



            // tilemap.SetColor(cellPos, Color.red);
            // tilemap.SetTile(cellPos, tileBase);
            //Debug.Log("cellpos is: " + cellPos);
            //Debug.Log("cell's tileSprite is.. " + tilemap.GetSprite(cellPos));
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(floorSize.x, floorSize.y, 0));
        // Gizmos.DrawCube
        //if(grid != null)
        //{
        //    foreach(Vector3Int unwalkableCell in unwalkableCells)
        //    {
        //        //Gizmos.color = unwalkableCell
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawCube(unwalkableCell, grid.cellSize * 1.2f);
        //    }
        //}

    }

}
