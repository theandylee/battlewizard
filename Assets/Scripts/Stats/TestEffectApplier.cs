using UnityEngine;

public class TestEffectApplier : MonoBehaviour
{
    [SerializeField] private Effect effect;
    [SerializeField] private float duration;

    private void OnEnable()
    {
        ApplyEffect();
    }

    private void ApplyEffect()
    {
        var statsHolder = GetComponent<StatsHolder>();
        
        statsHolder.ApplyEffect(effect, duration);
    }
}
