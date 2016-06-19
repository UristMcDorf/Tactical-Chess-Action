using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMages : Unit {
    protected UnitText _shieldText = null;
    protected UnitText shieldText
    {
        get
        {
            if (!_shieldText)
            {
                List<UnitText> texts = new List<UnitText>(GetComponentsInChildren<UnitText>());

                foreach (UnitText text in texts)
                    if (text.name == "ShieldText")
                    {
                        _shieldText = text;
                        break;
                    }
            }

            return _shieldText;
        }
    }

    protected int _maxShield = 0;
    protected int maxShield
    {
        get
        {
            return _maxShield;
        }
        set
        {
            _maxShield = value;

            shield = _maxShield;

            if (_maxShield <= 0)
                shieldText.GetComponent<Renderer>().enabled = false;

            UpdateHealthText();
        }
    }

    protected int shield = 0;

    public override void Initialise(string origin, string type, string side)
    {
        base.Initialise(origin, type, side);

        maxShield = (stats[1] - 1)*2;
        attack += stats[0] - 1;
    }

    public override void TakeDamage(int damage, int pierce = 0)
    {
        base.TakeDamage(pierce);

        damage = Mathf.Max(damage - pierce, 0);

        if (shield >= damage)
        {
            shield -= damage;
            UpdateHealthText();
        }
        else
        {
            damage -= shield;
            shield = 0;
            base.TakeDamage(damage, pierce);
        }
    }

    protected override void UpdateHealthText()
    {
        base.UpdateHealthText();

        if(maxShield > 0)
            shieldText.textMesh.text = shield + "/" + maxShield;
    }

    public override void ProcessTurnEnd()
    {
        base.ProcessTurnEnd();

        shield = maxShield;
        UpdateHealthText();
    }

    protected override Effect GetAppliedEffect()
    {
        switch (stats[2])
        {
        case (2):
            return new Effect(Effect.Types.Weaken, 1);
        case (3):
            return new Effect(Effect.Types.Weaken, 3);
        }

        return base.GetAppliedEffect();
    }
}