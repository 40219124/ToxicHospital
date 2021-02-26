using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    eInteractionRequirement currentCharacter = eInteractionRequirement.journalist;

    private enum eInputMovementInstruction { none, walk, jump }
    private eInputMovementInstruction currentMove = eInputMovementInstruction.none;
    private eInputMovementInstruction nextMove = eInputMovementInstruction.none;
    private Vector2 inputDirection;

    private enum eInputActionInstruction { none, interact }
    private eInputActionInstruction currentAction = eInputActionInstruction.none;
    private eInputActionInstruction nextAction = eInputActionInstruction.none;

    [SerializeField]
    private float moveSpeed = 3.0f;
    [SerializeField]
    private float moveAcceleration = 12.0f;
    [SerializeField]
    private float jumpForce = 13.0f;

    private Vector2 playerMoveVelocity = Vector2.zero;

    private Rigidbody2D rigidbody;
    private Collider2D collider;

    private GroundChecker groundChecker;

    private Animator animator;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
        AnimatorUpdate();
    }

    private void FixedUpdate()
    {
        PhysicsUpdate();
    }

    private void AnimatorUpdate()
    {
        animator.SetBool("FacingRight", playerMoveVelocity.x == 0 ? animator.GetBool("FacingRight") : playerMoveVelocity.x > 0);
        animator.SetInteger("MoveAction", (int)currentMove);
    }

    private void InputUpdate()
    {
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (currentMove != eInputMovementInstruction.jump && !inputDirection.Equals(Vector2.zero) && nextMove != eInputMovementInstruction.jump)
        {
            nextMove = eInputMovementInstruction.walk;
        }
        if (Input.GetButtonDown("Jump") && currentMove != eInputMovementInstruction.jump)
        {
            nextMove = eInputMovementInstruction.jump; // ~~~ buffer this for half a second and clear if nothing happens
        }

        if (Input.GetButtonDown("Interact"))
        {
            nextAction = eInputActionInstruction.interact; // ~~~ buffer this for half a second and clear if nothing happens
        }
    }

    private void PhysicsUpdate()
    {
        if (currentMove == eInputMovementInstruction.none && nextMove == eInputMovementInstruction.none && rigidbody.velocity.Equals(Vector2.zero))
        {
            return;
        }
        Vector2 velocity = new Vector2(playerMoveVelocity.x, rigidbody.velocity.y);
        float targetXVelocity = inputDirection.x * moveSpeed;
        float velocityDiff = targetXVelocity - velocity.x;
        if (velocityDiff != 0.0f)
        {
            float acceleration = 0.0f;
            acceleration = (velocityDiff > 0 ? 1.0f : -1.0f) * moveAcceleration * Time.fixedDeltaTime;
            if (Mathf.Abs(acceleration) > Mathf.Abs(velocityDiff))
            {
                acceleration = velocityDiff;
            }
            velocity += new Vector2(acceleration, 0.0f);
            playerMoveVelocity = new Vector2(velocity.x, 0.0f);
        }
        rigidbody.velocity = velocity;

        if (nextMove == eInputMovementInstruction.jump && groundChecker.PlayerCanJump)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            groundChecker.PlayerJumpStart(jumpForce);
        }

        if (velocity.Equals(Vector2.zero) && !groundChecker.PlayerIsJumping)
        {
            currentMove = eInputMovementInstruction.none;
        }
        else if (groundChecker.PlayerIsJumping)
        {
            currentMove = eInputMovementInstruction.jump;
        }
        else
        {
            currentMove = eInputMovementInstruction.walk;
        }
        nextMove = eInputMovementInstruction.none;
    }

    private void SwapCharacter()
    {
        if (currentCharacter == eInteractionRequirement.journalist)
        {
            currentCharacter = eInteractionRequirement.porter;
        }
        else
        {
            currentCharacter = eInteractionRequirement.journalist;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*Debug.Log("Start Collision Print");
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        collider.GetContacts(contacts);
        foreach (ContactPoint2D point in contacts)
        {
            Debug.Log($"Contact normal {point.normal}");
        }
        Debug.Log("End Collision Print");*/
    }

    public bool IsJumping
    {
        get
        {
            return groundChecker.PlayerIsJumping;
        }
    }
}
