using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE,
        MOVING,
        CLIMBING,
        JUMPING,
        FALLING
    }


    VirtualInputs VInputs;
    Rigidbody RB;
    Animator anims;

    [Tooltip("This can't be set, and sets to IDLE on Start call, so no touchy")]
    public PlayerStates PlayerState;

    [Header("Forces")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float inAirSpeed = 1.0f;
    private float setSpeed;

    public float jumpForce = 5.0f;
    public float stickForce = 1.0f;

    
    [Header("GroundChecks")]
    public LayerMask GroundLayers;
    public float GroundCheckDistance = 1.0f;
    public float GroundCheckRadius = 0.1f;
    public Vector3 GroundCheckStartOffset = Vector3.zero;

    [Header("Misc")]
    public bool ParentToGround = true;
    // Start is called before the first frame update
    void Start()
    {
        setSpeed = walkSpeed;

        PlayerState = PlayerStates.IDLE;

        RB = GetComponent<Rigidbody>();
        anims = GetComponentInChildren<Animator>();

        VInputs = GetComponent<VirtualInputs>();
        VInputs.GetInputListener("Forward").MethodToCall.AddListener(Forward);
        VInputs.GetInputListener("Back").MethodToCall.AddListener(Back);
        VInputs.GetInputListener("Left").MethodToCall.AddListener(Left);
        VInputs.GetInputListener("Right").MethodToCall.AddListener(Right);
        VInputs.GetInputListener("Run").MethodToCall.AddListener(Run);
        VInputs.GetInputListener("Jump").MethodToCall.AddListener(Jump);
    }

    private void Update()
    {
        SetAnimations();
        SetCurrentPlayerState();
    }

    Vector3 inputAxis = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {
        MovementHandling(inputAxis);
    }


    void SetCurrentPlayerState()
    {
        if (IDLECheck())
        {
            PlayerState = PlayerStates.IDLE;
        }

        if (MOVINGCheck())
        {
            PlayerState = PlayerStates.MOVING;
        }

        if (JUMPINGCheck())
        {
            PlayerState = PlayerStates.JUMPING;
        }

        if (FALLINGCheck())
        {
            PlayerState = PlayerStates.FALLING;
        }


        if (CLIMBINGCheck()) //Currently disabled
        {
            PlayerState = PlayerStates.CLIMBING;
        }
    }

    #region PlayerStateChecks
    bool IDLECheck()
    {
        return RB.velocity.magnitude == 0;
    }

    bool MOVINGCheck()
    {
        return RB.velocity.magnitude > 0;
    }

    bool CLIMBINGCheck()//ToDo Later when implementing climbing
    {
        return false;
    }
    bool JUMPINGCheck()
    {
        return RB.velocity.y > 0.0f && !IsGrounded();
    }
    bool FALLINGCheck()
    {
        return RB.velocity.y < 0.0f && !IsGrounded();
    }
    #endregion


    /// <summary>
    /// Takes a Vector3 with the direction inputs as axis on the +/- scale
    /// </summary>
    /// <param name="_inputAxis">Will read each axis as an input, e.g x = 1 as left, x = -1 as right, etc</param>
    void MovementHandling(Vector3 _inputAxis)
    {
        RaycastHit rh;

        Physics.SphereCast(transform.position + GroundCheckStartOffset, GroundCheckRadius,
            Vector3.down, out rh,
            GroundCheckDistance, GroundLayers.value);



        switch (PlayerState)
        {
            case PlayerStates.IDLE:
            case PlayerStates.MOVING:
                if (rh.collider != null && ParentToGround)
                {
                    transform.parent = rh.transform;
                }

                RB.velocity = new Vector3( /*X*/ RB.velocity.x + (_inputAxis.x * setSpeed),
                                           /*Y*/ RB.velocity.y,
                                           /*Z*/ RB.velocity.z + (_inputAxis.z * setSpeed));

                RB.velocity = Vector3.ClampMagnitude(RB.velocity, setSpeed);
                
                RB.velocity = new Vector3(RB.velocity.x, (inputAxis.y * jumpForce) - 0.1f,
                    RB.velocity.z);

                break;
            
            case PlayerStates.JUMPING:
            case PlayerStates.FALLING:
                RB.velocity = new Vector3( /*X*/ RB.velocity.x + (_inputAxis.x * inAirSpeed),
                                           /*Y*/ RB.velocity.y,
                                           /*Z*/ RB.velocity.z + (_inputAxis.z * inAirSpeed));
                transform.parent = null; //maybe disable this later? experiment

                break;
            default:
                break;
        }
        
    }

    void SetAnimations()
    {
        Vector3 flatRBV = RB.velocity;
        flatRBV.y = 0.0f;
        flatRBV = Vector3.ClampMagnitude(flatRBV, setSpeed);
        anims.SetFloat("MovementSpeed", flatRBV.magnitude / runSpeed);

    }
    #region InputMethods


    void Forward(InputState type)
    {
        switch (type)
        {
            case InputState.KEYDOWN:
                inputAxis.z = 1;
                break;
            case InputState.KEYHELD:
                break;
            case InputState.KEYUP:
                inputAxis.z = 0;
                break;
            default:
                break;
        }
        //MovementHandling(new Vector3(0, 0, 1));
    }
    void Back(InputState type)
    {
        switch (type)
        {
            case InputState.KEYDOWN:
                inputAxis.z = -1;
                break;
            case InputState.KEYHELD:
                break;
            case InputState.KEYUP:
                inputAxis.z = 0;
                break;
            default:
                break;
        }
        //MovementHandling(new Vector3(0, 0, -1));
    }
    void Left(InputState type)
    {
        switch (type)
        {
            case InputState.KEYDOWN:
                inputAxis.x = -1;
                break;
            case InputState.KEYHELD:
                break;
            case InputState.KEYUP:
                inputAxis.x = 0;
                break;
            default:
                break;
        }
    }
    void Right(InputState type)
    {
        switch (type)
        {
            case InputState.KEYDOWN:
                inputAxis.x = 1;
                break;
            case InputState.KEYHELD:
                break;
            case InputState.KEYUP:
                inputAxis.x = 0;
                break;
            default:
                break;
        }
        //MovementHandling(new Vector3(1, 0, 0));
    }
    void Jump(InputState type)
    {
        switch (type)
        {
            case InputState.KEYDOWN:
                if (PlayerState == PlayerStates.MOVING)
                {
                    anims.SetTrigger("Jump");

                }
                inputAxis.y = 1;
                break;
            case InputState.KEYHELD:
                break;
            case InputState.KEYUP:
                inputAxis.y = 0;
                break;
            default:
                break;
        }
      
    }
    void Run(InputState type)
    {
        switch (type)
        {
            case InputState.KEYDOWN:
                setSpeed = runSpeed;
                break;
            case InputState.KEYUP:
                setSpeed = walkSpeed;
                break;
            default:
                break;
        }
    }



    #endregion InputMethods

    #region Utility
    public bool IsGrounded()
    {
        RaycastHit rh;
        return Physics.SphereCast(transform.position + GroundCheckStartOffset, GroundCheckRadius,
            Vector3.down ,out rh ,
            GroundCheckDistance, GroundLayers.value);
    }
    private void OnDrawGizmos()
    {
        Vector3 offsetPos = transform.position + GroundCheckStartOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(offsetPos, 0.1f);

        Gizmos.DrawLine(offsetPos, offsetPos + (Vector3.down * GroundCheckDistance));
    }

    #endregion


}
