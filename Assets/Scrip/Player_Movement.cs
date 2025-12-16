using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : MonoBehaviour
{
    [Header("Components")]
    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;

    [Header("Movement Settings")]
    public float speed = 6f;
    public float sprintMultiplier = 1.8f;
    public float gravity = -10f;
    public float jumpHeight = 3f;
    private float verticalVelocity = 0f;
    private float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttacks();
    }

    void HandleMovement()
    {
        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isSprint = Input.GetKey(KeyCode.LeftShift);

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Animator Speed
        float animSpeed = Mathf.Clamp(moveDirection.magnitude, 0f, 0.5f) + (isSprint ? 0.5f : 0f);
        animator.SetFloat("Speed", animSpeed);

        // Jumping & Gravity
        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
                verticalVelocity = jumpHeight;
            else
                verticalVelocity = -1f; // Small value to keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Rotate Player
        if (moveDirection.magnitude >= 0.1f)
        {
            // Rotate relative to camera
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Move in camera direction
            moveDirection = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0) * moveDirection;
        }

        // Final movement
        Vector3 velocity = moveDirection * speed * (isSprint ? sprintMultiplier : 1f);
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAttacks()
    {
        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("Normal_Attack");

        if (Input.GetMouseButtonDown(1))
            animator.SetTrigger("Heavy_Attack");
    }

    // Called from animation event
    public void PlayerAttack()
    {
        Debug.Log("Player Attack Hit Event Triggered");
        Transform col = transform.Find("Colider");
        if (col != null)
        {
            col.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(DisableCollider(col));
        }
    }

    IEnumerator DisableCollider(Transform col)
    {
        yield return new WaitForSeconds(0.2f);
        col.GetComponent<BoxCollider>().enabled = false;
    }
}
