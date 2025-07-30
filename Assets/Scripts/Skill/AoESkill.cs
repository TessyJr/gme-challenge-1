using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shield Skill")]
public class AoESkill : Skill
{
    public GameObject _prefab;

    public override void UseSkill(GameObject user)
    {
        Instantiate(_prefab, user.transform.position, Quaternion.identity, user.transform);
    }
}
