using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridDisplay : MonoBehaviour
{

    public GridWorld world;

    public bool[,] grid;

    [SerializeField]
    GameObject cell;

    [SerializeField]
    GameObject dead;

    GameObject[,] cells;
    GameObject[,] deads;

    Controls controls;

    bool Started;

    public void Start()
    {

        grid = new bool[world.Size.x, world.Size.y];

        controls = new Controls();

        controls.Enable();

        controls.InGame.Enable();

        controls.InGame.Tap.performed += tap_performed;

        StartSim();
    }

    public void StartSim()
    {
        world.SimulationInit(grid);
        cells = new GameObject[world.Size.x, world.Size.y];
        deads = new GameObject[world.Size.x, world.Size.y];
        for (int x = 0; x < world.Size.x; x++)
        {
            for (int y = 0; y < world.Size.y; y++)
            {
                cells[x, y] = Instantiate(cell, new Vector3(x, y, 0), cell.transform.rotation);
                cells[x, y].SetActive(world.GetElement(x, y));

                deads[x, y] = Instantiate(dead, new Vector3(x, y, 0), dead.transform.rotation);
                deads[x, y].SetActive(!world.GetElement(x, y));
            }

        }
        Started = true;
    }

    private void tap_performed(InputAction.CallbackContext obj)
    {
        Vector2 inp = controls.InGame.PointerPosition.ReadValue<Vector2>();

        inp = Camera.main.ScreenToWorldPoint(new Vector3(inp.x,inp.y));

        Vector2Int vInt = new Vector2Int(Mathf.RoundToInt(inp.x), Mathf.RoundToInt(inp.y));

        print(vInt);

        if (vInt.x >= world.Size.x || vInt.y >= world.Size.y)
            return;
        if (vInt.x < 0 || vInt.y < 0)
            return;
        grid[vInt.x, vInt.y] = !grid[vInt.x, vInt.y];
        world.SetElement(vInt.x, vInt.y, grid[vInt.x, vInt.y]);
    }

    private void Update()
    {
        if (Keyboard.current.xKey.wasReleasedThisFrame)
        {
            print("Update");
            world.SimulationUpdate();

            
        }


        for (int x = 0; x < world.Size.x; x++)
        {
            for (int y = 0; y < world.Size.y; y++)
            {
                cells[x, y].SetActive(world.GetElement(x, y));
                deads[x, y].SetActive(!world.GetElement(x, y));
            }

        }

    }

}
