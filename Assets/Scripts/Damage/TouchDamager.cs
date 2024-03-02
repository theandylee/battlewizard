using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchDamager : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private DamageData damageData;
    [SerializeField] private float period;

    private readonly List<DamageTimer> _damageTimers = new();
    
    private void OnTriggerEnter(Collider other)
    {
        if (layerMask.HasLayer(other.gameObject.layer) &&
            other.gameObject.TryGetComponent<IDamageReceiver>(out var damageReceiver) &&
            _damageTimers.All(dt => dt.damageReceiver != damageReceiver))
        {
            _damageTimers.Add(new DamageTimer { timer = period, damageReceiver = damageReceiver });
            damageReceiver.DealDamage(damageData);
        }    }

    private void OnTriggerExit(Collider other)
    {
        if (!layerMask.HasLayer(other.gameObject.layer) ||
            !other.gameObject.TryGetComponent<IDamageReceiver>(out var damageReceiver))
            return;
        
        for (var i = 0; i < _damageTimers.Count; i++)
        {
            if (_damageTimers[i].damageReceiver != damageReceiver) continue;
                
            _damageTimers.RemoveAt(i);
            return;
        }
    }

    private void Update()
    {
        if (_damageTimers.Count == 0) return;
        
        var deltaTime = Time.deltaTime;
        
        for (var i = 0; i < _damageTimers.Count; i++)
        {
            var damageTimer = _damageTimers[i];
            
            damageTimer.timer -= deltaTime;

            if (damageTimer.timer <= 0f)
            {
                damageTimer.timer = period;
                damageTimer.damageReceiver.DealDamage(damageData);
            }

            _damageTimers[i] = damageTimer;
        }
    }

    private void OnDisable()
    {
        _damageTimers.Clear();
    }

    private struct DamageTimer
    {
        public float timer;
        public IDamageReceiver damageReceiver;
    }
}