using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    CharacterController controller;
    Transform mainCamera;
    Animator animator;
    CharacterStats characterStats;

    public float speed = 6f;
    public float gravity = -10f;
    public float jumpHeight = 3f;  // Renamed for clarity
    float verticalVelocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool IsSprint = Input.GetKey(KeyCode.LeftShift);
        float sprint = IsSprint ? 1.8f : 1f;

        if (Input.GetMouseButtonDown(0))  // Left Click
        {
            animator.SetTrigger("Normal_Attack");
        }

        if (Input.GetMouseButtonDown(1))  // Right Click
        {
            animator.SetTrigger("Heavy_Attack");
        }

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        animator.SetFloat("Speed", Mathf.Clamp(moveDirection.magnitude, 0f, 0.5f) + (IsSprint ? 0.5f : 0));

        if (controller.isGrounded && Input.GetAxis("Jump") > 0)
            verticalVelocity = jumpHeight;
        else
            verticalVelocity += gravity * Time.deltaTime;


        if (moveDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        moveDirection = mainCamera.TransformDirection(moveDirection);

        moveDirection = new Vector3(moveDirection.x * speed * sprint, verticalVelocity, moveDirection.z * speed * sprint);
        controller.Move(moveDirection * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            Debug.Log("Player hit by Health");
            GetComponent<CharacterStats>().changeHealth(20f);
            System_Manager.system.PlaySound(System_Manager.system.Sounds[1], System_Manager.system.Player.position);
            Destroy(other.gameObject);
        }

    }
    public void PlayerAttack()
    {
        Debug.Log("Player Attack Hit Event Triggered");
        transform.Find("Colider").GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DisableCollider());
    }
    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.2f);
        transform.Find("Colider").GetComponent<BoxCollider>().enabled = false;
    }
}