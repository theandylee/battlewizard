using UnityEngine;

public static class LayerMaskExtensions
{
    public static bool HasLayer(this LayerMask layerMask, int layer)
    {
        return (layerMask | (1 << layer)) == layerMask;
    }
}
