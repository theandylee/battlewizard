using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private DamageData damageData;
    [SerializeField] private List<Effect> effects;
    [SerializeField] private float range;
    
    private Transform _selfTransform;
    private float _distance;
    
    private void Awake()
    {
        _selfTransform = transform;
    }

    private void OnEnable()
    {
        _distance = 0f;
    }

    public StatsHolder InflictorStatsHolder
    {
        get => damageData.inflictorStatsHolder;
        set => damageData.inflictorStatsHolder = value;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageReceiver>(out var damageReceiver))
        {
            damageReceiver.DealDamage(damageData);
        }

        if (other.TryGetComponent<StatsHolder>(out var statsHolder))
        {
            foreach (var effect in effects)
            {
                statsHolder.ApplyEffect(effect, 3f);
            }
        }
        
        Pool.Instance.Despawn(gameObject);
    }

    private void FixedUpdate()
    {
        var scalarDelta = speed * Time.fixedDeltaTime;
        _selfTransform.position += _selfTransform.forward * scalarDelta;

        _distance += scalarDelta;
        if (_distance > range)
            Pool.Instance.Despawn(gameObject);
    }
}
