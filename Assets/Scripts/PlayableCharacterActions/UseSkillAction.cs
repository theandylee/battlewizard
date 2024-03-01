using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseSkillAction : PlayableCharacterAction
{
    [SerializeField] private TargetingSystem targetingSystem;
    [SerializeField] private Transform projectileSpawnPoint;

    public Action<Skill, float> onSkillCooldownStateUpdate;
    
    private InputAction _useSkillInputAction;

    private readonly List<CooldownTimer> _cooldownTimers = new();
    private SkillInventory _skillInventory;

    protected void Start()
    {
        _skillInventory = GetComponent<Inventory>().SkillInventory;
        
        _useSkillInputAction = inputActions.Skills.UseSkill;
        _useSkillInputAction.performed += OnUseSkillPressed;
    }
    
    private void OnUseSkillPressed(InputAction.CallbackContext context)
    {
        var selectedSkill = _skillInventory.SelectedSkill;

        if (IsCoolingDown(selectedSkill)) return;

        var projectileInstance = Pool.Instance.Spawn(selectedSkill.Prefab, projectileSpawnPoint.position);

        if (targetingSystem.NearestTarget != null)
        {
            projectileInstance.transform.LookAt(targetingSystem.NearestTarget.TargetPosition);
        }
        else
        {
            projectileInstance.transform.rotation = transform.rotation;
        }

        StartCooldown(selectedSkill);
    }

    private void StartCooldown(Skill skill)
    {
        var existingTimer = _cooldownTimers.Find(ct => ct.skill == skill);

        if (existingTimer != null) return;

        _cooldownTimers.Add(new CooldownTimer { skill = skill, timer = skill.CooldownDuration });
    }

    private bool IsCoolingDown(Skill skill)
    {
        return _cooldownTimers.Any(ct => ct.skill == skill);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        for (var i = _cooldownTimers.Count - 1; i >= 0; i--)
        {
            var cooldownTimer = _cooldownTimers[i];
            cooldownTimer.timer = Mathf.Clamp(cooldownTimer.timer - deltaTime, 0f, float.MaxValue);
            
            if (cooldownTimer.timer <= 0f)
                _cooldownTimers.RemoveAt(i);
            
            onSkillCooldownStateUpdate?.Invoke(cooldownTimer.skill, cooldownTimer.timer / cooldownTimer.skill.CooldownDuration);
        }
    }

    private class CooldownTimer
    {
        public Skill skill;
        public float timer;
    }
}