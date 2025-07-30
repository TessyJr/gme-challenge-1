using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shield Skill")]
public class ShieldSkill : Skill
{
    public float shieldDuration = 3f;
    public GameObject shieldPrefab;

    public override void UseSkill(GameObject user)
    {
        // GameObject shield = Object.Instantiate(shieldPrefab, user.transform.position, Quaternion.identity, user.transform);
        // Object.Destroy(shield, shieldDuration);
    }
}
