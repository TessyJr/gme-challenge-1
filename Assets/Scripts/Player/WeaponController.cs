using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This is the collider on the object we hit
        Collider2D hitCollider = collision.collider;

        // Ignore if we hit a weapon
        if (hitCollider.CompareTag("Weapon"))
            return;

        if (hitCollider.CompareTag("Player"))
        {
            PlayerController target = hitCollider.GetComponent<PlayerController>();
            if (target == null)
            {
                // If the collider is a child, try the root
                target = hitCollider.transform.root.GetComponent<PlayerController>();
            }

            if (target != null)
            {
                target.DecreaseHealth(1);
            }
        }
    }
}
