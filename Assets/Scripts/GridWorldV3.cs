using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWorldV3 : MonoBehaviour
{

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = new Vector2Int(10, 10);

    private int size1D;

    private byte[] cellBuffer1;
    private byte[] cellBuffer2;
    private byte[] currentCells;

    public void SimulationInit(bool[,] grid)
    {
        Size = new Vector2Int(grid.GetLength(0), grid.GetLength(1));
        size1D = Size.x * Size.y;

        cellBuffer1 = new byte[size1D];
        cellBuffer2 = new byte[size1D];
        currentCells = cellBuffer1;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                SetElement(i, j, grid[i, j]);
            }
        }

        
    }
    public void SetElement(int loc, byte[] readFrom, byte[] writeTo,bool value) 
    {
        if (value == GetElement(loc))
            return;

        // Update neibours neibour count

        update(loc - 1); // Left
        update(loc + 1); // Right
        update(loc - Size.x); // Top
        update(loc - Size.x - 1); // Top-Left
        update(loc - Size.x + 1); // Top-Right
        update(loc + Size.x); // Bottom
        update(loc + Size.x - 1); // Bottom Left
        update(loc + Size.x + 1); // Bottom Right

        if(value)
            writeTo[loc] = (byte)(writeTo[loc] | 1);
        else
            writeTo[loc] = (byte)(writeTo[loc] & 0b11111110);

        void update(int i)
        {
            if(i >= 0 && i < size1D)
            {
                if (value)
                {
                    writeTo[i] = (byte)(readFrom[i] + 10); // Last bit is used to store whether that cell is live or dead. So we add 10 and not 1
                }
                else if(readFrom[i] >= 10)
                {
                    writeTo[i] = (byte)(readFrom[i] - 10);                    
                }
                    
            }
        }
    }

    public void SetElement(int x, int y, bool value)
    {
        
        int loc = to1D(x, y);

        SetElement(loc, currentCells, currentCells, value);
        
    }


    public bool[,] GetElementsAsArray()
    {
        var array = new bool[Size.x, Size.y];

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                array[i, j] = GetElement(i, j);
            }
        }

        return array;
    }

    public bool GetElement(int i)
    {
        return (currentCells[i] & 0b1) == 1;
    }

    public bool GetElement(int x, int y)
    {
        return (currentCells[to1D(x, y)] & 0b1) == 1;
    }

    public void SimulationUpdate()
    {
        byte[] newBuffer;
        byte[] oldBuffer;
        if(ReferenceEquals(currentCells, cellBuffer1))
        {
            newBuffer = cellBuffer2;
            oldBuffer = cellBuffer1;
        }
        else
        {
            newBuffer = cellBuffer1;
            oldBuffer = cellBuffer2;
        }

        newBuffer.Initialize();
        currentCells = newBuffer;

        for(int i = 0; i < oldBuffer.Length; i++)
        {
            var b = oldBuffer[i];
            if (b == 0)
                continue;

            var neibours = b >> 1;
            bool alive = (b & 0b1) == 1;

            if(!alive)
            {
                if (neibours == 3)
                    SetElement(i, readFrom: oldBuffer,writeTo: newBuffer,true);

                continue;
            }

            if(neibours < 2 || neibours > 3)
                SetElement(i, readFrom: oldBuffer,writeTo: newBuffer,false);
        }
    }

    private int to1D(int x, int y) => x + (Size.x * y);

}
