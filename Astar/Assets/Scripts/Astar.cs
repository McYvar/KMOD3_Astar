using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        int loops = 0;
        Node current;
        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();
        List<Vector2Int> path = new List<Vector2Int>();

        Node startNode = new Node(startPos, null, 0, (int) Vector2Int.Distance(startPos, endPos));
        Node targetNode = new Node(endPos, null, (int) Vector2Int.Distance(startPos, endPos), 0);
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            loops++;
            current = GetLowestF(openSet);
            path.Add(current.position);

            if (current.Compare(targetNode))
            {
                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current);


        }

        Debug.Log("Loops: " + loops);
        return null;
    }

    private Node GetLowestF(List<Node> set)
    {
        Node lowestNode = set[0];
        foreach(Node node in set)
        {
            if (node.FScore < lowestNode.FScore)
                lowestNode = node;
        }
        return lowestNode;
    }

    private List<Node> GetAvailableNeighbourNodes(Node selected)
    {
        List<Node> neighbours = new List<Node>();

        return neighbours;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }

        public bool Compare(Node other)
        {
            return (position.x == other.position.x && position.y == other.position.y);
        }
    }
}
