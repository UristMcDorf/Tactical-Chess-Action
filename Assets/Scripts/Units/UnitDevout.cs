using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UnitDevout : Unit {
    public override Effect effect
    {
        get
        {
            return base.effect;
        }
        set
        {
            base.effect = value;

            if (_effect.type != Effect.Types.None)
            {
                switch (stats[1])
                {
                case (2):
                    _effect.duration -= Mathf.Max(1 - _effect.pierce, 0);
                    break;
                case (3):
                    _effect.duration -= Mathf.Max(3 - _effect.pierce, 0);
                    break;
                }
            }
        }
    }

    protected override int GetDamageValue(Cell cell)
    {
        if (cell.unit && !cell.unit.active) //if the target's acted already in this turn
            return base.GetDamageValue(cell) + (stats[0] - 1)*2;
        return base.GetDamageValue(cell);
    }

    protected override Effect GetAppliedEffect()
    {
        switch (stats[2])
        {
        case (2):
            return new Effect(Effect.Types.Paranoia, 1);
        case (3):
            return new Effect(Effect.Types.Paranoia, 3);
        }

        return base.GetAppliedEffect();
    }
}