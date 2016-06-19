using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSetupSaver : MonoBehaviour {
    //TODO unhardcode
    public List<UnitSelectorType> types;
    public string side;

    public void SendUnitSetupToGameStateManager()
    {
        //TODO reduce extra checks
        foreach (UnitSelectorType type in types)
        {
            if (!type.connectedOrigin)
                return;
        }

        GameStateManager.Instance.AssignUnitSetups(side, GetUnitSetupFromTypes());
    }

    UnitSetup GetUnitSetupFromTypes()
    {
        List<string> origins = new List<string>();

        foreach (UnitSelectorType type in types)
        {
            origins.Add(type.connectedOrigin.origin);
        }

        return new UnitSetup(origins);
    }
}
