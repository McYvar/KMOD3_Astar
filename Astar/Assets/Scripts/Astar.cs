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

        Node startNode = new Node(startPos, null, 0, Vector2Int.Distance(startPos, endPos));
        Node targetNode = new Node(endPos, null, Vector2Int.Distance(startPos, endPos), 0);
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            current = GetLowestF(openSet);

            openSet.Remove(current);
            closedSet.Add(current);

            if (current.Compare(targetNode))
            {
                List<Vector2Int> path = new List<Vector2Int>();

                path = GeneratePathFromParent(current, path);

                Debug.Log("Loops: " + loops);
                return path;
            }

            foreach(var neighbour in GetAvailableNeighbourNodes(current, grid, endPos)) {
                if (neighbour.Contains(closedSet)) continue;

                if (!neighbour.Contains(openSet) || neighbour.GScore < current.GScore)
                {
                    neighbour.parent = current;
                    if (!neighbour.Contains(openSet)) openSet.Add(neighbour);
                }
            }
            loops++;
        }

        return null;
    }

    // Gekke recursive functie btw :))))))
    private List<Vector2Int> GeneratePathFromParent(Node startNode, List<Vector2Int> path)
    {
        if (startNode.parent != null) GeneratePathFromParent(startNode.parent, path);
        path.Add(startNode.position);
        return path;
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

    private List<Node> GetAvailableNeighbourNodes(Node selected, Cell[,] grid, Vector2Int endPos)
    {
        List<Node> neighbours = new List<Node>();
        Cell currentCell = new Cell();
        foreach(Cell cell in grid)
        {
            if (selected.position == cell.gridPosition)
            {
                currentCell = cell;
                break;
            }
        }
        currentCell.gridPosition = selected.position;
        foreach(Cell cell in currentCell.GetNeighbours(grid))
        {
            if (cell.gridPosition.x < currentCell.gridPosition.x)
            {
                if (cell.HasWall(Wall.RIGHT) || currentCell.HasWall(Wall.LEFT)) continue;
            }
            if (cell.gridPosition.x > currentCell.gridPosition.x)
            {
                if (cell.HasWall(Wall.LEFT) || currentCell.HasWall(Wall.RIGHT)) continue;
            }
            if (cell.gridPosition.y < currentCell.gridPosition.y)
            {
                if (cell.HasWall(Wall.UP) || currentCell.HasWall(Wall.DOWN)) continue;
            }
            if (cell.gridPosition.y > currentCell.gridPosition.y)
            {
                if (cell.HasWall(Wall.DOWN) || currentCell.HasWall(Wall.UP)) continue;
            }
            neighbours.Add(new Node(cell.gridPosition, null, selected.GScore + 1, Vector2Int.Distance(cell.gridPosition, endPos)));
        }
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
        public Node(Vector2Int position, Node parent, float GScore, float HScore)
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

        public bool Contains(List<Node> set)
        {
            foreach (Node node in set)
            {
                if (node.Compare(this)) return true;
            }
            return false;
        }
    }
}
