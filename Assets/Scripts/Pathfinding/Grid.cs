using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;

    public Vector2 gridWorldSize;
    public LayerMask unwalkableMask;
    public float nodeRadius;
    public GameObject Character;//Test
    Node[,] grid;

    float nodeDiameter;
    int gridNodesX, gridNodesY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridNodesX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridNodesY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridNodesX * gridNodesY;
        }
    }


    void CreateGrid()
    {
        grid = new Node[gridNodesX, gridNodesY];
        Vector2 worldBottomLeft = Vector2.right * (transform.position.x - (gridWorldSize.x / 2)) + Vector2.up * (transform.position.y - (gridWorldSize.y / 2));

        for (int x = 0; x < gridNodesX; x++)
        {
            for (int y = 0; y < gridNodesY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius,unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridNodesX && checkY >= 0 && checkY < gridNodesY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x / gridWorldSize.x) + 0.5f;
        float percentY = (worldPosition.y / gridWorldSize.y) + 0.5f;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridNodesX - 1) * percentX);
        int y = Mathf.RoundToInt((gridNodesY - 1) * percentY);

        return grid[x, y];
    }

 //   public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        /*
        if (DrawPathGizmosOnly)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
        else
        {*/
           /* if (grid != null && displayGridGizmos)
            {
                //Node playerNode = NodeFromWorldPoint(Character.transform.position); //Test
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.Walkable) ? Color.white : Color.red;

                
                    if (path != null)
                        if (path.Contains(n))
                            Gizmos.color = Color.black;

                    if (playerNode == n) //Test
                        Gizmos.color = Color.cyan;
                

                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
                
            }*/
        //}
    }

}
