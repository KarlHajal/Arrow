using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using System.Diagnostics;  //Performance Test

public class Pathfinding : MonoBehaviour
{
    //public Transform seeker, target; //no use for it follow the implementation of the PathRequestManager

    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    //void Update() //no use for it follow the implementation of the PathRequestManager
    //{
    //    //if(Input.GetButtonDown("Jump")) //Performance Test
    //    if(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
    //        FindPath(seeker.position, target.position);
    //}

    public void StartFindPath(Vector2 startPos, Vector2 targetPos)
    {
        //StartCoroutine(FindPath(startPos, targetPos));
        FindPath(startPos, targetPos);
    }

    void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        //Stopwatch sw = new Stopwatch();  //Performance Test
        //sw.Start();  //Performance Test

        Vector2[] wayPoints = new Vector2[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {

            Node currentNode = openSet.RemoveFirst();

            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                //sw.Stop(); //Performance Test
                //print("Path Found: " + sw.ElapsedMilliseconds + "ms"); //Performance Test
                pathSuccess = true;
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
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
        
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    Vector2[] RetracePath(Node startingNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startingNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector2[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);
        return wayPoints;
        //grid.path = path;

    }

    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> wayPoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;
        wayPoints.Add(path[0].WorldPosition); //YouTube Comment

        for (int i = 1; i < path.Count; i++) 
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                wayPoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return wayPoints.ToArray();
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10*(distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
	
}

//If we'd like to find a path once every frame

/*
IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
{
    //Stopwatch sw = new Stopwatch();  //Performance Test
    //sw.Start();  //Performance Test

    Vector2[] wayPoints = new Vector2[0];
    bool pathSuccess = false;

    Node startNode = grid.NodeFromWorldPoint(startPos);
    Node targetNode = grid.NodeFromWorldPoint(targetPos);

    //List<Node> openSet = new List<Node>();

    Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
    HashSet<Node> closedSet = new HashSet<Node>();

    openSet.Add(startNode);

    while (openSet.Count > 0)
    {
        //LIST METHOD
        //-----------------------------------------------
        //Node currentNode = openSet[0];
        //for (int i = 1; i < openSet.Count; i++)
        //{
        //    if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
        //        currentNode = openSet[i];
        //}
        //
        //openSet.Remove(currentNode);
        //------------------------------------------------

        //HEAP METHOD
        //------------------------------------------------
        Node currentNode = openSet.RemoveFirst();
        //------------------------------------------------

        closedSet.Add(currentNode);

        if (currentNode == targetNode)
        {
            //sw.Stop(); //Performance Test
            //print("Path Found: " + sw.ElapsedMilliseconds + "ms"); //Performance Test
            pathSuccess = true;
            break;
        }

        foreach (Node neighbour in grid.GetNeighbours(currentNode))
        {
            if (!neighbour.Walkable || closedSet.Contains(neighbour))
                continue;

            int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
            if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
            {
                neighbour.gCost = newMovementCostToNeighbour;
                neighbour.hCost = GetDistance(neighbour, targetNode);
                neighbour.parent = currentNode;

                if(!openSet.Contains(neighbour))
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
    yield return null;
    if(pathSuccess)
    {
        wayPoints = RetracePath(startNode, targetNode);
    }
    requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
}*/
