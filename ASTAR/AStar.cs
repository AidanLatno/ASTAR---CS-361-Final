using System;
using System.Collections.Generic;
using System.Linq;

// This code defines a Node class to represent each cell in the grid
// along with its properties for the A* algorithm (G, H, F, and Parent Node).
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

// Main A* class that implements the pathfinding algorithm.
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

    // Main method to find the path from startNode to goalNode using A* algorithm.
    public List<Node> FindPath(Node startNode, Node goalNode)
    {
        // Open list to keep track of nodes to be evaluated and closed list for nodes already evaluated.
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        startNode.G = 0;
        startNode.H = GetDistance(startNode, goalNode);
        openList.Add(startNode);

        // Loop until there are nodes to evaluate in the open list.
        while (openList.Count > 0)
        {
            // Select the node with the lowest F score from the open list.
            Node currentNode = openList.OrderBy(node => node.F).ThenBy(node => node.H).First();

            // Update the open and closed lists.
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If the goal node is reached, create and return the path by backtracking from the goal node to the start node.
            if (currentNode == goalNode)
            {
                return CreatePath(currentNode);
            }
            
            // Get the adjacent nodes (up, down, left, right) of the current node.
            List<Node> children = GetAdjacentNodes(currentNode);

            foreach (Node child in children)
            {
                // If the child node is already in the closed list, skip it.
                if (closedList.Contains(child))
                {
                    continue;
                }

                // Calculate the G cost for the child node and update its properties if it's a better path.
                int newG = currentNode.G + GetDistance(currentNode, child);

                // If the child node is already in the open list and the new G cost is not better, skip it.
                if (openList.Contains(child) && newG >= child.G)
                {
                    continue;
                }

                // Update the child node's G, H, and Parent properties.
                child.G = newG;
                child.H = GetDistance(child, goalNode);
                child.Parent = currentNode;

                // If the child node is not in the open list, add it to the open list for further evaluation.
                if (!openList.Contains(child))
                {
                    openList.Add(child);
                }
            }
        }
        // If the open list is empty and the goal node was not reached, return null to show no path exists.
        return null;
    }

    // Gets the adjacent nodes (up, down, left, right) of the current node and checks if they are walkable.
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

    // Simple.
    private int GetDistance(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    // This method backtracks from the goal node to the start node using the Parent of each node to create the path.
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