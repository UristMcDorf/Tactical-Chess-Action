using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GridGenerator {
    const string cellPrefabPath = "Prefabs/Cell";
    static Cell _cellPrefab = null;
    static Cell cellPrefab
    {
        get
        {
            if (!_cellPrefab)
            {
                _cellPrefab = Resources.Load<Cell>(cellPrefabPath);
                {
                    if (!_cellPrefab)
                        Debug.LogError("Couldn't load cell prefab!");
                }
            }
            return _cellPrefab;
        }
    }

    public static Dictionary<Coordinates, Cell> GenerateCells(int xDimension = Constants.k_xGridDimension, int yDimension = Constants.k_yGridDimension)
    {
        Dictionary<Coordinates, Cell> newCells = new Dictionary<Coordinates, Cell>();
        Cell newCell = null;

        for (int i = 0; i < xDimension; i++)
        {
            for (int j = 0; j < yDimension; j++)
            {
                newCell = (Cell)GameObject.Instantiate(cellPrefab);

                newCell.coordinates = new Coordinates(i, j);

                newCells.Add(newCell.coordinates, newCell);
            }
        }

        return newCells;
    }
}
