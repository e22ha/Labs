using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Camera cam;
    public GameObject mazeHandler;

    public Cell cellPrefub;
    public Vector2 cellSize = new(1, 1);

    public int width = 10;
    public int height = 10;

    public void GenerateMaze()
    {
        foreach (Transform child in mazeHandler.transform)
            Destroy(child.gameObject);

        var generator = new Generator();
        var maze = generator.GenerateMaze(width, height);

        for (var x = 0; x < maze.Cells.GetLength(0); x++)
        {
            for (var z = 0; z < maze.Cells.GetLength(1); z++)
            {
                var c = Instantiate(cellPrefub, new Vector3(x * cellSize.x, 0, z * cellSize.y), Quaternion.identity);
                
                if (maze.Cells[x, z].Left == false)
                    Destroy(c.left);
                if (maze.Cells[x, z].Right == false)
                    Destroy(c.right);
                if (maze.Cells[x, z].Up == false)
                    Destroy(c.up);
                if (maze.Cells[x, z].Bottom == false)
                    Destroy(c.bottom);

                c.transform.parent = mazeHandler.transform;
                c.distance.text = maze.Cells[x, z].Distance.ToString();
            }
        }

        cam.transform.position =
            new Vector3((width * cellSize.x) / 2, Mathf.Max(width, height) * 2, (height * cellSize.y) / 2);
    }
}