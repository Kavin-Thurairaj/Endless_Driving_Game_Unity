using UnityEngine;

public class RandomizeObject : MonoBehaviour
{

    [SerializeField]
    Vector3 localRotationMin = Vector3.zero;
    [SerializeField]
    Vector3 localRotationMax = Vector3.zero;
    [SerializeField]
    float localScaleMultiplierMin = 0.8f;
    [SerializeField]
    float localScaleMultiplierMax = 1.5f;

    Vector3 originalScale = Vector3.one;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    // On enabled will be execute only when the script is enabled.
    void OnEnabled()
    {
        transform.localRotation = Quaternion.Euler(Random.Range(localRotationMin.x,localRotationMax.x),Random.Range(localRotationMin.y, localRotationMax.y),Random.Range(localRotationMin.z,localRotationMax.z));
        transform.localScale = originalScale * Random.Range(localScaleMultiplierMin,localScaleMultiplierMax);
    }

    
}
