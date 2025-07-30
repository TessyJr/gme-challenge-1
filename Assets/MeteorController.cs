using UnityEngine;
using System.Collections;

public class MeteorController : MonoBehaviour
{
    [Header("Indicator Settings")]
    [SerializeField] private GameObject _indicator;
    [SerializeField] private float _indicatorDuration = 1f;

    [Header("Stay Settings")]
    [SerializeField] private float _stayDuration = 2f;

    [Header("Knockback Settings")]
    [SerializeField] private float _knockbackForce = 5f;

    private bool _isReadyToDamage = false;
    private bool _hasDamaged = false;

    private void Start()
    {
        if (_indicator != null)
        {
            _indicator.transform.localScale = Vector3.zero;
            StartCoroutine(Lifecycle());
        }
    }

    private IEnumerator Lifecycle()
    {
        // SCALE INDICATOR PHASE
        float timer = 0f;
        while (timer < _indicatorDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / _indicatorDuration);
            _indicator.transform.localScale = Vector3.one * progress;
            yield return null;
        }

        _indicator.transform.localScale = Vector3.one;
        _isReadyToDamage = true;

        // STAY ACTIVE PHASE
        yield return new WaitForSeconds(_stayDuration);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamagePlayer(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamagePlayer(collision);
    }

    private void TryDamagePlayer(Collider2D collision)
    {
        if (!_isReadyToDamage || _hasDamaged) return;

        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DecreaseHealth(10);

                Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
                Vector2 knockback = knockbackDir * _knockbackForce;
                player.ApplyKnockback(knockback);

                _hasDamaged = true;
                Destroy(gameObject);
            }
        }
    }
}
