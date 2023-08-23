using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    private int _width = 10;
    private int _height = 10;


    public Maze GenerateMaze(int width, int height)
    {
        _height = height;
        _width = width;

        var maze = new MazeCell[width, height];

        for (var x = 0; x < maze.GetLength(0); x++)
        {
            for (var y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeCell { X = x, Y = y };
            }
        }

        var generateMaze = new Maze();

        defaultGenerator(maze);


        generateMaze.Cells = maze;

        return generateMaze;
    }

    public Maze GenerateMaze_AldousBroder(int width, int height)
    {
        _height = height;
        _width = width;

        var maze = new MazeCell[width, height];

        for (var x = 0; x < maze.GetLength(0); x++)
        {
            for (var y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeCell { X = x, Y = y };
            }
        }

        var generateMaze = new Maze();

        modefideAldousBroder(maze);
        ReCalcDist(maze);
        createExit(maze);

        generateMaze.Cells = maze;

        return generateMaze;
    }

    private void createExit(MazeCell[,] maze)
    {
        var founded = false;
        var c = new MazeCell();
        var maxD = (from MazeCell cell in maze select cell.Distance).Prepend(0).Max();

        foreach (var cell in maze)
        {
            if (cell.Distance != maxD) continue;
            var w = -1;
            Debug.Log("x: " + cell.X + "; y: " + cell.Y + ";");
            Debug.Log("D: " + cell.Distance + ";");
            if (cell.X == _width - 1)
            {
                if (cell.Y == 0)
                {
                    w = Random.Range(0, 2) switch
                    {
                        0 => 1,
                        1 => 2
                    };
                }
                else if (cell.Y == _height - 1)
                {
                    w = Random.Range(0, 2) switch
                    {
                        0 => 1,
                        1 => 0
                    };
                }
                else
                {
                    w = 1;
                }
            }
            else if (cell.X == 0)
            {
                if (cell.Y == 0)
                {
                    w = Random.Range(0, 2) switch
                    {
                        0 => 3,
                        1 => 2
                    };
                }
                else if (cell.Y == _height - 1)
                {
                    w = Random.Range(0, 2) switch
                    {
                        0 => 3,
                        1 => 0
                    };
                }
                else
                {
                    w = 3;
                }
            }

            if (0 < cell.X && cell.X < _width)
            {
                if (cell.Y == 0) w = 2;
                else if (cell.Y == _height - 1) w = 0;
            }
            
            RemoveWall(cell, w);
        }
    }


    private void defaultGenerator(MazeCell[,] maze)
    {
        RemoveWalls(maze);
        AddCycling(maze);
        ReCalcDist(maze);
    }

    private void modefideAldousBroder(MazeCell[,] maze)
    {
        var current = new MazeCell();
        var chosen = new MazeCell();
        var k = 0;
        do
        {
            current = GRanMCell(maze);
            if (k == 0)
            {
                current.Visited = true;
                current.Distance = k;
                k++;
            }


            if (k > 0 && !current.Visited) continue;


            var gWall = GWall(current);
            if (gWall.Item1 == 0) continue;
            if (!gWall.Item2) continue;
            chosen = GCellByWall(gWall.Item1, current, maze);
            if (!chosen.Visited)
            {
                RemoveWall(current, chosen);
                chosen.Visited = true;
                chosen.Distance = k;
            }

            k++;
        } while (!AllIsVisited(maze));

        foreach (var cell in maze)
        {
            cell.Visited = false;
        }
    }

    private bool AllIsVisited(MazeCell[,] maze)
    {
        foreach (var c in maze)
        {
            if (c.Visited) continue;
            return c.Visited;
        }

        return true;
    }

    private int CountVisited(MazeCell[,] maze)
    {
        var i = 0;
        foreach (var c in maze)
        {
            if (c.Visited) i++;
        }

        return i;
    }


    private void ReCalcDist(MazeCell[,] maze)
    {
        foreach (var cell in maze)
        {
            cell.Visited = cell.Distance == 0;
            if (cell.Distance > 0) cell.Distance = -1;
        }

        var k = 0;
        do
        {
            var cellList = GetCellWDist(maze, k);
            foreach (var start in cellList)
            {
                if (!start.Bottom && start.Y >= 0 && !maze[start.X, start.Y - 1].Visited)
                {
                    maze[start.X, start.Y - 1].Distance = start.Distance + 1;
                    maze[start.X, start.Y - 1].Visited = true;
                }

                if (!start.Up && start.Y <= _height - 1 && !maze[start.X, start.Y + 1].Visited)
                {
                    maze[start.X, start.Y + 1].Distance = start.Distance + 1;
                    maze[start.X, start.Y + 1].Visited = true;
                }

                if (!start.Left && start.X >= 0 && !maze[start.X - 1, start.Y].Visited)
                {
                    maze[start.X - 1, start.Y].Distance = start.Distance + 1;
                    maze[start.X - 1, start.Y].Visited = true;
                }

                if (!start.Right && start.Y <= _width && !maze[start.X + 1, start.Y].Visited)
                {
                    maze[start.X + 1, start.Y].Distance = start.Distance + 1;
                    maze[start.X + 1, start.Y].Visited = true;
                }
            }

            k++;
        } while (GetCellWDist(maze, k).Count > 0);
    }

    private static List<MazeCell> GetCellWDist(MazeCell[,] maze, int k)
    {
        var CellList = new List<MazeCell>();
        foreach (var cell in maze)
        {
            if (cell.Distance == k) CellList.Add(cell);
        }

        return CellList;
    }

    private bool GCountUnNameCell(MazeCell[,] m)
    {
        foreach (var c in m)
        {
            if (c.Distance == -1) return true;
        }

        return false;
    }

    private void RemoveWalls(MazeCell[,] maze)
    {
        var i = 0;
        var current = GRanMCell(maze);
        current.Visited = true;
        current.Distance = i;
        i++;

        var stack = new Stack<MazeCell>();
        do
        {
            var unvisitedNeighbours = new List<MazeCell>();

            var x = current.X;
            var y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < _width - 1 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < _height - 1 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                var chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];

                chosen.Distance = i;


                RemoveWall(current, chosen);

                chosen.Visited = true;
                stack.Push(chosen);

                current = chosen;
                i++;
            }
            else
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);
    }

    private void AddCycling(MazeCell[,] maze)
    {
        var countWallRemove = _width switch
        {
            <= 4 when _height <= 4 => 1,
            <= 7 when _height <= 7 => 4,
            _ => 10
        };

        do
        {
            var cur = GRanMCell(maze);
            var gWall = GWall(cur);
            if (gWall.Item1 == 0) continue;
            if (!gWall.Item2) continue;
            var chu = GCellByWall(gWall.Item1, cur, maze);
            if (Math.Abs(cur.Distance - chu.Distance) < 4) continue;

            RemoveWall(cur, chu);
            countWallRemove--;
        } while (countWallRemove > 0);
    }

    private (int, bool) GWall(MazeCell cur)
    {
        var i = Random.Range(1, 5);
        switch (i)
        {
            case 1:
                if (cur.Y >= _height - 1) break;
                return (i, cur.Up);
            case 2:
                if (cur.X >= _width - 1) break;
                return (i, cur.Right);
            case 3:
                if (cur.Y == 0) break;
                return (i, cur.Bottom);
            case 4:
                if (cur.X == 0) break;
                return (i, cur.Left);
        }

        return (0, false);
    }

    private MazeCell GRanMCell(MazeCell[,] maze)
    {
        return maze[Random.Range(0, _width), Random.Range(0, _height)];
    }

    private static MazeCell GCellByWall(int i, MazeCell current, MazeCell[,] maze)
    {
        return i switch
        {
            1 => maze[current.X, current.Y + 1],
            2 => maze[current.X + 1, current.Y],
            3 => maze[current.X, current.Y - 1],
            4 => maze[current.X - 1, current.Y],
            _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
        };
    }

    private static void RemoveWall(MazeCell current, int w)
    {
        switch (w)
        {
            case 0:
                current.Up = false;
                break;
            case 1:
                current.Right = false;
                break;
            case 2:
                current.Bottom = false;
                break;
            case 3:
                current.Left = false;
                break;
            default:
                Debug.Log("kek");
                break;
        }
    }

    private static void RemoveWall(MazeCell current, MazeCell choose)
    {
        if (current.X == choose.X)
        {
            if (current.Y > choose.Y)
            {
                current.Bottom = false;
                choose.Up = false;
            }
            else
            {
                choose.Bottom = false;
                current.Up = false;
            }
        }
        else
        {
            if (current.X > choose.X)
            {
                current.Left = false;
                choose.Right = false;
            }
            else
            {
                choose.Left = false;
                current.Right = false;
            }
        }
    }
}