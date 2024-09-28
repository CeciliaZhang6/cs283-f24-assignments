using UnityEngine;

public class Tour : MonoBehaviour
{
    public Transform[] pointsOfInterest;  // array of POIs
    public float speed = 2.0f;            // speed of the transition between POIs
    public Transform character;           // main character
    public Vector3 cameraOffset = new Vector3(0, 2, -5); // offset for the camera relative to the character

    private int currentPOIIndex = 0;      
    private Vector3 startPosition;        
    private Quaternion startRotation;     
    private Vector3 characterStartPosition; 
    private Quaternion characterStartRotation; 
    private float transitionProgress = 0f;
    private bool isTransitioning = false;  

    void Update()
    {
        // move to next POI when pressing N
        if (Input.GetKeyDown(KeyCode.N) && !isTransitioning)
        {
            StartTransition();
        }

        if (isTransitioning)
        {
            UpdateTransition();
        }
    }

    void StartTransition()
    {
        // get current position and rotation of camera and character
        startPosition = Camera.main.transform.position;
        startRotation = Camera.main.transform.rotation;
        characterStartPosition = character.position;
        characterStartRotation = character.rotation;

        // move to next POI
        currentPOIIndex = (currentPOIIndex + 1) % pointsOfInterest.Length;

        transitionProgress = 0f;
        isTransitioning = true;
    }

    void UpdateTransition()
    {
        // compute transition progress based on speed and time
        transitionProgress += Time.deltaTime * speed;

        // interpolate character's position and rotation
        character.position = Vector3.Lerp(characterStartPosition, pointsOfInterest[currentPOIIndex].position, transitionProgress);
        character.rotation = Quaternion.Slerp(characterStartRotation, pointsOfInterest[currentPOIIndex].rotation, transitionProgress);

        // calculate camera position with offset
        Vector3 targetCameraPosition = character.position + character.transform.TransformDirection(cameraOffset);
        Camera.main.transform.position = Vector3.Lerp(startPosition, targetCameraPosition, transitionProgress);

        // align camera rotation with character
        Camera.main.transform.rotation = Quaternion.Slerp(startRotation, character.rotation, transitionProgress);

        if (transitionProgress >= 1f)
        {
            isTransitioning = false;
        }
    }
}
