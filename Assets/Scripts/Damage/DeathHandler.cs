using UnityEngine;

public class DeathHandler : MonoBehaviour, ILethalDamageReceivedListener
{
    [SerializeField] private GameObject deathFxPrefab;
    
    public void OnLethalDamageReceived(HpChangeInfo hpChangeInfo)
    {
        Pool.Instance.Spawn(deathFxPrefab, transform.position, transform.rotation);
        Pool.Instance.Despawn(gameObject);
    }
}
