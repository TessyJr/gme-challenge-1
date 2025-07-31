using UnityEngine;
using System.Collections;

public class SkillBuffSpawner : MonoBehaviour
{
    [SerializeField] private SkillBuff[] _skillBuffs;
    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;

    private SkillBuff _currentBuff;

    private void Start()
    {
        StartCoroutine(SpawnSkillBuffWithDelay(5f)); // Initial delay
    }

    private IEnumerator SpawnSkillBuffWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_skillBuffs.Length == 0) yield break;

        SkillBuff randomSkillBuff = _skillBuffs[Random.Range(0, _skillBuffs.Length)];
        _currentBuff = Instantiate(randomSkillBuff, _spawnPosition, Quaternion.identity);

        // Tell the buff who spawned it (so it can notify when taken)
        _currentBuff.SetSpawner(this);
    }

    // Called by the SkillBuff object when it's picked up
    public void OnBuffTaken()
    {
        _currentBuff = null;
        StartCoroutine(SpawnSkillBuffWithDelay(5f)); // Wait another 5s before spawning next
    }
}
