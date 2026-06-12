Node[,] grid = new Node[5, 5];

for (int x = 0; x < 5; x++)
{
    for (int y = 0; y < 5; y++)
    {
        grid[x, y] = new Node(x, y, true);
    }
}

grid[2, 2].Walkable = false;

AStar aStar = new AStar(grid);

List<Node> path = aStar.FindPath(grid[0, 0], grid[4, 4]);

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