using UnityEngine.InputSystem;

public class SelectSkillSlotAction : PlayableCharacterAction
{
    private SkillInventory _skillInventory;

    private InputAction _nextSkillInputAction;
    private InputAction _previousSkillInputAction;
    
    protected void Start()
    {
        _skillInventory = GetComponent<Inventory>().SkillInventory;

        _nextSkillInputAction = inputActions.Skills.NextSkill;
        _previousSkillInputAction = inputActions.Skills.PreviousSkill;

        _nextSkillInputAction.performed += OnNextSlotPressed;
        _previousSkillInputAction.performed += OnPreviousSlotPressed;
    }

    private void OnNextSlotPressed(InputAction.CallbackContext obj)
    {
        _skillInventory.SelectNextSlot();
    }

    private void OnPreviousSlotPressed(InputAction.CallbackContext obj)
    {
        _skillInventory.SelectPreviousSlot();
    }
}
