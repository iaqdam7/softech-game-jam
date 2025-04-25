using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TPPPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float teleportLiftHeight = 5f;
    public float teleportDuration = 2f;
    public LayerMask clickableLayer;
    public float teleportRadius = 10f;
    public Color highlightColor = Color.green;
    public Color defaultColor = Color.white;

    public GameObject teleportEffectUI1; // First UI (7 sec)

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isTeleporting = false;
    private bool isInputDisabled = false;
    private Vector3 targetPosition;
    private float teleportTimer = 0f;

    private List<MeshRenderer> teleportableObjects = new List<MeshRenderer>();
    private MeshRenderer lastClickedRenderer = null;
    private Dictionary<MeshRenderer, GameObject> teleportableObjectsDict = new Dictionary<MeshRenderer, GameObject>();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Teleportable");
        foreach (GameObject obj in objects)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                teleportableObjects.Add(renderer);
                teleportableObjectsDict[renderer] = obj;
                obj.SetActive(false); // Initially hidden
            }
        }

        // Hide UI elements at start
        if (teleportEffectUI1) teleportEffectUI1.SetActive(false);

        // Start the UI sequence at game start
        StartCoroutine(StartGameUISequence());
    }

    IEnumerator StartGameUISequence()
    {
        isInputDisabled = true; // Disable input at game start

        if (teleportEffectUI1) teleportEffectUI1.SetActive(true);
        yield return new WaitForSeconds(7f);

        if (teleportEffectUI1) teleportEffectUI1.SetActive(false);

        isInputDisabled = false; // Enable input after UI sequence
    }

    void Update()
    {
        if (isInputDisabled) return;

        isGrounded = controller.isGrounded;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        bool isRunning = direction.magnitude >= 0.1f;
        animator.SetBool("isRunning", isRunning);

        if (isRunning)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }

        // Handle teleportable objects visibility
        UpdateTeleportableObjects();

        if (Input.GetMouseButtonDown(0) && !isTeleporting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
            {
                MeshRenderer clickedRenderer = hit.collider.GetComponent<MeshRenderer>();
                if (clickedRenderer != null && teleportableObjects.Contains(clickedRenderer))
                {
                    float distance = Vector3.Distance(transform.position, clickedRenderer.transform.position);
                    if (distance <= teleportRadius)
                    {
                        targetPosition = hit.point;
                        isTeleporting = true;
                        teleportTimer = 0f;
                        animator.SetTrigger("teleportJump");
                        lastClickedRenderer = clickedRenderer;
                    }
                }
            }
        }

        if (isTeleporting)
        {
            Teleport();
        }

        if (!isTeleporting)
        {
            ApplyGravity();
        }
    }

    void UpdateTeleportableObjects()
    {
        foreach (MeshRenderer renderer in teleportableObjects)
        {
            GameObject obj = teleportableObjectsDict[renderer];
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance <= teleportRadius)
            {
                obj.SetActive(true); // Show if player is in range
            }
            else
            {
                obj.SetActive(false); // Hide if out of range
            }
        }
    }

    void Teleport()
    {
        teleportTimer += Time.deltaTime;
        float progress = Mathf.SmoothStep(0f, 1f, teleportTimer / teleportDuration);
        Vector3 currentPosition = transform.position;
        Vector3 teleportPosition = Vector3.Lerp(currentPosition, targetPosition, progress);
        float height = Mathf.Sin(Mathf.PI * progress) * teleportLiftHeight;
        teleportPosition.y += height;
        controller.Move(teleportPosition - transform.position);

        if (progress >= 1f)
        {
            isTeleporting = false;
            animator.ResetTrigger("teleportJump");
            if (lastClickedRenderer != null)
            {
                lastClickedRenderer = null;
            }

            isInputDisabled = false; // Re-enable input immediately after teleport
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}