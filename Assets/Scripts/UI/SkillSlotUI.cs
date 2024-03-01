using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownProgress;

    public Skill Skill
    {
        get => _skill;
        set
        {
            iconImage.enabled = value != null;

            if (value == null) return;

            iconImage.sprite = value.Icon;
            _skill = value;
        }
    }

    private Skill _skill;

    public void SetSelected(bool value)
    {
        transform.localScale = Vector3.one * (value ? 1.2f : 1f);
    }

    public void SetCooldownValue(float value)
    {
        cooldownProgress.fillAmount = value;
    }
}