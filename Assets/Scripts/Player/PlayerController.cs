using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;

    [Header("Health Settings")]
    [SerializeField] private int _health = 50;
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private Canvas _healthCanvas;
    [SerializeField] private Image _healthBar;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private GameObject _damageTextPrefab;

    [Header("Movement Settings")]
    public SimpleJoystick _joystick;
    [SerializeField] private float _playerSpeed = 3f;

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weapon;
    [SerializeField] private float _orbitRadius = 1.2f;

    [Header("Damage Flash Settings")]
    [SerializeField] private GameObject _damageFlash;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private float dashingTrailTime = 1f;

    // DASH
    private float _dashSpeed;
    private bool _isDashing = false;
    private float _dashTimer = 0f;

    // SHIELD
    private bool _isShielded = false;

    // SPIKE
    private bool isTouchingSpike = false;
    private float spikeDamageTimer = 0f;
    private float spikeDamageInterval = .8f;

    private Vector2 _input;
    private Vector2 _lastDirection;
    private Vector2 _externalForce;

    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        _maxHealth = _health;
        UpdateHealthBar();
    }

    void Update()
    {
        _input = new Vector2(_joystick.Horizontal(), _joystick.Vertical());

        if (_input.sqrMagnitude > 0.01f)
        {
            _input.Normalize();
            _lastDirection = _input;

            // Orbit weapon
            _weapon.localPosition = _input * _orbitRadius;
            float angle = Mathf.Atan2(_input.y, _input.x) * Mathf.Rad2Deg;
            _weapon.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
                _isDashing = false;
        }

        if (isTouchingSpike)
        {
            spikeDamageTimer -= Time.deltaTime;
            if (spikeDamageTimer <= 0f)
            {
                DecreaseHealth(2);
                spikeDamageTimer = spikeDamageInterval;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        if (_isDashing)
            velocity = _lastDirection * _dashSpeed;
        else if (_input.sqrMagnitude > 0.01f)
            velocity = _input * _playerSpeed;

        Vector2 totalVelocity = velocity + _externalForce;
        _rb.MovePosition(_rb.position + totalVelocity * Time.fixedDeltaTime);

        _externalForce = Vector2.Lerp(_externalForce, Vector2.zero, 10f * Time.fixedDeltaTime);
    }

    public void Dash(float dashSpeed, float _dashDuration)
    {
        if (_lastDirection.sqrMagnitude > 0.01f)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.dashEffect);
            _dashSpeed = dashSpeed;
            _isDashing = true;
            _dashTimer = _dashDuration;
            StartCoroutine(DashTrail());
        }
    }

    public void DecreaseHealth(int amount)
    {
        if (_isShielded) return;

        _health -= amount;
        UpdateHealthBar();

        if (_health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        // Damage flash
        if (_damageFlash != null)
        {
            StartCoroutine(FlashDamageEffect());
        }

        // blood particle
        if (_bloodParticle != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.damagedEffect);
            Instantiate(_bloodParticle, transform.position, Quaternion.identity, transform);
        }

        // Shake health canvas
        StartCoroutine(ShakeHealthCanvas());

        // Show animated damage text
        ShowDamageText($"-{amount}");
    }

    private void UpdateHealthBar()
    {
        if (_healthBar != null)
        {
            float healthPercent = Mathf.Clamp01((float)_health / _maxHealth);

            RectTransform rt = _healthBar.rectTransform;
            Vector2 size = rt.sizeDelta;
            size.x = healthPercent * 1.6f;
            rt.sizeDelta = size;

            _healthBar.color = Color.Lerp(Color.red, Color.green, healthPercent);
        }

        if (_healthText != null)
        {
            _healthText.text = _health.ToString();
        }
    }

    private IEnumerator FlashDamageEffect()
    {
        _damageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _damageFlash.SetActive(false);
    }

    private IEnumerator ShakeHealthCanvas(float duration = 0.2f, float magnitude = 0.05f)
    {
        if (_healthCanvas == null) yield break;

        RectTransform canvasTransform = _healthCanvas.GetComponent<RectTransform>();
        Vector3 originalPos = canvasTransform.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            canvasTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasTransform.anchoredPosition = originalPos;
    }

    private void ShowDamageText(string text)
    {
        if (_damageTextPrefab == null || _healthCanvas == null) return;

        GameObject instance = Instantiate(_damageTextPrefab, _healthCanvas.transform);
        TextMeshProUGUI tmp = instance.GetComponent<TextMeshProUGUI>();
        tmp.text = text;

        RectTransform rt = instance.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0f, -0.5f); // start at center

        StartCoroutine(AnimateDamageText(tmp, rt));
    }

    private IEnumerator AnimateDamageText(TextMeshProUGUI tmp, RectTransform rt)
    {
        float duration = 0.8f;
        float elapsed = 0f;
        Color originalColor = tmp.color;
        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0f, -0.5f); // go downward

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(rt.gameObject);
    }

    public void ApplyKnockback(Vector2 force)
    {
        _externalForce = force;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            DecreaseHealth(2);
            isTouchingSpike = true;
            spikeDamageTimer = spikeDamageInterval;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            isTouchingSpike = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            isTouchingSpike = false;
        }
    }

    private IEnumerator DashTrail()
    {
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTrailTime);
        tr.emitting = false;
    }

    public int GetHealth() => _health;
    public void SetIsShielded(bool isShielded) => _isShielded = isShielded;
}
