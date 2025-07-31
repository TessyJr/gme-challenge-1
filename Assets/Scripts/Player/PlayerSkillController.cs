using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    public Image _skillButtonIcon;

    public void UseSkill()
    {
        if (_skill)
        {
            _skill.UseSkill(gameObject);
            _skill = null;

            _skillButtonIcon.sprite = null;
            _skillButtonIcon.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.CompareTag("SkillBuff"))
        {
            SkillBuff skillBuff = collidedObject.GetComponent<SkillBuff>();
            _skill = skillBuff.GetSkill();
            skillBuff.TakeBuff();

            _skillButtonIcon.enabled = true;
            _skillButtonIcon.sprite = _skill.GetIcon();
        }
    }
}
