using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Camera cam;
    public GameObject mazeHandler;

    public Cell CellPrefub;
    public Vector2 CellSize = new(1, 1);

    public int Width = 10;
    public int Height = 10;

    public void GenerateMaze()
    {
        foreach (Transform child in mazeHandler.transform)
            GameObject.Destroy(child.gameObject);

        var generator = new Generator();
        var maze = generator.GenerateMaze(Width, Height);

        for (var x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (var z = 0; z < maze.cells.GetLength(1); z++)
            {
                var c = Instantiate(CellPrefub, new Vector3(x * CellSize.x, 0, z * CellSize.y), Quaternion.identity);
                
                if (maze.cells[x, z].Left == false)
                    Destroy(c.Left);
                if (maze.cells[x, z].Right == false)
                    Destroy(c.Right);
                if (maze.cells[x, z].Up == false)
                    Destroy(c.Up);
                if (maze.cells[x, z].Bottom == false)
                    Destroy(c.Bottom);

                c.transform.parent = mazeHandler.transform;
            }
        }

        cam.transform.position =
            new Vector3((Width * CellSize.x) / 2, Mathf.Max(Width, Height) * 8, (Height * CellSize.y) / 2);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
