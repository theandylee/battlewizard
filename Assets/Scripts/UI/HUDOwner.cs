using UnityEngine;

public class HUDOwner : MonoBehaviour
{
    [SerializeField] private GameObject HUDPrefab;

    private HUDRoot _hudRoot;

    private void OnEnable()
    {
        var hudInstance = Pool.Instance.Spawn(HUDPrefab);
        _hudRoot = hudInstance.GetComponent<HUDRoot>();
        _hudRoot.Init(gameObject);
    }

    private void OnDisable()
    {
        if (_hudRoot == null) return;
        
        Pool.Instance.Despawn(_hudRoot.gameObject);
        _hudRoot = null;
    }
}
