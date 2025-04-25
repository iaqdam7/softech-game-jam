using UnityEngine;

public class FloatingSegment : MonoBehaviour
{
    public float floatSpeed = 1f;  // Speed at which the segment floats
    public float floatHeight = 1.5f;  // Height of the floating movement

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the segment (unique for each prefab)
        startPosition = transform.position;
    }

    void Update()
    {
        // Use Mathf.PingPong for smooth floating movement, it moves back and forth
        float yMovement = Mathf.PingPong(Time.time * floatSpeed, floatHeight);

        // Apply the floating effect to the Y axis, keeping X and Z fixed
        transform.position = new Vector3(startPosition.x, startPosition.y + yMovement, startPosition.z);
    }
}
