using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SimpleJoystick _joystick;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Transform _weapon;

    [Header("Settings")]
    [SerializeField] private float _playerSpeed = 3f;
    [SerializeField] private float _orbitRadius = 1.2f;
    [SerializeField] private float _dashSpeed = 8f;
    [SerializeField] private float _dashDuration = 0.1f;
    [SerializeField] private float _dashCooldown = 2f;
    [SerializeField] private int _health = 10;

    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private float _dashCooldownTimer = 0f;

    private Vector2 _input;
    private Vector2 _lastDirection;

    void Update()
    {
        // Cooldown tick
        _dashCooldownTimer -= Time.deltaTime;

        // Read input
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

        // Dash end logic
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
                _isDashing = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;

        if (_isDashing)
            velocity = _lastDirection * _dashSpeed;
        else if (_input.sqrMagnitude > 0.01f)
            velocity = _input * _playerSpeed;

        _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);
    }

    public void OnDashButtonPressed()
    {
        if (_dashCooldownTimer <= 0f && _lastDirection.sqrMagnitude > 0.01f)
        {
            _isDashing = true;
            _dashTimer = _dashDuration;
            _dashCooldownTimer = _dashCooldown;
        }
    }

    public void DecreaseHealth(int amount)
    {
        _health -= amount;
        _healthText.text = _health.ToString();

        if (_health <= 0)
            Destroy(gameObject);
    }
}
