using System.Collections;
using UnityEngine;

public class SkillBuffParticleController : MonoBehaviour
{
    public void MoveParticle(Vector3 targetPosition)
    {
        StartCoroutine(MoveAndPlayEffect(targetPosition));
    }

    private IEnumerator MoveAndPlayEffect(Vector3 targetPosition)
    {
        float duration = 1f;
        float elapsed = 0f;

        Vector3 startPosition = gameObject.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
