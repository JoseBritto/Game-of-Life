using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWorld : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size = new Vector2Int(10, 10);

    private bool[,] gridElements;

    public Vector2Int Size => size;


    public void SetGridSize(Vector2Int newSize)
    {

        if (newSize == size)
            return;
        size = newSize;
        UpdateGridSize();
    }

    public void UpdateGridSize()
    {
        if (gridElements.Length == (size.x * size.y))
            return;

        int xLength = Math.Min(gridElements.GetLength(0), size.x);

        int yLength = Math.Min(gridElements.GetLength(1), size.y);

        var newGrid = new bool[size.x, size.y];

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                newGrid[x, y] = gridElements[x, y];
            }
        }

        gridElements = newGrid;
    }

    public void SetElement(int x, int y, bool value)
    {
        if(gridElements.GetLength(0) <= x || gridElements.GetLength(1) <= y)
        {
            Debug.LogError("Index out of bounds for GridElements");
            return;
        }

        gridElements[x, y] = value;
    }

    public bool GetElement(int x, int y)
    {
        if (gridElements.GetLength(0) <= x || gridElements.GetLength(1) <= y)
        {
            Debug.LogError("Index out of bounds for GridElements");
            return false;
        }

        return gridElements[x, y];

    }

    public void SimulationInit(bool[,] grid = null)
    {
        if(grid == null)
        {
            gridElements = new bool[size.x, size.y];
            return;
        }
        size = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
        gridElements = grid;
    }
    public void SimulationUpdate()
    {
        var nextGen = new bool[gridElements.GetLength(0), gridElements.GetLength(1)];

        for (int x = 0; x < gridElements.GetLength(0); x++)
        {
            for (int y = 0; y < gridElements.GetLength(1); y++)
            {
                if(gridElements[x,y])//cell is alive
                {
                    var n = getLiveNeighbours(gridElements, x, y);
                    if (n < 2)
                    {
                        //print($"killing ({x},{y})");
                        nextGen[x, y] = false;
                    }
                    else if (n == 2 || n == 3)
                        nextGen[x, y] = true;
                    else
                        nextGen[x, y] = false;
                }
                else  //cell is dead
                {      

                    if (getLiveNeighbours(gridElements, x, y) == 3)
                    {
                      //  print($"live: ({x},{y})");
                        nextGen[x, y] = true;

                    }
                    else
                        nextGen[x, y] = false;

                }
            }
        }


        gridElements = nextGen;
    }

   

    private int getLiveNeighbours(bool[,] elements, int i, int j)
    {
        var rowLimit = elements.GetLength(0) - 1;
        var columnLimit = elements.GetLength(1) - 1;
        
        int liveNeighbours = 0;

        for (var x = Math.Max(0, i - 1); x <= Math.Min(i + 1, rowLimit); x++)
        {
            for (var y = Math.Max(0, j - 1); y <= Math.Min(j + 1, columnLimit); y++)
            {
                if (x != i || y != j)
                {
                    if (elements[x, y])
                        liveNeighbours++;
                }
            }
        }

        return liveNeighbours;
    }


    public bool[,] GetElementsAsArray()
    {
        return gridElements;
    }
}
