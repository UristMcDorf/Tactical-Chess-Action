using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//SINGLETON
public class Grid : MonoBehaviour {
    static Grid _Instance = null;
    public static Grid Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType<Grid>();
                {
                    if (!_Instance)
                        Debug.LogError("No grid on scene!");
                }
            }
            return _Instance;
        }
    }

    Dictionary<Coordinates, Cell> cells = null;

    void Start()
    {
        GenerateField(new UnitSetup(), new UnitSetup());
    }

    public Cell GetCell(Coordinates coordinates)
    {
        Cell cell = null;

        cells.TryGetValue(coordinates, out cell);

        return cell;
    }

    public void GenerateField(UnitSetup white, UnitSetup black)
    {
        cells = GridGenerator.GenerateCells();

        foreach(Cell cell in cells.Values)
        {
            cell.transform.SetParent(this.transform, false);
        }

        Dictionary<Coordinates, Unit> units = UnitGenerator.GenerateUnits(white, black);

        foreach(Coordinates coordinates in units.Keys)
        {
            cells[coordinates].unit = units[coordinates];
        }
    }

    public void Clear()
    {
        Dictionary<Coordinates, Cell> toRemove = new Dictionary<Coordinates, Cell>(cells);

        foreach (Coordinates coordinates in toRemove.Keys)
        {
            Destroy(cells[coordinates].gameObject);
        }

        cells.Clear();
    }
}