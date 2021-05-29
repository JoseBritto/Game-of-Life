using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPool : MonoBehaviour
{

    [SerializeField]
    private GameObject cell;

    [SerializeField] Transform ParentOfCells;

    Queue<GameObject> cells;

    private void Start()
    {
        cells = new Queue<GameObject>();
        cells.Enqueue(createCell());    
    }

    public GameObject GetCell()
    {
        if (cells.Count <= 0)
        {
            return createCell();
        }
        else
        {
            var cell = cells.Dequeue();
            cell.SetActive(true);
            return cell;
        }

    }

    public void HideCell(GameObject cell)
    {
        cells.Enqueue(cell);

        cell.SetActive(false);
    }

    private GameObject createCell()
    {
        return Instantiate(cell, ParentOfCells);
    }


}
