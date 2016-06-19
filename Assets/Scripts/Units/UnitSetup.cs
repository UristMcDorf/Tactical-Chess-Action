using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSetup {
    Dictionary<string, string> typeToOriginDictionary = null;

    public UnitSetup()
    {
        SetTypeToOriginDictionary(new List<string>(new string[] {"Default", "Default", "Default", "Default", "Default", "Default"}));   
    }

    public UnitSetup(List<string> originList)
    {
        SetTypeToOriginDictionary(originList);
    }

    public UnitSetup(Dictionary<string, string> typeToOriginDictionary)
    {
        SetTypeToOriginDictionary(typeToOriginDictionary);
    }

    //TODO unhardcode
    void SetTypeToOriginDictionary(List<string> originList)
    {
        Dictionary<string, string> typeToOriginDictionary = new Dictionary<string, string>();

        typeToOriginDictionary.Add("Pawn", originList[0]);
        typeToOriginDictionary.Add("Rook", originList[1]);
        typeToOriginDictionary.Add("Knight", originList[2]);
        typeToOriginDictionary.Add("Bishop", originList[3]);
        typeToOriginDictionary.Add("Queen", originList[4]);
        typeToOriginDictionary.Add("King", originList[5]);

        SetTypeToOriginDictionary(typeToOriginDictionary);
    }

    void SetTypeToOriginDictionary(Dictionary<string, string> typeToOriginDictionary)
    {
        this.typeToOriginDictionary = new Dictionary<string, string>(typeToOriginDictionary);
    }

    public string GetOriginAtType(string type)
    {
        string origin;

        if (typeToOriginDictionary.TryGetValue(type, out origin))
            return origin;
        else
        {
            Debug.LogError ("Wrong type " + type);
            return "Default";
        }
    }
}
