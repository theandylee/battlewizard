using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    public Vector3 TargetPosition => _selfTransform.position + offset;
    
    public TargetingSystem TargetingSystem { get; set; }

    private Transform _selfTransform;
    
    private void Awake()
    {
        _selfTransform = GetComponent<Transform>();
    }

    private void OnDisable()
    {
        if (TargetingSystem == null) return;
        
        TargetingSystem.RemoveTarget(this);
        TargetingSystem = null;
    }
}
