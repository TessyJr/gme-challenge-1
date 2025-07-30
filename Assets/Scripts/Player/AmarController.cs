using UnityEngine;
using UnityEngine.UI;

public class AmarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("UI Sliders")]
    [SerializeField] private Slider sliderA;
    [SerializeField] private Slider sliderB;
    [SerializeField] private Slider sliderC;
    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
                Debug.LogError("Missing Rigidbody2D on this GameObject.");
        }
    }

    private void Update()
    {
        float targetX = sliderA != null ? sliderA.value : 0f;
        float targetY = sliderB != null ? sliderB.value : 0f;
        float rotation = sliderC != null ? sliderC.value : 0f;

        rb.position = new Vector2(targetX, targetY);

        rb.rotation = rotation;
    }
}
