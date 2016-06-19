using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UnitPrimal : Unit {
    public override void Initialise(string origin, string type, string side)
    {
        base.Initialise(origin, type, side);

        damage += stats[0] - 1;
        maxHealth += (stats[1] - 1)*3;
    }

    protected override Effect GetAppliedEffect()
    {
        switch (stats[2])
        {
        case (2):
            return new Effect(Effect.Types.Breach, 1);
        case (3):
            return new Effect(Effect.Types.Breach, 3);
        }

        return base.GetAppliedEffect();
    }
}
