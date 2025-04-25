using UnityEngine;
using Cinemachine;

public class IsometricCameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset = new Vector3(0, 10, -10); // Offset to keep the camera above and behind the player
    public float rotationSpeed = 5f; // Speed of rotating the camera

    private CinemachineVirtualCamera vCam;

    void Start()
    {
        // Get the Cinemachine Virtual Camera
        vCam = GetComponent<CinemachineVirtualCamera>();

        // Set the Follow and LookAt targets to the player
        vCam.Follow = player;
        vCam.LookAt = player;
        
        // Optional: Set up a fixed isometric rotation
        vCam.transform.eulerAngles = new Vector3(30, 45, 0); // Adjust to your desired isometric angle
    }

    void Update()
    {
        // Adjust position to create the offset
        vCam.transform.position = player.position + offset;

        // Optionally, you can allow camera rotation by holding a key (e.g., right-click to rotate)
        if (Input.GetMouseButton(1)) // Right mouse button for rotation
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            // Rotate the camera around the player
            vCam.transform.RotateAround(player.position, Vector3.up, horizontal * rotationSpeed);
            vCam.transform.RotateAround(player.position, vCam.transform.right, vertical * rotationSpeed);
        }
    }
}
