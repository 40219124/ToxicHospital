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
    private bool jumping = false;
    private float prevYVel = 0.0f;

    private enum eInputActionInstruction { none, interact }
    private eInputActionInstruction currentAction = eInputActionInstruction.none;
    private eInputActionInstruction nextAction = eInputActionInstruction.none;

    [SerializeField]
    private float moveSpeed = 3.0f;
    [SerializeField]
    private float moveAcceleration = 12.0f;
    [SerializeField]
    private float jumpForce = 13.0f;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
    }

    private void FixedUpdate()
    {
        PhysicsUpdate();
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
        if (jumping)
        {
            if(!(rigidbody.velocity.y < prevYVel))
            {
                jumping = false;
            }
        }

        prevYVel = rigidbody.velocity.y;

        if (currentMove == eInputMovementInstruction.none && nextMove == eInputMovementInstruction.none)
        {
            return;
        }
        Vector2 velocity = rigidbody.velocity;
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
            rigidbody.velocity = velocity;
        }

        if(nextMove == eInputMovementInstruction.jump && !jumping)
        {
            jumping = true;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce); // ~~~ only if grounded
            prevYVel = rigidbody.velocity.y + float.Epsilon;
        }

        if (velocity.Equals(Vector2.zero) && !jumping)
        {
            currentMove = eInputMovementInstruction.none;
        }
        else if (jumping)
        {
            currentMove = eInputMovementInstruction.jump;
        }
        else
        {
            currentMove = eInputMovementInstruction.walk; // ~~~ jump doesn't get a look in (might not matter)
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
}
