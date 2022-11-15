
using UnityEngine;

public class Generator : MonoBehaviour
{
    private int Width = 10;
    private int Height = 10;

    public Maze GenerateMaze(int Width, int Height)
    {
        this.Height = Height;
        this.Width = Width;

        MazeCell[,] cells = new MazeCell[Width, Height];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y };
            }
        }

        Maze maze = new Maze();

        maze.cells = cells;

        return maze;
    }

}
