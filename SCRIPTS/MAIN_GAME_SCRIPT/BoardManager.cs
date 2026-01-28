using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Cell cellPrefab;
    public Transform boardArea;

    private int grid_size;

   
    public Cell[] cells;

    void Start()
    {
        grid_size = GameSettings.Instance.grid_size;
        CreateBoard();
    }

    void CreateBoard()
    {
        
        int totalCells = grid_size * grid_size;

      
        cells = new Cell[totalCells];

        for (int i = 0; i < totalCells; i++)
        {
            Cell cell = Instantiate(cellPrefab, boardArea);
            cell.index = i;

            
            cells[i] = cell;
        }
    }
    public void ResetBoardUI()
    {
        foreach (Cell cell in cells)
        {
            cell.ResetCell();
        }
    }

    public Cell GetCell(int index)
    {
        if (cells == null) return null;
        if (index < 0 || index >= cells.Length) return null;
        return cells[index];
    }
}
