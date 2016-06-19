using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//SINGLETON
public class GameStateManager : MonoBehaviour, IGameStateManagerHandler {
    static GameStateManager _Instance = null;
    public static GameStateManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType<GameStateManager>();
                {
                    if (!_Instance)
                        Debug.LogError("No game state manager on scene!");
                }
            }
            return _Instance;
        }
    }

    Text _textObject = null;
    Text textObject
    {
        get
        {
            if (!_textObject)
            {
                List<Text> texts = new List<Text>(FindObjectsOfType<Text>());

                foreach (Text text in texts)
                    if (text.name == "CurrentTurnText")
                        _textObject = text;

                if (!_textObject)
                    Debug.LogError("CurrentTurnText not found!");
            }

            return _textObject;
        }
    }

    int _currentTurn = 1;
    int currentTurn
    {
        get
        {
            return _currentTurn;
        }
        set
        {
            _currentTurn = value;

            CheckCurrentPlayerSide();
        }
    }

    List<Unit> units = new List<Unit>();

    Dictionary<string, int> totalUnitCounts = new Dictionary<string, int>();
    Dictionary<string, int> actedUnitCounts = new Dictionary<string, int>();
    string currentPlayerSide = "none";

    //TODO unhardcode sides
    UnitSetup black = new UnitSetup();
    UnitSetup white = new UnitSetup();

    void Start()
    {
        currentTurn = 1;
    }

    public void AddUnitToList(Unit unit)
    {
        units.Add(unit);

        if (totalUnitCounts.ContainsKey(unit.side))
            totalUnitCounts[unit.side]++;
        else
        {
            totalUnitCounts[unit.side] = 1;
            actedUnitCounts[unit.side] = 0;
        }
    }

    public void RemoveUnitFromList(Unit unit)
    {
        units.Remove(unit);

        totalUnitCounts[unit.side]--;
    }

    public void OnUnitAction(Unit unit)
    {
        actedUnitCounts[unit.side]++;

        CheckForTurnEnd();
        CheckCurrentPlayerSide();
    }

    void CheckForTurnEnd()
    {
        foreach (Unit unit in units)
        {
            if (unit.active)
                return;
        }

        NextTurn();
    }

    public void NextTurn()
    {
        foreach (Unit unit in units)
        {
            unit.active = true;

            unit.ProcessTurnEnd(); //TODO error on poisoned unit removal?
        }

        List<string> refreshDictionaryList = new List<string>();

        foreach (string key in actedUnitCounts.Keys)
            refreshDictionaryList.Add(key);

        foreach (string key in refreshDictionaryList)
            actedUnitCounts[key] = 0;

        currentTurn++;
    }

    void UpdateTurnText()
    {
        textObject.text = "Current turn: " + currentTurn + '\n' + "Current player is: " + currentPlayerSide;
    }

    void CheckCurrentPlayerSide()
    {
        int currentAmount = 100;

        if (actedUnitCounts.Count == 0)
        {
            currentPlayerSide = "White";
            UpdateTurnText();
            return;
        }

        foreach (string key in actedUnitCounts.Keys) //TODO SUPERHACKY
        {
            if (actedUnitCounts[key] == currentAmount)
            {
                if (currentTurn % 2 == 0)
                    currentPlayerSide = "Black";
                else
                    currentPlayerSide = "White";
                continue;
            }

            if (actedUnitCounts[key] >= totalUnitCounts[key])
                continue;

            if (actedUnitCounts[key] < currentAmount)
            {
                currentAmount = actedUnitCounts[key];
                currentPlayerSide = key;
            }
        }

        UpdateTurnText();
    }

    public void AssignUnitSetups(string side, UnitSetup setup)
    {
        if (side == "White")
            white = setup;
        else if (side == "Black")
            black = setup;
    }

    public void ReloadGame()
    {
        this.Clear();
        Grid.Instance.Clear();

        totalUnitCounts["Black"] = 0;
        totalUnitCounts["White"] = 0;

        actedUnitCounts["Black"] = 0;
        actedUnitCounts["White"] = 0;

        currentTurn = 1;

        Grid.Instance.GenerateField(white, black);
    }

    void Clear()
    {
        List<Unit> unitsToRemove = new List<Unit>(units);

        foreach (Unit unit in unitsToRemove)
        {
            Destroy(unit.gameObject);
        }

        units.Clear();
    }
}
