using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridDisplay : MonoBehaviour
{

    public GridWorldV3 world;

    public Transform BackgroundGrid;


    public bool[,] grid;

    public CellPool cellPool;

    [SerializeField]

    GameObject[,] cells;

    Controls controls;


    public float SimsPerSecond = 5;

    private float secsPerSim => 1 / SimsPerSecond;

    float timeUntilNextSim;

    bool Started;
    [SerializeField]
    bool randomGrid;
    [SerializeField]
    int seed;

    bool canStart = true;

    public Camera cam;

    public void Start()
    {
        
        world = GetComponent<GridWorldV3>();

        grid = new bool[world.Size.x, world.Size.y];

        if (randomGrid)
            StartCoroutine(GenerateGrid());

        controls = InputManager.Instance.InputControls;

        controls.InGame.Tap.performed += tap_performed;

        cells = new GameObject[world.Size.x, world.Size.y];

        //StartSim();

        SetUpBg();
    }

    IEnumerator GenerateGrid()
    {
        canStart = false;

        System.Random rand;
        if (seed != 0)
            rand = new System.Random(seed);
        else
            rand = new System.Random();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = rand.Next(0, 2) == 1;
               // print("Generating " + i + " " + j);
            }
            yield return null;
        }
        canStart = true;
    }


    private void SetUpBg()
    {
        BackgroundGrid.localScale = new Vector3(world.Size.x, world.Size.y);
        BackgroundGrid.gameObject.GetComponent<Renderer>().material.mainTextureScale = world.Size;
        BackgroundGrid.position = new Vector3(Mathf.Floor(BackgroundGrid.localScale.x / 2), Mathf.Floor(BackgroundGrid.localScale.y / 2));
        Camera.main.transform.position = new Vector3(BackgroundGrid.transform.position.x, BackgroundGrid.transform.position.y, Camera.main.transform.position.z);
    }

    public void StartSim()
    {
        world.SimulationInit(grid);
        timeUntilNextSim = 0;

        Started = true;
    }
    public void PauseSim()
    {
        grid = world.GetElementsAsArray();
        Started = false;
    }

    private void tap_performed(InputAction.CallbackContext obj)
    {
        Vector2 inp = controls.InGame.PointerPosition.ReadValue<Vector2>();

        inp = Camera.main.ScreenToWorldPoint(new Vector3(inp.x, inp.y));

        Vector2Int vInt = new Vector2Int(Mathf.RoundToInt(inp.x), Mathf.RoundToInt(inp.y));

        print(vInt);

        if (vInt.x >= world.Size.x || vInt.y >= world.Size.y)
            return;
        if (vInt.x < 0 || vInt.y < 0)
            return;
        grid[vInt.x, vInt.y] = !grid[vInt.x, vInt.y];

        if(Started)
            world.SetElement(vInt.x, vInt.y, grid[vInt.x, vInt.y]);
    }

    private void Update()
    {
        if (Started)
        {
            timeUntilNextSim -= Time.deltaTime;

            if (timeUntilNextSim <= 0)
            {
                int i = (int) (Time.deltaTime / secsPerSim);
                do
                {
                    world.SimulationUpdate();
                    i--;
                } while (i > 0);

                timeUntilNextSim = secsPerSim;
            }
            if (Keyboard.current.xKey.wasReleasedThisFrame)
            {
                print("Stop");
                PauseSim();
            }
        }
        else if (Keyboard.current.xKey.wasReleasedThisFrame)
        {
            if (canStart)
            {
                print("Start");
                StartSim();
            }
            else
                print("Wait for world generation!!");
        }

        var maxX = Started ? world.Size.x : grid.GetLength(0);
        var maxY = Started ? world.Size.y : grid.GetLength(1);

        var vertExtent = cam.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        var startX = cam.transform.position.x - horzExtent;
        var endX = cam.transform.position.x + horzExtent;

        var startY = cam.transform.position.y - vertExtent;
        var endY = cam.transform.position.y + vertExtent;

        for (int x = (int)Mathf.Max(0, startX); x < Mathf.Min(maxX, endX); x++)
            for (int y = (int)Mathf.Max(0, startY); y < Mathf.Min(maxY, endY); y++)
            {
                var element = Started ? world.GetElement(x, y) : grid[x, y];

                if (element && cells[x, y] == null)
                {
                    cells[x, y] = cellPool.GetCell();
                    cells[x, y].transform.position = new Vector3(x, y);
                }
                else if (!element)
                {
                    if (cells[x, y] != null)
                    {
                        cellPool.HideCell(cells[x, y]);
                        cells[x, y] = null;
                    }
                }
            }






    }

}
