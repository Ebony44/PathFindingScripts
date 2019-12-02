using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

using System.Diagnostics;

public class PathFindingSystem : MonoBehaviour
{
    private PathRequestManager requestManager;
    [SerializeField] private Grid grid;
    private Tilemap tilemap;
    


    [SerializeField] private TileBase tileBase;

    [SerializeField] private float nodeRadius = 2f;
    [SerializeField] private LayerMask unwalkableMask;

    [SerializeField] private GameObject placeholderPrefab;

    public List<Vector3> tileWorldLocations = new List<Vector3>();

    private Vector3Int[] unwalkableCells;

    // public Action<>

    
    private PathGrid pathGrid;

    // public Transform PathFinder;
    // public Transform Target;

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

    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
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
            PathHeap<PathNode> openSet = new PathHeap<PathNode>(pathGrid.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {

                PathNode currentNode = openSet.RemoveFirstItem();
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
                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour) + neighbour.Hindrance;
                    if (newMovementCostToNeighbour < neighbour.GCost || openSet.Contains(neighbour) == false)
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (openSet.Contains(neighbour) == false)
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
            // List<PathNode> tempList = openSet;
            // HashSet<PathNode> tempList2 = closedSet;

        }


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


    


}
