using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

//SINGLETON
public class UIManager : MonoBehaviour, ICellMouseEnterExitHandler, ICellMouseDownHandler {
    static UIManager _Instance = null;
    public static UIManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType<UIManager>();
                {
                    if (!_Instance)
                        Debug.LogError("No UI manager on scene!");
                }
            }
            return _Instance;
        }
    }

    const string mouseoverHighlighterPrefabPath = "Prefabs/MouseoverHighlighter";
    static Highlighter mouseoverHighlighterPrefab = null;

    const string selectionHighlighterPrefabPath = "Prefabs/SelectionHighlighter";
    static Highlighter selectionHighlighterPrefab = null;

    const string attackHighlighterPrefabPath = "Prefabs/AttackHighlighter";
    static Highlighter attackHighlighterPrefab = null;

    const string moveHighlighterPrefabPath = "Prefabs/MoveHighlighter";
    static Highlighter moveHighlighterPrefab = null;

    Highlighter _mouseoverHighlighter = null;
    public Highlighter mouseoverHighlighter
    {
        get
        {
            if (!_mouseoverHighlighter)
            {
                _mouseoverHighlighter = (Highlighter)Instantiate(mouseoverHighlighterPrefab);

                _mouseoverHighlighter.transform.parent = this.transform;
            }

            return _mouseoverHighlighter;
        }
    }

    Highlighter _selectionHighlighter = null;
    public Highlighter selectionHighlighter
    {
        get
        {
            if (!_selectionHighlighter)
            {
                _selectionHighlighter = (Highlighter)Instantiate(selectionHighlighterPrefab);

                _selectionHighlighter.transform.parent = this.transform;
            }

            return _selectionHighlighter;
        }
    }

    Cell _currentlySelectedCell = null;
    Cell currentlySelectedCell
    {
        get
        {
            return _currentlySelectedCell;
        }
        set
        {
            _currentlySelectedCell = value;

            UpdateMoveAttackHighlighters();
        }
    }

    List<Highlighter> selectedCellHighlighters = new List<Highlighter>();
    List<Cell> selectedCellActionRange = new List<Cell>();

    void Awake()
    {
        LoadResources();
    }

    void LoadResources()
    {
        mouseoverHighlighterPrefab = Resources.Load<Highlighter>(mouseoverHighlighterPrefabPath);
        selectionHighlighterPrefab = Resources.Load<Highlighter>(selectionHighlighterPrefabPath);
        attackHighlighterPrefab = Resources.Load<Highlighter>(attackHighlighterPrefabPath);
        moveHighlighterPrefab = Resources.Load<Highlighter>(moveHighlighterPrefabPath);
    }

    public void OnCellMouseEnter(Cell cell)
    {
        ExecuteEvents.Execute<ICellMouseEnterExitHandler>(mouseoverHighlighter.gameObject, null, (x, y) => x.OnCellMouseEnter(cell));
    }

    public void OnCellMouseExit(Cell cell)
    {
        ExecuteEvents.Execute<ICellMouseEnterExitHandler>(mouseoverHighlighter.gameObject, null, (x, y) => x.OnCellMouseExit(cell));
    }

    public void OnCellMouseDown(Cell cell)
    {
        if (selectedCellActionRange.Contains(cell)) //TODO less hacky
        {
            currentlySelectedCell.unit.PerformAction(cell);

            ExecuteEvents.Execute<ICellMouseDownHandler>(selectionHighlighter.gameObject, null, (x, y) => x.OnCellMouseDown(null));
            currentlySelectedCell = null;
            return;
        }
        
        ExecuteEvents.Execute<ICellMouseDownHandler>(selectionHighlighter.gameObject, null, (x, y) => x.OnCellMouseDown(cell));
            
        currentlySelectedCell = cell;
    }

    public void UpdateMoveAttackHighlighters()
    {
        foreach (Highlighter highlighter in selectedCellHighlighters)
        {
            Destroy(highlighter.gameObject);
        }

        selectedCellHighlighters.Clear();
        selectedCellActionRange.Clear();
        
        if (currentlySelectedCell && currentlySelectedCell.unit && currentlySelectedCell.unit.active)
        {
            List<Cell> moveCells = currentlySelectedCell.unit.GetMoveCells();
            List<Cell> attackCells = currentlySelectedCell.unit.GetAttackCells();

            Highlighter highlighter = null;
            foreach (Cell cell in moveCells)
            {
                highlighter = (Highlighter)Instantiate(moveHighlighterPrefab);

                highlighter.transform.parent = transform;
                highlighter.cell = cell;

                selectedCellHighlighters.Add(highlighter);
                selectedCellActionRange.Add(cell);
            }
            foreach (Cell cell in attackCells)
            {
                highlighter = (Highlighter)Instantiate(attackHighlighterPrefab);

                highlighter.transform.parent = transform;
                highlighter.cell = cell;

                selectedCellHighlighters.Add(highlighter);
                selectedCellActionRange.Add(cell);
            }
        }
    }
}
