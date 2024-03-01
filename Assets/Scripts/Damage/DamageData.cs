using System;

[Serializable]
public class DamageData
{
    [NonSerialized] public StatsHolder inflictorStatsHolder;
    public float value;
    public DamageType damageType;
}