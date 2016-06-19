using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour, IUnitActionHandler {
    public enum GetCellsOptions {None, UnitsBlock, UnitsBlockAfterFirst, UnitsSkip};

    const string blackCellSpritePath = "Sprites/BlackCell";
    static Sprite _blackCellSprite = null;
    static Sprite blackCellSprite
    {
        get
        {
            if (!_blackCellSprite)
            {
                _blackCellSprite = Resources.Load<Sprite>(blackCellSpritePath);
                {
                    if (!_blackCellSprite)
                        Debug.LogError("Couldn't load black cell sprite!");
                }
            }
            return _blackCellSprite;
        }
    }

    const string whiteCellSpritePath = "Sprites/WhiteCell";
    static Sprite _whiteCellSprite = null;
    static Sprite whiteCellSprite
    {
        get
        {
            if (!_whiteCellSprite)
            {
                _whiteCellSprite = Resources.Load<Sprite>(whiteCellSpritePath);
                {
                    if (!_whiteCellSprite)
                        Debug.LogError("Couldn't load white cell sprite!");
                }
            }
            return _whiteCellSprite;
        }
    }

    Coordinates _coordinates = new Coordinates(0, 0);
    public Coordinates coordinates
    {
        get
        {
            return _coordinates;
        }
        set
        {
            _coordinates = value;

            transform.localPosition = new Vector2(_coordinates.x*Constants.k_xCellSize, _coordinates.y*Constants.k_yCellSize);

            GetComponent<SpriteRenderer>().sprite = ((_coordinates.x + _coordinates.y) % 2 == 0) ? blackCellSprite : whiteCellSprite;
        }
    }

    Unit _unit = null;
    public Unit unit
    {
        get
        {
            return _unit;
        }
        set
        {
            _unit = value;

            if (_unit)
            {
                if (_unit.cell)
                    _unit.cell.unit = null;

                _unit.transform.SetParent(this.transform, false);

                _unit.cell = this;
            }
        }
    }

    public void OnMouseEnter()
    {
        if (!CanReactToMouse())
            return;
        ExecuteEvents.Execute<ICellMouseEnterExitHandler>(UIManager.Instance.gameObject, null, (x, y) => x.OnCellMouseEnter(this));
    }

    public void OnMouseExit()
    {
        if (!CanReactToMouse())
            return;
        ExecuteEvents.Execute<ICellMouseEnterExitHandler>(UIManager.Instance.gameObject, null, (x, y) => x.OnCellMouseExit(this));
    }

    public void OnMouseDown()
    {
        if (!CanReactToMouse())
            return;
        ExecuteEvents.Execute<ICellMouseDownHandler>(UIManager.Instance.gameObject, null, (x, y) => x.OnCellMouseDown(this));
    }

    public void OnUnitAction(Unit unit, int damage, int pierce, Effect effect)
    {
        if (this.unit)
            ExecuteEvents.Execute<IUnitActionHandler>(this.unit.gameObject, null, (x, y) => x.OnUnitAction(unit, damage, pierce, effect));
        else
            this.unit = unit;
    }

    public List<Cell> GetCells(int range, GetCellsOptions extraOptions, string side = "none")
    {
        List<Cell> cells = new List<Cell>();

        Coordinates coordinates = null;
        Cell cell = null;

        bool skipDiagonals = false; //minimum is orthogonal adjacent
        if (range <= 0)
        {
            range = 1;
            skipDiagonals = true;
        }

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (skipDiagonals && i != 0 && j != 0)
                    continue;
                
                for (int k = 1; k <= range; k++)
                {
                    coordinates = new Coordinates(this.coordinates.x + k*i, this.coordinates.y + k*j);

                    if (coordinates.AreCorrect())
                    {
                        cell = Grid.Instance.GetCell(coordinates);

                        if (cell.unit && extraOptions == GetCellsOptions.UnitsBlock)
                            break;

                        if (cell.unit && extraOptions == GetCellsOptions.UnitsSkip)
                            continue;

                        cells.Add(cell);

                        if (cell.unit && extraOptions == GetCellsOptions.UnitsBlockAfterFirst)
                            break;

                        if (extraOptions == GetCellsOptions.UnitsBlock || extraOptions == GetCellsOptions.UnitsSkip) //TODO less hacky
                        {
                            bool hasAdjacentRooks = false;
                            foreach (Cell adjacent in cell.GetAdjacentCells())
                            {
                                if (adjacent.unit && adjacent.unit.side != side && adjacent.unit.type == "Rook")
                                {
                                    hasAdjacentRooks = true;
                                    break;
                                }
                            }

                            if (hasAdjacentRooks)
                                break;
                        }
                    }
                }
            }
        }

        return cells;
    }

    public List<Cell> GetAdjacentCells()
    {
        List<Cell> cells = GetCells(1, Cell.GetCellsOptions.None);

        return cells;
    }

    public bool HasAllyOf(Unit unit)
    {
        return this.unit != null && this.unit != unit && this.unit.side == unit.side;
    }

    bool CanReactToMouse()
    {
        return MenuStateManager.Instance.AllowsMouseOnGrid();
    }
}
