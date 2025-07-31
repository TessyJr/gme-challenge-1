using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField] private float _knockbackForce = 5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Meteor"))
        {
            if (_playerController != null)
            {
                Vector2 knockbackDir = (_playerController.transform.position - collision.transform.position).normalized;

                _playerController.ApplyKnockback(knockbackDir * _knockbackForce);
                _playerController.SetIsShielded(false);
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            if (_playerController != null)
            {
                Vector2 knockbackDir = (_playerController.transform.position - collision.transform.position).normalized;

                _playerController.ApplyKnockback(knockbackDir * _knockbackForce);
                _playerController.SetIsShielded(false);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Meteor"))
        {
            if (_playerController != null)
            {
                Vector2 knockbackDir = (_playerController.transform.position - collision.transform.position).normalized;

                _playerController.ApplyKnockback(knockbackDir * _knockbackForce);
                _playerController.SetIsShielded(false);
            }

            Destroy(gameObject);
        }
    }

    public void SetPlayerController(PlayerController playerController) => _playerController = playerController;
}
