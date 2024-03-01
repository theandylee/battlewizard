using UnityEngine;

[CreateAssetMenu(menuName = StringConsts.SkillMenuName, fileName = StringConsts.SkillFileName)]
public class Skill : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public float CooldownDuration { get; private set; }
}