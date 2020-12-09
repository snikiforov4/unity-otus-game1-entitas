using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    public void UpdateHealth(float newValue)
    {
        if (TryGetComponent<TextMesh>(out var textMesh))
        {
            textMesh.text = $"{newValue}";
        }
    }
}