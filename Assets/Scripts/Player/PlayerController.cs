using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;

    [Header("Health Settings")]
    [SerializeField] private int _health = 10;
    [SerializeField] private TextMeshProUGUI _healthText;

    [Header("Movement Settings")]
    public SimpleJoystick _joystick;
    [SerializeField] private float _playerSpeed = 3f;

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weapon;
    [SerializeField] private float _orbitRadius = 1.2f;

    // DASH
    private float _dashSpeed;
    private bool _isDashing = false;
    private float _dashTimer = 0f;

    //SHIELD
    private bool _isShielded = false;

    //SPIKE
    private bool isTouchingSpike = false;
    private float spikeDamageTimer = 0f;
    private float spikeDamageInterval = .8f;

    private Vector2 _input;
    private Vector2 _lastDirection;
    private Vector2 _externalForce; // <-- Knockback force

    void Start()
    {
        _healthText.text = _health.ToString();
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

        // Decay knockback force
        _externalForce = Vector2.Lerp(_externalForce, Vector2.zero, 10f * Time.fixedDeltaTime);
    }

    public void Dash(float dashSpeed, float _dashDuration)
    {
        if (_lastDirection.sqrMagnitude > 0.01f)
        {
            _dashSpeed = dashSpeed;
            _isDashing = true;
            _dashTimer = _dashDuration;
        }
    }

    public void DecreaseHealth(int amount)
    {
        if (_isShielded) return;

        _health -= amount;
        _healthText.text = _health.ToString();

        if (_health <= 0)
            Destroy(gameObject);
    }

    public void ApplyKnockback(Vector2 force)
    {
        _externalForce = force;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "SpikeAbove")
        {
            Debug.Log("HIT SPIKE PERTAMA");
            DecreaseHealth(2);
            isTouchingSpike = true;
            spikeDamageTimer = spikeDamageInterval;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "SpikeAbove")
        {
            isTouchingSpike = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "SpikeAbove")
        {
            isTouchingSpike = false;
        }
    }

    public int GetHealth() => _health;
    public void SetIsShielded(bool isShielded) => _isShielded = isShielded;
}
