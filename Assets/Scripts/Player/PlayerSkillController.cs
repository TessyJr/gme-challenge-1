using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        if (_skill)
        {
            _skill.UseSkill(gameObject);
            _skill = null;
        }
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
