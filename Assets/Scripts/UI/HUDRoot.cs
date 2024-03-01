using System.Collections.Generic;
using UnityEngine;

public class HUDRoot : MonoBehaviour
{
    private readonly List<IHUDElement> _elements = new();
    
    public void Init(GameObject character)
    {
        GetComponentsInChildren(_elements);

        foreach (var hudElement in _elements)
        {
            hudElement.Init(character);
        }
    }
}

public interface IHUDElement
{
    void Init(GameObject character);
}