using UnityEngine;

public class SegmentAnimationController : MonoBehaviour
{
    public Animator segmentAnimator; // Reference to the Animator component of the segment
    public string moveStateName = "Move"; // Name of the animation state for "Move"
    private bool isMoving = false; // To track if the animation is playing

    void Update()
    {
        // Check if the animation is playing and whether it has completed
        if (isMoving && segmentAnimator != null)
        {
            AnimatorStateInfo stateInfo = segmentAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(moveStateName) && stateInfo.normalizedTime >= 1f)
            {
                // Animation has completed, reset isMove
                ResetSegmentAnimation();
            }
        }
    }

    // Function to trigger the "isMove" animation for this segment
    public void TriggerSegmentAnimation()
    {
        if (segmentAnimator != null)
        {
            segmentAnimator.SetBool("isMove", true);
            isMoving = true; // Mark as moving
        }
    }

    // Function to reset the animation trigger
    private void ResetSegmentAnimation()
    {
        if (segmentAnimator != null)
        {
            segmentAnimator.SetBool("isMove", false);
            isMoving = false; // Mark as no longer moving
        }
    }
}
