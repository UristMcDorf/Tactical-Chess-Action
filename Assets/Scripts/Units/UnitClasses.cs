using UnityEngine;
using System.Collections.Generic;

public static class UnitClasses {
    static Dictionary<string, List<int>> unitTypes = new Dictionary<string, List<int>>();

    //TODO unhardcode
    static UnitClasses()
    {
        unitTypes.Add("Pawn", new List<int>(new int[] {1, 1, 1}));
        unitTypes.Add("Rook", new List<int>(new int[] {1, 3, 2}));
        unitTypes.Add("Knight", new List<int>(new int[] {2, 1, 3}));
        unitTypes.Add("Bishop", new List<int>(new int[] {3, 2, 1}));
        unitTypes.Add("Queen", new List<int>(new int[] {3, 3, 3}));
        unitTypes.Add("King", new List<int>(new int[] {2, 2, 2}));
    }

    public static List<int> GetStats(string type)
    {
        List<int> stats = null;

        if (!unitTypes.TryGetValue(type, out stats))
            Debug.LogError("Wrong type " + type);

        return stats;
    }
}
