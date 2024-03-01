using System;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour, IDamageReceiver, INeedStats
{
    [SerializeField] private float value;

    private Stat _maxHpStat;
    private Stat _defenseStat;
    
    private readonly List<IDamageReceivedListener> _damageReceivedListeners = new();
    private readonly List<IHpSetListener> _hpSetListeners = new();
    private readonly List<ILethalDamageReceivedListener> _lethalDamageReceivedListeners = new();

    public bool IsDead { get; private set; }
    public float Value => value;
    public float MaxValue => _maxHpStat.value;
    
    public Action<GameObject> onDeath;
    public Action<HpChangeInfo> onHpSet;
    public Action<HpChangeInfo> onDamageReceived;
    
    public void InjectStatsHolder(StatsHolder statsHolder)
    {
        _maxHpStat = statsHolder.GetActualStat(StatType.MaxHP);
        _defenseStat = statsHolder.GetActualStat(StatType.Defense);
        
        SetHpToMax();
    }
    
    private void Awake()
    {
        GetComponentsInChildren(_damageReceivedListeners);
        GetComponentsInChildren(_hpSetListeners);
        GetComponentsInChildren(_lethalDamageReceivedListeners);
    }
    
    private void OnEnable()
    {
        SetHpToMax();
    }

    private void SetHpToMax()
    {
        if (_maxHpStat == null) return;

        SetHp(_maxHpStat.value);
    }

    private void SetHp(float newValue)
    {
        var hpChangeInfo = new HpChangeInfo { oldHP = value, newHP = newValue, maxHP = _maxHpStat.value};
        value = newValue;
        onHpSet?.Invoke(hpChangeInfo);
        foreach (var hpSetListener in _hpSetListeners)
        {
            hpSetListener.OnHpSet(hpChangeInfo);
        }
    }

    public void DealDamage(DamageData damageData)
    {
        if (IsDead) return;
        
        var receivedDamageInfo = new HpChangeInfo { oldHP = value, maxHP = _maxHpStat.value };

        var defenseMultiplier = 1f - (_defenseStat?.value ?? 0f);
        
        value = Mathf.Clamp(value - damageData.value * defenseMultiplier, 0f, float.MaxValue);

        receivedDamageInfo.newHP = value;

        onDamageReceived?.Invoke(receivedDamageInfo);
        foreach (var damageReceivedListener in _damageReceivedListeners)
        {
            damageReceivedListener.OnDamageReceived(receivedDamageInfo);
        }

        if (value <= 0f)
        {
            Die(receivedDamageInfo);
        }
    }

    private void Die(HpChangeInfo receivedDamageInfo)
    {
        onDeath?.Invoke(gameObject);
            
        foreach (var lethalDamageReceivedListener in _lethalDamageReceivedListeners)
        {
            lethalDamageReceivedListener.OnLethalDamageReceived(receivedDamageInfo);
        }

        IsDead = true;
    }
}