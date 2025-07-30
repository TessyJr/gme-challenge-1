using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    public void UseSkill()
    {
        _skill.UseSkill(gameObject);
        _skill = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.CompareTag("SkillBuff"))
        {
            _skill = collidedObject.GetComponent<SkillBuff>().GetSkill();
            Destroy(collidedObject);
        }
    }
}
