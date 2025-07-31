using UnityEngine;

public class SkillBuff : MonoBehaviour
{
    [SerializeField] private Skill _skill;
    private SkillBuffSpawner _spawner;

    public void SetSpawner(SkillBuffSpawner spawner) => _spawner = spawner;
    public Skill GetSkill() => _skill;

    public void TakeBuff()
    {
        if (_spawner != null)
        {
            _spawner.OnBuffTaken();
        }

        Destroy(gameObject);
    }
}
