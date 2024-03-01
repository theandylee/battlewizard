using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public Rigidbody NearestTarget { get; private set; }

    private readonly List<Rigidbody> _targets = new();

    private void OnTriggerEnter(Collider other)
    {
        if (_targets.Contains(other.attachedRigidbody)) return;

        _targets.Add(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        _targets.Remove(other.attachedRigidbody);
    }

    private void Update()
    {
        UpdateNearestTarget();
    }

    private void UpdateNearestTarget()
    {
        var selfPosition = transform.position;

        var minDistance = float.MaxValue;
        Rigidbody target = null;

        foreach (var targetRigidBody in _targets)
        {
            var distance = Vector3.Distance(selfPosition, targetRigidBody.worldCenterOfMass);
            if (!(distance < minDistance)) continue;

            minDistance = distance;
            target = targetRigidBody;
        }

        NearestTarget = target;
    }
}