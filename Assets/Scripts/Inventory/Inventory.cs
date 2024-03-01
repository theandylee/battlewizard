using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public SkillInventory SkillInventory { get; set; }

    private StatsHolder _statsHolder;
    
    private void Awake()
    {
        _statsHolder = GetComponent<StatsHolder>();
        SkillInventory = new(_statsHolder);
    }
}

public class SkillInventory
{
    private readonly List<Skill> _skills = new();
    private readonly Dictionary<int, Skill> _equippedSkills = new();

    public Action<int> onSelectedSlotChanged;
    public Action<int> onSkillEquipped;
    
    private Stat _skillInventorySizeStat;
    
    public SkillInventory(StatsHolder statsHolder)
    {
        _skillInventorySizeStat = statsHolder.GetActualStat(StatType.SkillInventorySize);
    }

    public int Size => Mathf.FloorToInt(_skillInventorySizeStat.value);
    
    public int SelectedSlotIndex
    {
        get;
        set;
    }
    
    public Skill GetEquippedSkill(int slotIndex)
    {
        _equippedSkills.TryGetValue(slotIndex, out var skill);

        return skill;
    }

    public void SelectNextSlot()
    {
        SelectedSlotIndex = (SelectedSlotIndex + 1) % Size;
        
        onSelectedSlotChanged?.Invoke(SelectedSlotIndex);
    }

    public void SelectPreviousSlot()
    {
        SelectedSlotIndex = (SelectedSlotIndex - 1 + Size) % Size;
        
        onSelectedSlotChanged?.Invoke(SelectedSlotIndex);
    }
    
    public Skill SelectedSkill => _skills[SelectedSlotIndex];
    
    public void Add(Skill skill)
    {
        if (_skills.Contains(skill)) return;
        
        _skills.Add(skill);
    }

    public void Equip(Skill skill, int slotIndex)
    {
        if (!_skills.Contains(skill)) return;

        _equippedSkills[slotIndex] = skill;
        
        onSkillEquipped?.Invoke(slotIndex);
    }
}