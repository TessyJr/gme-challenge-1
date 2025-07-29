using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float _knockbackForce = 10f;
    [SerializeField] private PlayerController _owner; // Set this from PlayerController

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D hitCollider = collision.collider;

        // === Hit PLAYER ===
        if (hitCollider.CompareTag("Player"))
        {
            PlayerController target = hitCollider.GetComponent<PlayerController>();
            if (target == null)
                target = hitCollider.transform.root.GetComponent<PlayerController>();

            if (target != null && target != _owner)
            {
                target.DecreaseHealth(1);

                Vector2 knockbackDir = (target.transform.position - transform.position).normalized;
                target.ApplyKnockback(knockbackDir * _knockbackForce);

                Debug.Log($"[Player Knockback] {target.name} ← {knockbackDir}");
            }
        }

        // === Weapon clashes with another weapon ===
        if (hitCollider.CompareTag("Weapon"))
        {
            PlayerController otherPlayer = hitCollider.GetComponentInParent<PlayerController>();

            if (otherPlayer != null && otherPlayer != _owner)
            {
                Vector2 dirToOther = (otherPlayer.transform.position - _owner.transform.position).normalized;
                Vector2 dirToSelf = -dirToOther;

                otherPlayer.ApplyKnockback(dirToOther * _knockbackForce);
                _owner.ApplyKnockback(dirToSelf * _knockbackForce);

                Debug.Log($"[Weapon Clash] {_owner.name} ⇄ {otherPlayer.name}");
            }
        }
    }

    public void SetOwner(PlayerController owner)
    {
        _owner = owner;
    }
}
