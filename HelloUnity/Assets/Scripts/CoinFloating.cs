using UnityEngine;

public class CoinFloating : MonoBehaviour
{
    public float floatSpeed = 0.5f; 
    public float floatHeight = 0.2f; 

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // float up and down
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
