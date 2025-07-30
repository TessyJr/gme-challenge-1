using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Dash Skill")]
public class DashSkill : Skill
{
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;

    public override void UseSkill(GameObject user)
    {
        // Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
        // if (rb != null)
        // {
        //     Vector2 dashDirection = user.transform.right.normalized;
        //     rb.velocity = dashDirection * (dashDistance / dashDuration);
        // }
    }
}
