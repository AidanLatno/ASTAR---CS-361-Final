

const int size = 5;

// Layout grid: 0 = walkable, 1 = wall
int[,] layoutGrid = new int[size,size]
{
    { 0, 0, 0, 0, 0 },
    { 0, 1, 1, 1, 0 },
    { 0, 1, 0, 0, 0 },
    { 0, 1, 0, 1, 1 },
    { 0, 1, 0, 0, 0 }
};

Node[,] grid = new Node[size, size];

// Fill node grid based on layout grid
for (int x = 0; x < size; x++)
{
    for (int y = 0; y < size; y++)
    {
        grid[x, y] = new Node(x, y, layoutGrid[x, y] == 0);
    }
}

// Create A* instance and find path from (0,0) to (4,4)
AStar aStar = new AStar(grid);
List<Node> path = aStar.FindPath(grid[0, 0], grid[4, 4]);

// Print the path
if (path != null)
{
    foreach (Node node in path)
    {
        Console.WriteLine($"({node.X}, {node.Y})");
    }
}
else
{
    Console.WriteLine("No path found");
}

// Mark the path for visualization
foreach (Node node in path)
{
    layoutGrid[node.X, node.Y] = 2;
}


// Show path on the grid visually
for (int x = 0; x < size; x++)
{
    for (int y = 0; y < size; y++)
    {
        if(layoutGrid[x, y] == 0)
            Console.Write(". ");
        else if(layoutGrid[x, y] == 1)
            Console.Write("# ");
        else if(layoutGrid[x, y] == 2)
            Console.Write("o ");
    }
    Console.WriteLine();
}