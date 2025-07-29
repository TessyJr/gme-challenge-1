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

// using UnityEngine;

// public class WeaponController : MonoBehaviour
// {
//     [SerializeField] private float _knockbackForce = 100f;

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         Collider2D hitCollider = collision.collider;

//         // Ignore if we hit a weapon
//         if (hitCollider.CompareTag("Weapon"))
//             return;

//         if (hitCollider.CompareTag("Player"))
//         {
//             // Get the player controller
//             PlayerController target = hitCollider.GetComponent<PlayerController>();
//             if (target == null)
//                 target = hitCollider.transform.root.GetComponent<PlayerController>();

//             // Apply damage and knockback
//             if (target != null)
//             {
//                 target.DecreaseHealth(1);

//                 Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
//                 if (targetRb != null)
//                 {
//                     // Direction from weapon to target
//                     Vector2 knockbackDir = (targetRb.position - (Vector2)transform.position).normalized;
//                     targetRb.AddForce(knockbackDir * _knockbackForce, ForceMode2D.Impulse);
//                 }
//             }
//         }
//     }
// }
