using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    #region Setup
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    #endregion Setup

    public float moveSpeed;
    bool jumping;
    float distToGround;

    public Animator animator;

    [Header("Required Fields")]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    Vector2 moveVector = Vector2.zero;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float runSpeed = 6.0f;
    public float walkSpeed = 2.0f;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }

  
    // Update is called once per frame
    void Update()
    {
        // Grounded - Allow Jump
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.up * jumpVelocity;
            jumping = true;
            //AudioManager.instance.PlaySound("jump");
        }
        // Falling - Apply Gravity Multiplier
        if (rb.velocity.y < 0)
        {
            jumping = false;
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Bigger Jumps - Lower Gravity Multiplier
        else if (jumping && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVector = moveVector.normalized;

        if (moveVector != Vector2.zero)
        {
            float targetRot = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref turnSmoothVelocity, turnSmoothTime);
        }
        animator.SetBool("Moving", moveVector != Vector2.zero);

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * moveVector.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            CallbackHandler.instance.StartHoming(transform);
        }
    }

    private void FixedUpdate()
    {
        // Move Character
        rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
    }

    // Check if Player is Grounded
    public bool IsGrounded()
    {
        // Prevents Grounded at Start of Jump
        if (jumping)
            return false;

        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}