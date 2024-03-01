using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class StatsHolder : MonoBehaviour
{
    [FormerlySerializedAs("_baseStats")] [SerializeField]
    private List<Stat> baseStats = new();

    [SerializeField] private List<Stat> _actualStats = new();
    [SerializeField] private List<ActiveEffect> _activeEffects = new();
    
    public void ApplyEffect(Effect effect, float duration)
    {
        var activeEffectOfType = _activeEffects.FirstOrDefault(activeEffect => activeEffect.effect == effect);

        if (activeEffectOfType == null)
        {
            var newActiveEffect = new ActiveEffect { effect = effect, timer = duration };
            _activeEffects.Add(newActiveEffect);
        }
        else
        {
            activeEffectOfType.timer = Mathf.Max(activeEffectOfType.timer, duration);
        }
        
        RecalculateStats();
    }
    
    private void Awake()
    {
        CreateActualStats();
        
        foreach (var statsNeeder in GetComponentsInChildren<INeedStats>())
        {
            statsNeeder.InjectStatsHolder(this);
        }
    }

    private void CreateActualStats()
    {
        foreach (var baseStat in baseStats)     
        {
            _actualStats.Add(baseStat.Clone());
        }
    }

    public Stat GetActualStat(StatType statType) => _actualStats.Find(stat => stat.statType == statType);

    private void RecalculateStats()
    {
        for (var i = 0; i < baseStats.Count; i++)
        {
            var baseStat = baseStats[i];
            
            var addedValue = 0f;
            var addedPercent = 0f;
            
            foreach (var activeEffect in _activeEffects)
            {
                if (activeEffect.effect.statType != baseStat.statType) continue;

                switch (activeEffect.effect.applicationType)
                {
                    case EffectApplicationType.AddPercent:
                        addedPercent += activeEffect.effect.value;
                        break;
                    case EffectApplicationType.AddValue:
                        addedValue += activeEffect.effect.value;
                        break;
                }
            }

            _actualStats[i].value = baseStat.value * (1f + addedPercent) + addedValue;
        }
    }
    
    private void Update()
    {
        var deltaTime = Time.deltaTime;

        var effectsCountBefore = _activeEffects.Count;
            
        for (var i = _activeEffects.Count - 1; i >= 0; i--)
        {
            var activeEffect = _activeEffects[i];
            
            if (float.IsNaN(activeEffect.timer))
                continue;

            if (activeEffect.timer > 0f)
                activeEffect.timer -= deltaTime;
            
            if (activeEffect.timer <= 0f)
                _activeEffects.RemoveAt(i);
        }
        
        if (_activeEffects.Count != effectsCountBefore)
            RecalculateStats();
    }
}

public interface INeedStats
{
    void InjectStatsHolder(StatsHolder statsHolder);
}