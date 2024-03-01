using System;

[Serializable]
public class Stat
{
    public StatType statType;
    public float value;

    public Stat Clone() => new Stat { statType = statType, value = value };
}