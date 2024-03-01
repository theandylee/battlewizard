using System.Collections.Generic;
using UnityEngine;

public class InventoryInitializer : MonoBehaviour
{
    [SerializeField] private List<Skill> skills;
    
    private void OnEnable()
    {
        var inventory = GetComponent<Inventory>();

        for (var i = 0; i < skills.Count; i++)
        {
            inventory.SkillInventory.Add(skills[i]);
            inventory.SkillInventory.Equip(skills[i], i);
        }
    }
}
