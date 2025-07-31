using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Shield Skill")]
public class ShieldSkill : Skill
{
    public float shieldDuration = 3f;
    public GameObject shieldPrefab;

    public override void UseSkill(GameObject user)
    {
        if (!user.TryGetComponent<PlayerController>(out var player)) return;

        player.SetIsShielded(true);

        GameObject shield = Instantiate(shieldPrefab, user.transform.position, Quaternion.identity, user.transform);
        ShieldController controller = shield.GetComponent<ShieldController>();
        controller.SetPlayerController(player);
        controller.StartShieldTimer(shieldDuration);
    }
}
