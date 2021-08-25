using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridWorldV2 : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size = new Vector2Int(10, 10);

    public Vector2Int Size => size;

    private HashSet<int> liveCells;

    private int size1D;
    public void SimulationInit(bool[,] grid)
    {
        size = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
        size1D = size.x * size.y;
        
        liveCells = new HashSet<int>();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j])
                    liveCells.Add(to1D(i, j));
            }
        }
    }

    public void SetElement(int x, int y, bool value)
    {
        var pos = to1D(x, y);
        
        if (pos < 0 || pos >= size1D)
            return;

        if (value)
            liveCells.Add(pos);
        else
            liveCells.Remove(pos);
    }

/*

    public void SetElement(int i, bool value)
    {
        if (i >= gridElements.Length)
        {
            Debug.LogError("Index out of bounds for GridElements");
            return;
        }

        changed.Add(i);

        gridElements[i] = value;
    }*/

    public bool[,] GetElementsAsArray()
    {
        var array = new bool[Size.x, Size.y];

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                array[i, j] = liveCells.Contains(to1D(i, j));
            }
        }

        return array;
    }

    public bool GetElement(int x, int y)
    {
        return liveCells.Contains(to1D(x,y));
    }



    public void SimulationUpdate()
    {
        var temp = new HashSet<int>();

       
        foreach (var cell in liveCells)
        {
            int liveNeibours = getAndUpdateNeibours(cell, temp, true);

            if (!shouldDie(liveNeibours))
                temp.Add(cell);
        }

        liveCells = temp;
    }

    private int getAndUpdateNeibours(int cell, HashSet<int> temp, bool updateDead)
    {
        int live = 0;

        //Left
        if (liveCells.Contains(cell - 1))
            live++;
        else if (updateDead)
        {
            update(cell - 1);
        }

        //Rigit
        if (liveCells.Contains(cell + 1))
            live++;
        else if (updateDead)
        {
            update(cell + 1);
        }

        //Top
        if (liveCells.Contains(cell - Size.x))
            live++;
        else if (updateDead)
        {
            update(cell - Size.x);
        }

        //Top Left
        if (liveCells.Contains(cell - Size.x - 1))
            live++;
        else if (updateDead)
        {
            update(cell - Size.x - 1);
        }

        //Top Rigt
        if (liveCells.Contains(cell - Size.x + 1))
            live++;
        else if (updateDead)
        {
            update(cell - Size.x + 1);
        }

        //Bottom
        if (liveCells.Contains(cell + Size.x))
            live++;
        else if (updateDead)
        {
            update(cell + Size.x);
        }

        //Bottom Left
        if (liveCells.Contains(cell + Size.x - 1))
            live++;
        else if (updateDead)
        {
             update(cell + Size.x - 1);
        }

        //Bottom Right
        if (liveCells.Contains(cell + Size.x + 1))
            live++;
        else if (updateDead)
        {
             update(cell + Size.x + 1);
        }

        return live;

        void update(int cellPos)
        {
            if (cellPos >= size1D || cell < 0)
                return;

            if (shouldBecomeAlive(getAndUpdateNeibours(cellPos, temp, false)))
            {
                temp.Add(cellPos);
            }
        }
    }

    private bool shouldDie(int liveNeibours)
    {
        //Over or underpopulation
        if (liveNeibours > 3 || liveNeibours < 2)
            return true;

        return false;
    }

    private bool shouldBecomeAlive(int liveNeibours)
    {
        return liveNeibours == 3;
    }
   

    private int to1D(int x, int y) => x + (size.x * y);

   

    /* private int getLiveNeighbours(int i, out List<int> neibours)
     {
         int live = 0;

         neibours = new List<int>();

         if (i > 0)
         {
             neibours.Add(i - 1);
             if (gridElements[i - 1]) live++; //Left

             if (i - size.x + 1 >= 0)
             {
                 neibours.Add(i - size.x + 1);
                 if (gridElements[i - size.x + 1]) live++; //Upper Right Diag

                 if (i - size.x >= 0)
                 {
                     neibours.Add(i - size.x);
                     if (gridElements[i - size.x]) live++; //Above

                     if (i - size.x - 1 >= 0)
                     {
                         neibours.Add(i - size.x - 1);
                         if (gridElements[i - size.x - 1]) live++;  //Upper Left Diag
                     }
                 }
             }
         }


         if (i < gridElements.Length - 1)
         {
             neibours.Add(i + 1);
             if (gridElements[i + 1]) live++; //Right


             if (i + size.x - 1 < gridElements.Length)
             {
                 neibours.Add(i + size.x - 1);
                 if (gridElements[i + size.x - 1]) live++; //Lower Left Diag

                 if (i + size.x < gridElements.Length)
                 {
                     neibours.Add(i + size.x);
                     if (gridElements[i + size.x]) live++; //Below

                     if (i + size.x + 1 < gridElements.Length)
                     {
                         neibours.Add(i + size.x + 1);
                         if (gridElements[i + size.x + 1]) live++; //Lower Right Diag
                     }
                 }
             }
         }

         return live;
     }*/

    
}
