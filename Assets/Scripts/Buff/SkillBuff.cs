using UnityEngine;

public class SkillBuff : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    public Skill GetSkill() => _skill;
}
