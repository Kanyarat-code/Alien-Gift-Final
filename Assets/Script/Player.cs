using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactableLayer;

    private CharacterController controller;
    //   private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;

    protected override void Start()
    {
        base.Start();

        controller = GetComponent<CharacterController>();
        //     animator = GetComponent<Animator>();

        Debug.Log($"{IdentityName} has entered the game!");
    }

    private void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }
    }

    private void MovePlayer()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection =
            new Vector3(moveX, 0f, moveZ).normalized;

        Turn(moveDirection);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 movement = moveDirection * movementSpeed;
        movement.y = velocity.y;

        controller.Move(movement * Time.deltaTime);

        //    animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }

    private void Turn(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //     animator.SetTrigger("Jump");
    }

    private void Interaction()
    {
        RaycastHit hit;

        Vector3 rayOrigin = transform.position + transform.up * 0.8f;

        if (Physics.Raycast(
            rayOrigin,
            transform.forward,
            out hit,
            interactRange,
            interactableLayer))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Use(this);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(
            transform.position + transform.up * 0.8f,
            transform.forward * interactRange
        );
    }
}
