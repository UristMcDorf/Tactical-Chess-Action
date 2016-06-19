using UnityEngine;
using System.Collections;

public class UnitMilitary : Unit {
    public override void TakeDamage(int damage, int pierce = 0)
    {
        base.TakeDamage(pierce);

        damage = Mathf.Max(damage - pierce, 0);

        base.TakeDamage(Mathf.Max(damage - (stats[1] - 1), 0));
    }

    protected override int GetPierceValue()
    {
        return stats[0] - 1;
    }

    protected override Effect GetAppliedEffect()
    {
        switch (stats[2])
        {
        case (2):
            return new Effect(Effect.Types.Blind, 1, GetPierceValue());
        case (3):
            return new Effect(Effect.Types.Blind, 3, GetPierceValue());
        }

        return base.GetAppliedEffect();
    }
}