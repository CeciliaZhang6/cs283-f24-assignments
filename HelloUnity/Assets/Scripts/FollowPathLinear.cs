using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathLinear : MonoBehaviour
{
    public List<Transform> pathPoints;  // points defining the path
    public float duration = 3.0f;       // time to move between points

    private int currentPointIndex = 0;  
    private bool isMoving = false;      
    private Vector3 initialPosition;    
    private Quaternion initialRotation; 
    private Coroutine movementCoroutine;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // press "l" to move/stop
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isMoving)
            {
                StopMoving();
            }
            else
            {
                StartMoving();
            }
        }
    }

    void StartMoving()
    {
        if (pathPoints.Count > 1)
        {
            isMoving = true;
            movementCoroutine = StartCoroutine(FollowPath());
        }
        else
        {
            Debug.LogError("Path must have at least two points.");
        }
    }

    void StopMoving()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }

        // reset to the initial position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        isMoving = false;
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            // current and next points
            Transform startPoint = pathPoints[currentPointIndex];
            Transform endPoint = pathPoints[(currentPointIndex + 1) % pathPoints.Count];

            // move from current to next point
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
            {
                float u = timer / duration;

                // do lerp
                transform.position = Vector3.Lerp(startPoint.position, endPoint.position, u);

                // direction to face
                Vector3 direction = (endPoint.position - startPoint.position).normalized;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }

                yield return null;
            }

            currentPointIndex = (currentPointIndex + 1) % pathPoints.Count;
        }
    }
}
