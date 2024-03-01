using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pool
{
    public static Pool Instance
    {
        get { return _instance ??= new Pool(); }
        private set => _instance = value;
    }

    private static Pool _instance;

    private readonly Dictionary<int, Stack<GameObject>> _stacks = new();
    private readonly Dictionary<int, int> _instanceToOriginal = new();


    public GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
    {
        if (prefab == null) return null;
        
        var originalId = prefab.GetInstanceID();

        if (!_stacks.TryGetValue(originalId, out var stack))
        {
            stack = new Stack<GameObject>();
            _stacks.Add(originalId, stack);
        }

        if (!stack.TryPop(out var instance))
        {
            instance = Object.Instantiate(prefab, position, rotation);
            _instanceToOriginal.Add(instance.GetInstanceID(), originalId);
        }

        instance.transform.position = position;
        instance.transform.rotation = rotation;
        
        instance.SetActive(true);
        
        return instance;
    }

    public void Despawn(GameObject instance)
    {
        var instanceId = instance.GetInstanceID();

        if (!_instanceToOriginal.TryGetValue(instanceId, out var originalId))
        {
            throw new ArgumentException("Cannot despawn non-pooled gameobject");
        }

        if (!_stacks.TryGetValue(originalId, out var stack))
        {
            throw new Exception("No stack for this object");
        }

        instance.SetActive(false);
        stack.Push(instance);
    }

    public void Reset()
    {
        _stacks.Clear();
        _instanceToOriginal.Clear();
    }
}