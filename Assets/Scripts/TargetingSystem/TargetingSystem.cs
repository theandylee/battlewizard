using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public Target NearestTarget { get; private set; }

    private readonly List<Target> _targets = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out var target)) return;

        if (_targets.Contains(target)) return;

        target.TargetingSystem = this;
        _targets.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out var target)) return;

        target.TargetingSystem = null;
        _targets.Remove(target);
    }

    public void RemoveTarget(Target target)
    {
        _targets.Remove(target);
    }

    private void Update()
    {
        UpdateNearestTarget();
    }

    private void UpdateNearestTarget()
    {
        var selfPosition = transform.position;

        var minDistance = float.MaxValue;
        Target target = null;

        foreach (var targetRigidBody in _targets)
        {
            var distance = Vector3.Distance(selfPosition, targetRigidBody.TargetPosition);
            if (!(distance < minDistance)) continue;

            minDistance = distance;
            target = targetRigidBody;
        }

        NearestTarget = target;
    }
}