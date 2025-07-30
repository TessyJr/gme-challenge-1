using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Arena Settings")]
    [SerializeField] private GameObject _topArenaBorder;
    [SerializeField] private GameObject _bottomArenaBorder;
    [SerializeField] private GameObject _leftArenaBorder;
    [SerializeField] private GameObject _rightArenaBorder;
    [SerializeField] private float _offsetBorder = 1f;

    [Header("Meteor Settings")]
    [SerializeField] private GameObject _meteor;
    [SerializeField] private int _meteorAmount = 3;
    [SerializeField] private float _spawnInterval = 5f;

    private float _topY, _bottomY, _leftX, _rightX;

    private void Start()
    {
        CalculateBounds();
        InvokeRepeating(nameof(SpawnMeteors), 1f, _spawnInterval);
    }

    private void CalculateBounds()
    {
        if (_topArenaBorder == null || _bottomArenaBorder == null || _leftArenaBorder == null || _rightArenaBorder == null)
            return;

        Vector3 topCenter = _topArenaBorder.transform.position;
        Vector3 bottomCenter = _bottomArenaBorder.transform.position;
        Vector3 leftCenter = _leftArenaBorder.transform.position;
        Vector3 rightCenter = _rightArenaBorder.transform.position;

        float topHalfHeight = _topArenaBorder.transform.localScale.y / 2f;
        float bottomHalfHeight = _bottomArenaBorder.transform.localScale.y / 2f;
        float leftHalfWidth = _leftArenaBorder.transform.localScale.x / 2f;
        float rightHalfWidth = _rightArenaBorder.transform.localScale.x / 2f;

        _topY = topCenter.y - topHalfHeight - _offsetBorder;
        _bottomY = bottomCenter.y + bottomHalfHeight + _offsetBorder;
        _leftX = leftCenter.x - leftHalfWidth - _offsetBorder;
        _rightX = rightCenter.x + rightHalfWidth + _offsetBorder;
    }

    private void SpawnMeteors()
    {
        for (int i = 0; i < _meteorAmount; i++)
        {
            float x = Random.Range(_leftX, _rightX);
            float y = Random.Range(_bottomY, _topY);

            Vector3 spawnPosition = new(x, y, 0f);
            Instantiate(_meteor, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        if (_topArenaBorder == null || _bottomArenaBorder == null || _leftArenaBorder == null || _rightArenaBorder == null)
            return;

        Vector3 topCenter = _topArenaBorder.transform.position;
        Vector3 bottomCenter = _bottomArenaBorder.transform.position;
        Vector3 leftCenter = _leftArenaBorder.transform.position;
        Vector3 rightCenter = _rightArenaBorder.transform.position;

        float topHalfHeight = _topArenaBorder.transform.localScale.y / 2f;
        float bottomHalfHeight = _bottomArenaBorder.transform.localScale.y / 2f;
        float leftHalfWidth = _leftArenaBorder.transform.localScale.x / 2f;
        float rightHalfWidth = _rightArenaBorder.transform.localScale.x / 2f;

        float topY = topCenter.y - topHalfHeight - _offsetBorder;
        float bottomY = bottomCenter.y + bottomHalfHeight + _offsetBorder;
        float leftX = leftCenter.x - leftHalfWidth - _offsetBorder;
        float rightX = rightCenter.x + rightHalfWidth + _offsetBorder;

        Vector3 topLeft = new(leftX, topY, 0f);
        Vector3 topRight = new(rightX, topY, 0f);
        Vector3 bottomLeft = new(leftX, bottomY, 0f);
        Vector3 bottomRight = new(rightX, bottomY, 0f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
