using UnityEngine;

public class SkillBuff : MonoBehaviour
{
    [SerializeField] private Skill _skill;
    private SkillBuffSpawner _spawner;

    [Header("Floating Animation")]
    [SerializeField] private float _floatAmplitude = 0.02f;
    [SerializeField] private float _floatFrequency = 3f;

    private Vector3 _startPos;
    private bool _isTaken = false;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        FloatMotion();
    }

    private void FloatMotion()
    {
        float offsetY = Mathf.Sin(Time.time * _floatFrequency) * _floatAmplitude;
        transform.position = _startPos + new Vector3(0f, offsetY, 0f);
    }

    public void SetSpawner(SkillBuffSpawner spawner) => _spawner = spawner;
    public Skill GetSkill() => _skill;

    public bool TakeBuff()
    {
        if (!_isTaken)
        {
            _isTaken = true;
            _spawner?.OnBuffTaken();
            Destroy(gameObject);

            return true;
        }

        return false;
    }
}
