using System;
using System.Collections.Generic;
using System.Linq;

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool Walkable { get; set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;

    public Node Parent { get; set; }

    public Node(int x, int y, bool walkable = true)
    {
        X = x;
        Y = y;
        Walkable = walkable;
        G = int.MaxValue;
        H = 0;
        Parent = null;
    }
}

public class AStar
{
    private Node[,] grid;
    private int width;
    private int height;

    public AStar(Node[,] grid)
    {
        this.grid = grid;
        width = grid.GetLength(0);
        height = grid.GetLength(1);
    }

    public List<Node> FindPath(Node startNode, Node goalNode)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        startNode.G = 0;
        startNode.H = GetDistance(startNode, goalNode);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList.OrderBy(node => node.F).ThenBy(node => node.H).First();

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == goalNode)
            {
                return CreatePath(currentNode);
            }

            List<Node> children = GetAdjacentNodes(currentNode);

            foreach (Node child in children)
            {
                if (closedList.Contains(child))
                {
                    continue;
                }

                int newG = currentNode.G + GetDistance(currentNode, child);

                if (openList.Contains(child) && newG >= child.G)
                {
                    continue;
                }

                child.G = newG;
                child.H = GetDistance(child, goalNode);
                child.Parent = currentNode;

                if (!openList.Contains(child))
                {
                    openList.Add(child);
                }
            }
        }

        return null;
    }

    private List<Node> GetAdjacentNodes(Node currentNode)
    {
        List<Node> children = new List<Node>();

        int[,] directions =
        {
            { 0, 1 },
            { 1, 0 },
            { 0, -1 },
            { -1, 0 }
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = currentNode.X + directions[i, 0];
            int newY = currentNode.Y + directions[i, 1];

            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
            {
                Node neighbor = grid[newX, newY];

                if (neighbor.Walkable)
                {
                    children.Add(neighbor);
                }
            }
        }

        return children;
    }

    private int GetDistance(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    private List<Node> CreatePath(Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
}