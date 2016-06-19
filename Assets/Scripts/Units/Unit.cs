using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Unit : MonoBehaviour, IUnitActionHandler {
    protected const string baseUnitSpritePath = "Sprites/Units";
    protected static Dictionary<string, Sprite> _unitSprites = null;
    protected static Dictionary<string, Sprite> unitSprites
    {
        get
        {
            if(_unitSprites == null)
            {
                _unitSprites = new Dictionary<string, Sprite>();

                foreach (string side in Constants.k_unitSides)
                {
                    foreach (string origin in Constants.k_unitOrigins)
                    {
                        foreach (string type in Constants.k_unitTypes)
                        {
                            _unitSprites.Add(origin + type + side, Resources.Load<Sprite>(baseUnitSpritePath + '/' + origin + '/' + type + side));
                        }
                    }
                }
            }

            return _unitSprites;
        }
    }

    protected UnitText _healthText = null;
    protected UnitText healthText
    {
        get
        {
            if (!_healthText)
            {
                List<UnitText> texts = new List<UnitText>(GetComponentsInChildren<UnitText>());

                foreach (UnitText text in texts)
                    if (text.name == "HealthText")
                    {
                        _healthText = text;
                        break;
                    }
            }

            return _healthText;
        }
    }

    protected UnitText _conditionText = null;
    protected UnitText conditionText
    {
        get
        {
            if (!_conditionText)
            {
                List<UnitText> texts = new List<UnitText>(GetComponentsInChildren<UnitText>());

                foreach (UnitText text in texts)
                    if (text.name == "ConditionText")
                    {
                        _conditionText = text;
                        break;
                    }
            }

            return _conditionText;
        }
    }

    protected string _origin = "Default";
    public string origin
    {
        get
        {
            return _origin;
        }
        set
        {
            if (Constants.k_unitOrigins.Contains(value))
            {
                _origin = value;
            }
            else
                Debug.LogError("Wrong origin " + value);
        }
    }

    protected string _type = "Pawn";
    public string type
    {
        get
        {
            return _type;
        }
        set
        {
            if (Constants.k_unitTypes.Contains(value))
            {
                _type = value;
            }
            else
                Debug.LogError("Wrong type " + value);
        }
    }
        
    protected string _side = "Black";
    public string side
    {
        get
        {
            return _side;
        }
        set
        {
            if (Constants.k_unitSides.Contains(value))
            {
                _side = value;
            }
            else
                Debug.LogError("Wrong side " + value);
        }
    }

    protected Cell _cell = null;
    public Cell cell
    {
        get
        {
            return _cell;
        }
        set
        {
            _cell = value;
        }
    }

    protected bool _active = true;
    public bool active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;

            UpdateActiveState();
        }
    }

    protected List<int> _stats = new List<int>();
    public List<int> stats
    {
        get
        {
            return _stats;
        }
        set
        {
            _stats = value;

            UpdateStats();
        }
    }

    protected int move = 3;
    protected int attack = 1;

    protected int _maxHealth = 4;
    protected int maxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
            health = _maxHealth;
        }
    }

    protected int _health = 4;
    protected int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            if (_health > 0)
                UpdateHealthText();
            else
                Die();
        }
    }

    protected int damage = 2;

    protected Effect _effect = new Effect(Effect.Types.None, 0);
    public virtual Effect effect
    {
        get
        {
            if (_effect == null)
                _effect = new Effect(Effect.Types.None, 0);
            
            return _effect;
        }
        set
        {
            _effect = value;
        }
    }

    protected void Start()
    {
        this.transform.localPosition = Vector2.zero;
    }

    protected void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = unitSprites[origin + type + side];
    }

    public virtual void Initialise(string origin, string type, string side)
    {
        this.origin = origin;
        this.type = type;
        this.side = side;

        this.name = origin + type + side;
        this.stats = UnitClasses.GetStats(type);

        UpdateSprite();
        GameStateManager.Instance.AddUnitToList(this);
    }

    public virtual List<Cell> GetMoveCells()
    {
        int actualMove = ((effect.type == Effect.Types.Slow) ? (move - 1) : move);
        List<Cell> cells = null;

        if (type == "Knight")
            cells = this.cell.GetCells(actualMove, Cell.GetCellsOptions.UnitsSkip, side);
        else
            cells = this.cell.GetCells(actualMove, Cell.GetCellsOptions.UnitsBlock, side);

        if (effect.type == Effect.Types.Paranoia)
        {
            List<Cell> cellsToRemove = new List<Cell>();

            foreach (Cell cell in cells)
                foreach (Cell adjacent in cell.GetAdjacentCells())
                    if (!cellsToRemove.Contains(cell) && adjacent.HasAllyOf(this))
                        cellsToRemove.Add(cell);

            foreach (Cell cell in cellsToRemove)
                cells.Remove(cell);
        }

        return cells;
    }

    public virtual List<Cell> GetAttackCells()
    {
        List<Cell> cells = this.cell.GetCells(attack, Cell.GetCellsOptions.UnitsBlockAfterFirst);

        foreach (Cell cell in this.cell.GetCells(attack, Cell.GetCellsOptions.UnitsBlockAfterFirst))
        {
            if (!cell.unit || cell.unit.side == this.side)
                cells.Remove(cell);
            if (effect.type == Effect.Types.Blind)
            {
                bool keepCell = false;

                foreach (Cell adjacent in cell.GetAdjacentCells())
                    if (adjacent.HasAllyOf(this))
                        keepCell = true;

                if (!keepCell)
                    cells.Remove(cell);
            }
        }

        return cells;
    }

    public virtual void PerformAction(Cell cell)
    {
        if (active)
        {
            if (type == "Bishop" && cell.unit)
            {
                foreach (Cell adjacent in this.cell.GetAdjacentCells())
                    if (adjacent.unit && adjacent.unit.side != this.side)
                        ExecuteEvents.Execute<IUnitActionHandler>(adjacent.gameObject, null, (x, y) => x.OnUnitAction(this, GetDamageValue(adjacent), GetPierceValue(), GetAppliedEffect()));
            }
            else
            {
                ExecuteEvents.Execute<IUnitActionHandler>(cell.gameObject, null, (x, y) => x.OnUnitAction(this, GetDamageValue(cell), GetPierceValue(), GetAppliedEffect()));
            }
            active = false;
            ExecuteEvents.Execute<IGameStateManagerHandler>(GameStateManager.Instance.gameObject, null, (x, y) => x.OnUnitAction(this));
        }
    }

    public virtual void OnUnitAction(Unit unit, int damage, int pierce, Effect effect)
    {
        if (unit.side != side)
        {
            TakeDamage(damage);
            this.effect = effect;
            UpdateEffectText();
        }
    }

    public virtual void TakeDamage(int damage, int pierce = 0)
    {
        health -= damage + ((effect.type == Effect.Types.Breach) ? 1 : 0);
    }

    protected void Die()
    {
        Destroy(gameObject);

        GameStateManager.Instance.RemoveUnitFromList(this);
        UIManager.Instance.Invoke("UpdateMoveAttackHighlighters", 0.01f); //TODO this is hacky. fix
    }

    protected virtual void UpdateHealthText()
    {
        healthText.textMesh.text = health + "/" + maxHealth;
    }

    protected void UpdateEffectText() //TODO less hacky
    {
        switch (effect.type)
        {
        case (Effect.Types.None):
            conditionText.textMesh.text = string.Empty;
            break;
        case (Effect.Types.Blind):
            conditionText.textMesh.text = "BLN";
            break;
        case (Effect.Types.Breach):
            conditionText.textMesh.text = "BRC";
            break;
        case (Effect.Types.Weaken):
            conditionText.textMesh.text = "WKN";
            break;
        case (Effect.Types.Poison):
            conditionText.textMesh.text = "PSN";
            break;
        case (Effect.Types.Slow):
            conditionText.textMesh.text = "SLW";
            break;
        case (Effect.Types.Paranoia):
            conditionText.textMesh.text = "PRN";
            break;
        }

        if (effect.type != Effect.Types.None) //TODO less exceptional
            conditionText.textMesh.text += effect.duration;
    }

    protected void UpdateActiveState()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    protected void UpdateStats()
    {
        switch (stats[0])
        {
        case(1):
            damage = 2;
            break;
        case(2):
            damage = 3;
            break;
        case(3):
            damage = 4;
            break;
        }
        switch (stats[1])
        {
        case(1):
            maxHealth = 6;
            break;
        case(2):
            maxHealth = 9;
            break;
        case(3):
            maxHealth = 12;
            break;
        }
        switch (stats[2])
        {
        case(1):
            move = 1;
            break;
        case(2):
            move = 2;
            break;
        case(3):
            move = 3;
            break;
        }
    }

    public virtual void ProcessTurnEnd()
    {
        if (effect.type == Effect.Types.Poison)
            TakeDamage(1, 2);

        effect.TickDown();

        UpdateEffectText();
    }

    protected virtual int GetDamageValue(Cell cell)
    {
        return damage - ((effect.type == Effect.Types.Weaken) ? 1 : 0);
    }

    protected virtual int GetPierceValue()
    {
        return 0;
    }

    public bool HasEffect()
    {
        return effect != null;
    }

    protected virtual Effect GetAppliedEffect()
    {
        return null;
    }
}
