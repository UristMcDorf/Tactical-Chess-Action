using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class UnitGenerator {
    const string unitPrefabPath = "Prefabs/Units";

    static Dictionary<string, Unit> _unitPrefabs = new Dictionary<string, Unit>();
    static Dictionary<string, Unit> unitPrefabs
    {
        get
        {
            if (_unitPrefabs.Count == 0)
            {
                List<Unit> units = new List<Unit>(Resources.LoadAll<Unit>(unitPrefabPath));

                foreach (Unit unit in units)
                {
                    _unitPrefabs.Add(unit.name, unit);
                }

                {
                    if (_unitPrefabs.Count == 0)
                        Debug.LogError("Couldn't load unit prefabs!");
                }
            }
            return _unitPrefabs;
        }
    }

    public static Dictionary<Coordinates, Unit> GenerateUnits()
    {
        return GenerateUnits(new UnitSetup(), new UnitSetup());
    }

    public static Dictionary<Coordinates, Unit> GenerateUnits(UnitSetup white, UnitSetup black) //TODO add loading from .txt file; unhardcode sides
    {
        Dictionary<Coordinates, Unit> newUnits = new Dictionary<Coordinates, Unit>();

        Unit newUnit = null;

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Rook")]);
        newUnit.Initialise(white.GetOriginAtType("Rook"), "Rook", "White");
        newUnits.Add(new Coordinates(0, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Knight")]);
        newUnit.Initialise(white.GetOriginAtType("Knight"), "Knight", "White");
        newUnits.Add(new Coordinates(1, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Bishop")]);
        newUnit.Initialise(white.GetOriginAtType("Bishop"), "Bishop", "White");
        newUnits.Add(new Coordinates(2, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Queen")]);
        newUnit.Initialise(white.GetOriginAtType("Queen"), "Queen", "White");
        newUnits.Add(new Coordinates(3, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("King")]);
        newUnit.Initialise(white.GetOriginAtType("King"), "King", "White");
        newUnits.Add(new Coordinates(4, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Bishop")]);
        newUnit.Initialise(white.GetOriginAtType("Bishop"), "Bishop", "White");
        newUnits.Add(new Coordinates(5, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Knight")]);
        newUnit.Initialise(white.GetOriginAtType("Knight"), "Knight", "White");
        newUnits.Add(new Coordinates(6, 0), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Rook")]);
        newUnit.Initialise(white.GetOriginAtType("Rook"), "Rook", "White");
        newUnits.Add(new Coordinates(7, 0), newUnit);

        for (int i = 0; i < 8; i++)
        {
            newUnit = (Unit)GameObject.Instantiate(unitPrefabs[white.GetOriginAtType("Pawn")]);
            newUnit.Initialise(white.GetOriginAtType("Pawn"), "Pawn", "White");
            newUnits.Add(new Coordinates(i, 1), newUnit);
        }

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Rook")]);
        newUnit.Initialise(black.GetOriginAtType("Rook"), "Rook", "Black");
        newUnits.Add(new Coordinates(0, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Knight")]);
        newUnit.Initialise(black.GetOriginAtType("Knight"), "Knight", "Black");
        newUnits.Add(new Coordinates(1, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Bishop")]);
        newUnit.Initialise(black.GetOriginAtType("Bishop"), "Bishop", "Black");
        newUnits.Add(new Coordinates(2, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Queen")]);
        newUnit.Initialise(black.GetOriginAtType("Queen"), "Queen", "Black");
        newUnits.Add(new Coordinates(3, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("King")]);
        newUnit.Initialise(black.GetOriginAtType("King"), "King", "Black");
        newUnits.Add(new Coordinates(4, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Bishop")]);
        newUnit.Initialise(black.GetOriginAtType("Bishop"), "Bishop", "Black");
        newUnits.Add(new Coordinates(5, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Knight")]);
        newUnit.Initialise(black.GetOriginAtType("Knight"), "Knight", "Black");
        newUnits.Add(new Coordinates(6, 7), newUnit);

        newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Rook")]);
        newUnit.Initialise(black.GetOriginAtType("Rook"), "Rook", "Black");
        newUnits.Add(new Coordinates(7, 7), newUnit);

        for (int i = 0; i < 8; i++)
        {
            newUnit = (Unit)GameObject.Instantiate(unitPrefabs[black.GetOriginAtType("Pawn")]);
            newUnit.Initialise(black.GetOriginAtType("Pawn"), "Pawn", "Black");
            newUnits.Add(new Coordinates(i, 6), newUnit);
        }

        return newUnits;
    }
}
