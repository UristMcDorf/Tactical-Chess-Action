using UnityEngine;
using System.Collections;

public class UnitShadows : Unit {
    protected override int GetDamageValue(Cell cell)
    {
        if (cell.unit && cell.unit.HasEffect()) //if the target has any effect applied
            return base.GetDamageValue(cell) + (stats[0] - 1)*2;
        return base.GetDamageValue(cell);
    }

    protected override Effect GetAppliedEffect()
    {
        switch (stats[2])
        {
        case (2):
            return new Effect(Effect.Types.Poison, 1);
        case (3):
            return new Effect(Effect.Types.Poison, 3);
        }

        return base.GetAppliedEffect();
    }

    public override void OnUnitAction(Unit unit, int damage, int pierce, Effect effect)
    {
        base.OnUnitAction(unit, damage, pierce, effect);

        unit.TakeDamage(Mathf.Max((stats[1] - 1) - pierce, 0), 0);
    }
}