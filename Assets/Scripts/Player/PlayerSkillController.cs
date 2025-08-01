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

            // Get the Skill first, before destroying
            Skill pickedSkill = skillBuff.GetSkill();
            skillBuff.SpawnParticle(_skillButtonIcon.gameObject);
            bool taken = skillBuff.TakeBuff(); // May destroy the object

            if (taken && pickedSkill != null)
            {
                _skill = pickedSkill;
                _skillButtonIcon.enabled = true;
                _skillButtonIcon.sprite = pickedSkill.GetIcon();
            }
        }
    }
}
