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

    public float jumpForce = 5.0f;

    
    [Header("GroundChecks")]
    public LayerMask GroundLayers;
    public float GroundCheckDistance = 1.0f;
    public Vector3 GroundCheckStartOffset = Vector3.zero;


    private float setSpeed;
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
        VInputs.GetInputListener("RunDown").MethodToCall.AddListener(RunDown);
        VInputs.GetInputListener("RunUp").MethodToCall.AddListener(RunUp);

        VInputs.GetInputListener("Jump").MethodToCall.AddListener(Jump);
    }

    private void Update()
    {
        SetAnimations();
        SetCurrentPlayerState();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
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
        switch (PlayerState)
        {
            case PlayerStates.IDLE:
            case PlayerStates.MOVING:
                RB.velocity = new Vector3( /*X*/ RB.velocity.x + (_inputAxis.x * setSpeed),
                                           /*Y*/ RB.velocity.y,
                                           /*Z*/ RB.velocity.z + (_inputAxis.z * setSpeed));

                RB.velocity = Vector3.ClampMagnitude(RB.velocity, setSpeed);
                
                RB.velocity = new Vector3(RB.velocity.x, _inputAxis.y * jumpForce, RB.velocity.z);

                break;
            
            case PlayerStates.CLIMBING:
                break;
            case PlayerStates.JUMPING:
                break;
            case PlayerStates.FALLING:
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


    void Forward()
    {
        MovementHandling(new Vector3(0, 0, 1));
    }
    void Back()
    {
        MovementHandling(new Vector3(0, 0, -1));
    }
    void Left()
    {
        MovementHandling(new Vector3(-1, 0, 0));
    }
    void Right()
    {
        MovementHandling(new Vector3(1, 0, 0));
    }
    void Jump()
    {
        anims.SetTrigger("Jump");
        MovementHandling(new Vector3(0, 1, 0));
    }
    void RunDown()
    {
        setSpeed = runSpeed;
    }
    void RunUp()
    {
        setSpeed = walkSpeed;
    }
    #endregion InputMethods

    #region Utility
    public bool IsGrounded()
    {
        RaycastHit rh;
        return Physics.Raycast(transform.position + GroundCheckStartOffset, 
            Vector3.down ,out rh ,
            GroundCheckDistance, GroundLayers.value);
    }
    private void OnDrawGizmos()
    {
        Vector3 offsetPos = transform.position + GroundCheckStartOffset;

        Gizmos.color = Color.cyan;
        //Gizmos.DrawSphere(offsetPos, SphereCastRadius);

        Gizmos.DrawLine(offsetPos, offsetPos + (Vector3.down * GroundCheckDistance));
    }

    #endregion


}
