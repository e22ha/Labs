using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    private int _width = 10;
    private int _height = 10;
    private bool _start = true;

    public Maze GenerateMaze(int width, int height)
    {
        _height = height;
        _width = width;

        var maze = new MazeCell[width, height];

        for (var x = 0; x < maze.GetLength(0); x++)
        {
            for (var y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeCell { x = x, y = y };
            }
        }

        var _maze = new Maze();

        RemoveWalls(maze);
        AddCycling(maze);

        _maze.Cells = maze;

        return _maze;
    }

    private void RemoveWalls(MazeCell[,] maze)
    {
        var i = 0;
        var current = GRanMCell(maze);
        current.visited = true;
        current.distance = i;
        i++;

        var stack = new Stack<MazeCell>();
        do
        {
            var unvisitedNeighbours = new List<MazeCell>();

            var x = current.x;
            var y = current.y;

            if (x > 0 && !maze[x - 1, y].visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < _width - 1 && !maze[x + 1, y].visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < _height - 1 && !maze[x, y + 1].visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                var chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];

                chosen.distance = i;


                RemoveWall(current, chosen);

                chosen.visited = true;
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
        int countWallRemove = _width switch
        {
            <= 4 when _height <= 4 => 1,
            <= 7 when _height <= 7 => 4,
            _ => 10
        };

        do
        {
            var cur = GRanMCell(maze);
            var _cur = GWall(cur);
            if (!_cur[1] || (cur.y <= 0&&cur.x<=0)) continue;
            var chu = GCellByWall(_cur);
            if (Math.Abs(cur.distance - chu.distance) < 4) continue;
            RemoveWall(cur, chu);
            countWallRemove--;
        } while (countWallRemove > 0);
    }

    private static (int, bool) GWall(MazeCell cur)
    {
        var i = Random.Range(1, 5);
        switch (i)
        {
            case 1:
                return (i, cur.up);
            case 2:
                return (i,cur.right);
            case 3:
                return (i,cur.bottom);
            case 4:
                return (i,cur.left);
            default:
                return (0,false);
        }
    }

    private MazeCell GRanMCell(MazeCell[,] maze)
    {
        return maze[Random.Range(0, _width), Random.Range(0, _height)];
    }

    private MazeCell GCellByWall((int, bool),MazeCell[,] maze)
    {
        return maze[Random.Range(0, 1), Random.Range(0, 1)];
    }

    private static void RemoveWall(MazeCell current, MazeCell choose)
    {
        if (current.x == choose.x)
        {
            if (current.y > choose.y)
            {
                current.bottom = false;
                choose.up = false;
            }
            else
            {
                choose.bottom = false;
                current.up = false;
            }
        }
        else
        {
            if (current.x > choose.x)
            {
                current.left = false;
                choose.right = false;
            }
            else
            {
                choose.left = false;
                current.right = false;
            }
        }
    }
}