using UnityEngine;

public class RespawnDetector : MonoBehaviour
{
    private Vector3 lastSafePosition; // Stores the last safe position of the player

    void Start()
    {
        // Set the initial safe position to the player's starting point
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            lastSafePosition = player.transform.position;
        }
    }

    void Update()
    {
        // Optionally, update the safe position when the player is grounded
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.GetComponent<CharacterController>().isGrounded)
        {
            lastSafePosition = player.transform.position;
        }
    }

    // OnTriggerEnter will trigger when the player enters the detector zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Respawn player at the last safe position
            RespawnPlayer(other.gameObject);
        }
    }

    // Function to respawn the player
    private void RespawnPlayer(GameObject player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false; // Disable CharacterController before moving the player
            player.transform.position = lastSafePosition; // Set player position to the last safe position
            controller.enabled = true; // Re-enable CharacterController after moving the player
        }
    }
}
