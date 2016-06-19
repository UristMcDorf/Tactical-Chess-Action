using UnityEngine;
using System.Collections;

public class Effect {
    public enum Types {None, Blind, Breach, Poison, Weaken, Slow, Paranoia};

    public Types type = Types.None;
    public int pierce = 0;

    protected int _duration = 0;
    public int duration
    {
        get
        {
            return _duration;
        }
        set
        {
            _duration = Mathf.Max(value, 0);

            if (_duration == 0)
                type = Types.None;
        }
    }

    public Unit unit = null;

    public Effect(Types type, int duration)
    {
        this.type = type;
        this.duration = duration;
    }

    public Effect(Types type, int duration, int pierce)
    {
        this.type = type;
        this.duration = duration;
        this.pierce = pierce;
    }

    public void TickDown()
    {
        if (type != Types.None)
        {
            duration--;
        }
    }
}
