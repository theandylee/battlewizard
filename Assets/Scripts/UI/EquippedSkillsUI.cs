using System.Collections.Generic;
using UnityEngine;

public class EquippedSkillsUI : MonoBehaviour, IHUDElement
{
    [SerializeField] private GameObject skillSlotPrefab;

    private SkillInventory _skillInventory;

    private List<SkillSlotUI> _skillSlots = new();
    private UseSkillAction _useSkillAction;

    public void Init(GameObject character)
    {
        _skillInventory = character.GetComponent<Inventory>().SkillInventory;
        _useSkillAction = character.GetComponent<UseSkillAction>();
        
        _skillInventory.onSelectedSlotChanged += OnSelectedSlotChanged;
        _skillInventory.onSkillEquipped += OnSkillEquipped;

        _useSkillAction.onSkillCooldownStateUpdate += OnCooldownStateUpdate;
        
        FullUpdate();
    }

    private void OnCooldownStateUpdate(Skill skill, float value)
    {
        var slot = _skillSlots.Find(slot => slot.Skill == skill);

        if (slot == null) return;
        
        slot.SetCooldownValue(value);
    }

    private void OnSkillEquipped(int slotIndex)
    {
        UpdateSlotSkill(slotIndex);
    }

    private void OnSelectedSlotChanged(int selectedSlotIndex)
    {
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        for (var i = 0; i < _skillSlots.Count; i++)
        {
            _skillSlots[i].SetSelected(i == _skillInventory.SelectedSlotIndex);
        }
    }

    private void UpdateSlotSkill(int slotIndex)
    {
        _skillSlots[slotIndex].Skill = _skillInventory.GetEquippedSkill(slotIndex);
    }

    private void FullUpdate()
    {
        for (var slotIndex = 0; slotIndex < _skillInventory.Size; slotIndex++)
        {
            var skill = _skillInventory.GetEquippedSkill(slotIndex);

            if (_skillSlots.Count - 1 < slotIndex)
            {
                AddSlot();
            }
            
            _skillSlots[slotIndex].Skill = skill;
        }
        
        UpdateSelection();
    }

    private void AddSlot()
    {
        var slotInstance = Pool.Instance.Spawn(skillSlotPrefab);
        var skillSlot = slotInstance.GetComponent<SkillSlotUI>();
        _skillSlots.Add(skillSlot);
        
        slotInstance.transform.SetParent(transform);
        slotInstance.transform.localScale = Vector3.one;
    }

    private void RemoveSlot()
    {
        if (_skillSlots.Count == 0) return;

        var skillSlot = _skillSlots[^1];
        _skillSlots.RemoveAt(_skillSlots.Count - 1);
        Pool.Instance.Despawn(skillSlot.gameObject);
    }
}
