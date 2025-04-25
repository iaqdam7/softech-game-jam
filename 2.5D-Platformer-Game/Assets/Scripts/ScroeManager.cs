using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public GameObject earringPanel; // UI panel to show when an earring is collected
    public GameObject pendantPanel; // UI panel to show when pendant design 1 is collected
    public GameObject pendant2Panel; // UI panel to show when pendant design 2 is collected
    public GameObject RingPanel; // UI panel to show when ring is collected
    public GameObject Ring;

    private List<GameObject> collectedSegments = new List<GameObject>(); // To store collected segments
    private bool canAddSegment = true; // To control the timing of adding segments
    private HashSet<Collider> collectedObjects = new HashSet<Collider>(); // To track collected objects

    void Start()
    {
        // Hide all pop-up panels initially
        earringPanel.SetActive(false);
        pendantPanel.SetActive(false);
        pendant2Panel.SetActive(false);
        RingPanel.SetActive(false);
        Ring.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collectedObjects.Contains(other)) return; // Avoid multiple collections of the same object

        // Handle earrings collection
        if (other.gameObject.CompareTag("Eearrings"))
        {
            collectedObjects.Add(other);
            ShowPopUp(earringPanel, 5f); // Show the earring pop-up panel for 1.1 seconds
            other.gameObject.SetActive(false); // Disable the collected earring
        }

        // Handle pendant design 1 collection
        else if (other.gameObject.CompareTag("Pendant"))
        {
            collectedObjects.Add(other);
            ShowPopUp(pendantPanel, 5f); // Show the pendant design 1 pop-up panel for 1.1 seconds
            other.gameObject.SetActive(false); // Disable the collected pendant
        }

        // Handle pendant design 2 collection
        else if (other.gameObject.CompareTag("Pendant_2"))
        {
            collectedObjects.Add(other);
            ShowPopUp(pendant2Panel, 5f); // Show the pendant design 2 pop-up panel for 1.1 seconds
            other.gameObject.SetActive(false); // Disable the collected pendant design 2
        }

        // Handle segment collection
        else if (other.gameObject.CompareTag("Segment") && canAddSegment)
        {
            collectedObjects.Add(other);
            StartCoroutine(AddSegmentWithDelay(other));
        }

        // Handle ring collection by player
        else if (other.gameObject.CompareTag("Ring"))
        {
            collectedObjects.Add(other);
            other.gameObject.SetActive(false); // Disable the collected ring

            ShowPopUp(RingPanel, 5f); // Show the Ring panel for 3.5 seconds
        }
    }

    private IEnumerator AddSegmentWithDelay(Collider other)
    {
        // Prevent adding another segment immediately
        canAddSegment = false;

        // Add segment to the list of collected segments
        collectedSegments.Add(other.gameObject);
        other.gameObject.SetActive(true); // Disable the segment instead of destroying it

        // Trigger the 'isMove' animation for the segment
        Animator segmentAnimator = other.GetComponent<Animator>();
        if (segmentAnimator != null)
        {
            segmentAnimator.SetBool("isMove", true); // Assuming "isMove" is a boolean parameter in the segment's Animator
        }

        // Wait for the animation to complete (adjust the time if necessary to match your animation length)
        yield return new WaitForSeconds(1f); // Adjust this duration to match the length of your "isMove" animation

        // If 3 or more segments are collected, combine them
        if (collectedSegments.Count >= 3)
        {
            // Wait for 2 seconds before showing the ring
            yield return new WaitForSeconds(2.2f); // Delay the ring activation for 2 seconds

            // Enable the Ring after delay and play its animation
            Ring.SetActive(true); // Activate the ring object and start playing its default animation

            // Deactivate the segments after collecting all three
            foreach (var segment in collectedSegments)
            {
                segment.SetActive(false); // Disable each segment
            }

            CombineSegments();
        }

        // Wait for 2 seconds before allowing the next segment to be added
        yield return new WaitForSeconds(2f);

        canAddSegment = true;
    }

    // Function to show the pop-up panel, trigger animation, and hide after a custom delay
    void ShowPopUp(GameObject panel, float delay)
    {
        panel.SetActive(true); // Show the panel

        // Trigger the panel's animation
        Animator animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("PopUp"); // Trigger the "PopUp" animation
        }

        // Hide the panel after the specified delay
        StartCoroutine(HidePanelAfterDelay(panel, delay));
    }

    // Coroutine to hide the pop-up panel after a delay
    private IEnumerator HidePanelAfterDelay(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(false); // Hide the panel
    }

    // Coroutine to hide the Ring panel after 3.5 seconds
    private IEnumerator HideRingPanelAfterDelay()
    {
        yield return new WaitForSeconds(3.5f); // Show for 3.5 seconds
        RingPanel.SetActive(false); // Hide the Ring panel
    }

    // Function to combine collected segments
    void CombineSegments()
    {
        // Logic for combining segments (e.g., visual effect)
        collectedSegments.Clear(); // Clear the collected segments after combining
    }
}
