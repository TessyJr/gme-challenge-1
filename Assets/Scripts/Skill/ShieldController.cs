using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private float _knockbackForce = 5f;

    public void SetPlayerController(PlayerController playerController) => _playerController = playerController;

    public void StartShieldTimer(float duration)
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.shieldEffect);
        StartCoroutine(ShieldDurationCoroutine(duration));
    }

    private IEnumerator ShieldDurationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        DestroyShield();
    }

    public void DestroyShield()
    {
        if (_playerController != null)
        {
            _playerController.SetIsShielded(false);
        }

        Destroy(gameObject);
    }

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

            Destroy(gameObject); // this can be changed to DestroyShield() if you prefer
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") ||
            collision.gameObject.CompareTag("Meteor") ||
            collision.gameObject.CompareTag("Spike"))
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
}
