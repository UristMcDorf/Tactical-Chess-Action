using UnityEngine;
using System.Collections;

public class UnitBeasts : Unit {
    protected int delayedDamageNextTurn = 0;
    protected int delayedDamageThisTurn = 0;

    protected override int GetDamageValue(Cell cell)
    {
        if (cell.unit) //if the target has this guy's ally adjacent
        {
            foreach (Cell adjacentCell in cell.GetAdjacentCells())
            {
                if (adjacentCell.HasAllyOf(this))
                {
                    return base.GetDamageValue(cell) + (stats[0] - 1)*2;
                }
            }
        }
        return base.GetDamageValue(cell);
    }

    protected override Effect GetAppliedEffect()
    {
        switch (stats[2])
        {
        case (2):
            return new Effect(Effect.Types.Slow, 1);
        case (3):
            return new Effect(Effect.Types.Slow, 3);
        }

        return base.GetAppliedEffect();
    }

    public override void TakeDamage(int damage, int pierce = 0)
    {
        base.TakeDamage(pierce);

        damage = Mathf.Max(damage - pierce, 0);

        switch (stats[1])
        {
        case(1):
            base.TakeDamage(damage);
            break;
        case(2):
            delayedDamageThisTurn += damage;
            break;
        case(3):
            delayedDamageNextTurn += damage;
            break;
        }
    }

    public override void ProcessTurnEnd()
    {
        base.ProcessTurnEnd();

        base.TakeDamage(delayedDamageThisTurn);
        delayedDamageThisTurn = delayedDamageNextTurn;
        delayedDamageNextTurn = 0;
    }
}