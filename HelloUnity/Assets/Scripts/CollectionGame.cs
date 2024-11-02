using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectionGame : MonoBehaviour
{
    public Text collectionCountText;
    private int collectedCount = 0;

    private void Start()
    {
        UpdateCollectionCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            collectedCount++;
            UpdateCollectionCount();

            // rotate and disappear effect
            StartCoroutine(RotateAndHideCoin(other.gameObject));
        }
    }

    private void UpdateCollectionCount()
    {
        collectionCountText.text = "Collected: " + collectedCount;
    }

    private IEnumerator RotateAndHideCoin(GameObject coin)
    {
        float rotationSpeed = 720f; // degrees per second
        float duration = 0.5f; // rotation effect duration

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // rotate coin
            coin.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // hide coin after rotation
        coin.SetActive(false);
    }
}
