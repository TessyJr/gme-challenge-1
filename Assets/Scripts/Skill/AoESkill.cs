using UnityEngine;

[CreateAssetMenu(menuName = "Skills/AoE Skill")]
public class AoESkill : Skill
{
    public float radius = 3f;
    public int damage = 10;
    public LayerMask enemyLayer;
    public GameObject effectPrefab;

    public override void UseSkill(GameObject user)
    {
        // Collider2D[] enemies = Physics2D.OverlapCircleAll(user.transform.position, radius, enemyLayer);

        // foreach (Collider2D enemy in enemies)
        // {
        //     var health = enemy.GetComponent<EnemyHealth>();
        //     if (health != null)
        //     {
        //         health.TakeDamage(damage);
        //     }
        // }

        // if (effectPrefab != null)
        // {
        //     Object.Instantiate(effectPrefab, user.transform.position, Quaternion.identity);
        // }
    }
}
