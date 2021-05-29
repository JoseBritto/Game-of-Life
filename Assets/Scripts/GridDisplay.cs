using UnityEngine;
using UnityEngine.InputSystem;

public class GridDisplay : MonoBehaviour
{

    public GridWorld world;

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

    public void Start()
    {

        grid = new bool[world.Size.x, world.Size.y];

        controls = InputManager.Instance.InputControls;

        controls.InGame.Tap.performed += tap_performed;

        cells = new GameObject[world.Size.x, world.Size.y];

        //StartSim();

        SetUpBg();
    }


    private void SetUpBg()
    {
        BackgroundGrid.localScale = new Vector3(world.Size.x, world.Size.y);
        BackgroundGrid.gameObject.GetComponent<Renderer>().material.mainTextureScale = world.Size;
    }

    public void StartSim()
    {
        world.SimulationInit(grid);
        timeUntilNextSim = 0;

        Started = true;
    }
    public void StopSim()
    {
        grid = world.GridElements;
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
                world.SimulationUpdate();
                timeUntilNextSim = secsPerSim;
            }
            if (Keyboard.current.xKey.wasReleasedThisFrame)
            {
                print("Stop");
                StopSim();
            }
        }
        else if (Keyboard.current.xKey.wasReleasedThisFrame)
        {
            print("Start");
            StartSim();
        }

        var maxX = Started ? world.Size.x : grid.GetLength(0);
        var maxY = Started ? world.Size.y : grid.GetLength(1);
        for (int x = 0; x < maxX; x++)
            for (int y = 0; y < maxY; y++)
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
