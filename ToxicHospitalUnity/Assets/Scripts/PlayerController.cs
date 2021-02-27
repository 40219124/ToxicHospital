using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ePlayerAction
{
    none = 0,
    waiting = 1 << 0,
    interacting = 1 << 1,
    walking = 1 << 2,
    jumping = 1 << 3,
    pushing = 1 << 4,
    swapping = 1 << 5,
    isJustMovement = none | walking | jumping,
    stopsXMovement = waiting | interacting | swapping,
    slowsXAccVel = pushing,
    blocksJumping = waiting | interacting | jumping | pushing | swapping,
    blocksInteracting = interacting,
    rememberInteractable = waiting | interacting | pushing,
    directionLocked = pushing | interacting | waiting,
    blocksSwapping = waiting | interacting | jumping | pushing | swapping
    // ~~~ stopsInteracting = inMenu???    
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }

    [SerializeField]
    eInteractionRequirement currentCharacter = eInteractionRequirement.porter;

    private enum eInputMovementInstruction { none, walk, jump }
    private eInputMovementInstruction currentMove = eInputMovementInstruction.none;
    private eInputMovementInstruction nextMove = eInputMovementInstruction.none;
    private Vector2 inputDirection;

    private enum eInputActionInstruction { none, interact, swap }
    private eInputActionInstruction currentAction = eInputActionInstruction.none;
    private eInputActionInstruction nextAction = eInputActionInstruction.none;

    private ePlayerAction playerAction = ePlayerAction.none;

    [SerializeField]
    private float moveSpeed = 3.0f;
    [SerializeField]
    private float slowSpeedCoefficient = 0.5f;
    [SerializeField]
    private float moveAcceleration = 12.0f;
    [SerializeField]
    private float slowAccelerationCoefficient = 0.5f;
    [SerializeField]
    private float jumpForce = 13.0f;

    private Vector2 playerMoveVelocity = Vector2.zero;

    private Rigidbody2D rigidbody;
    private Collider2D collider;

    private GroundChecker groundChecker;
    private PlayerInteractor interactor;
    private PlayerHitboxChooser hitboxChooser;
    private PlayerSwapToMillieCheck millieSwapChecker;

    private BaseInteractable currentInteraction;

    private Animator animator;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        interactor = GetComponentInChildren<PlayerInteractor>();
        hitboxChooser = GetComponentInChildren<PlayerHitboxChooser>();
        millieSwapChecker = GetComponentInChildren<PlayerSwapToMillieCheck>();
        animator = GetComponentInChildren<Animator>();
        SetAnimatorCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
        ActionUpdate();
        AnimatorUpdate();
        hitboxChooser.ChooseHitbox(currentCharacter, playerAction);
        if(playerAction == ePlayerAction.interacting)
        {
            if (!VisualLoreCanvasManager.Instance.gameObject.activeInHierarchy)
            {
                playerAction = ePlayerAction.none;
                currentInteraction = null;
            }
        }
    }

    private void FixedUpdate()
    {
        PhysicsUpdate();
    }

    private void AnimatorUpdate()
    {
        if (ActionInGroup(playerAction, ePlayerAction.directionLocked))
        {
            float diff = currentInteraction.transform.position.x - transform.position.x;
            animator.SetBool("FacingRight", diff == 0 ? animator.GetBool("FacingRight") : diff > 0);
        }
        else
        {
            animator.SetBool("FacingRight", playerMoveVelocity.x == 0 ? animator.GetBool("FacingRight") : playerMoveVelocity.x > 0);
        }
        animator.SetInteger("MoveAction", (int)currentMove);
        if (playerAction == ePlayerAction.pushing)
        {
            bool pushing = false;
            if (Vector2.Dot(currentInteraction.transform.position - transform.position, playerMoveVelocity) > 0)
            {
                pushing = true;
            }
            animator.SetBool("Pushing", pushing);
            animator.SetBool("Pulling", !pushing);
        }
        else
        {
            animator.SetBool("Pushing", false);
            animator.SetBool("Pulling", false);
        }
    }

    private void InputUpdate()
    {
        if (!ActionInGroup(playerAction, ePlayerAction.stopsXMovement))
        {
            inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (currentMove != eInputMovementInstruction.jump && !inputDirection.Equals(Vector2.zero) && nextMove != eInputMovementInstruction.jump)
            {
                nextMove = eInputMovementInstruction.walk;
            }
        }
        else
        {
            inputDirection = Vector2.zero;
        }
        if (!ActionInGroup(playerAction, ePlayerAction.blocksJumping))
        {
            if (Input.GetButtonDown("Jump") && currentMove != eInputMovementInstruction.jump)
            {
                nextMove = eInputMovementInstruction.jump; // ~~~ buffer this for half a second and clear if nothing happens
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            nextAction = eInputActionInstruction.interact; // ~~~ buffer this for half a second and clear if nothing happens
        }
        else if (Input.GetButtonDown("SwapChar"))
        {
            nextAction = eInputActionInstruction.swap;
        }
    }

    private void ActionUpdate()
    {
        if (nextAction == eInputActionInstruction.interact)
        {
            if (!ActionInGroup(playerAction, ePlayerAction.blocksInteracting))
            {
                BaseInteractable interactable = currentInteraction;
                if (interactor.TryInteractInput(currentCharacter, ref interactable, out ePlayerAction outAction))
                {
                    currentAction = nextAction;
                    playerAction = outAction;
                    if (playerAction == ePlayerAction.waiting)
                    {
                        StartCoroutine(WaitForInteractable(interactable));
                    }
                    SaveCurrentInteractable(outAction, interactable);
                }
                else
                {
                    currentAction = eInputActionInstruction.none;
                }
            }
        }
        else if (nextAction == eInputActionInstruction.swap)
        {
            StartSwapCharacter();
        }

        nextAction = eInputActionInstruction.none;
    }

    private IEnumerator WaitForInteractable(BaseInteractable interactable)
    {
        yield return null;
        while (interactable.PlayerMustWait)
        {
            yield return null;
        }
        playerAction = interactable.GetPlayerAction;
        SaveCurrentInteractable(playerAction, interactable);
    }

    private void SaveCurrentInteractable(ePlayerAction action, BaseInteractable interactable)
    {
        if ((action & ePlayerAction.rememberInteractable) != ePlayerAction.none)
        {
            currentInteraction = interactable;
        }
        else
        {
            currentInteraction = null;
        }
    }

    private void PhysicsUpdate()
    {
        if (currentMove == eInputMovementInstruction.none && nextMove == eInputMovementInstruction.none && rigidbody.velocity.Equals(Vector2.zero))
        {
            return;
        }
        Vector2 velocity = new Vector2(playerMoveVelocity.x, rigidbody.velocity.y);
        float targetXVelocity = inputDirection.x * moveSpeed * (ActionInGroup(playerAction, ePlayerAction.slowsXAccVel) ? slowSpeedCoefficient : 1.0f);
        float velocityDiff = targetXVelocity - velocity.x;
        if (velocityDiff != 0.0f)
        {
            float acceleration = 0.0f;
            acceleration = Mathf.Sign(velocityDiff) * moveAcceleration * Time.fixedDeltaTime;
            if (ActionInGroup(playerAction, ePlayerAction.slowsXAccVel))
            {
                acceleration *= slowAccelerationCoefficient;
            }
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
            SetPlayerAction(ePlayerAction.none);
        }
        else if (groundChecker.PlayerIsJumping)
        {
            currentMove = eInputMovementInstruction.jump;
            SetPlayerAction(ePlayerAction.jumping);
        }
        else
        {
            currentMove = eInputMovementInstruction.walk;
            SetPlayerAction(ePlayerAction.walking);
        }
        nextMove = eInputMovementInstruction.none;
    }

    private void SetPlayerAction(ePlayerAction action)
    {
        if(ActionInGroup(action, ePlayerAction.isJustMovement) || action == ePlayerAction.none)
        {
            if (ActionInGroup(playerAction, ePlayerAction.isJustMovement) || playerAction == ePlayerAction.none)
            {
                playerAction = action;
            }
        }
        else
        {
            playerAction = action;
        }
    }

    private bool StartSwapCharacter()
    {
        bool outBool = !ActionInGroup(playerAction, ePlayerAction.blocksSwapping) && 
            !(currentCharacter == eInteractionRequirement.journalist && !millieSwapChecker.CanSwapToMillie);
        if (outBool)
        {
            playerAction = ePlayerAction.swapping;
            animator.SetTrigger("SwapChar");
        }
        return outBool;
    }

    private void FinaliseSwapCharacter()
    {
        if (currentCharacter == eInteractionRequirement.journalist)
        {
            currentCharacter = eInteractionRequirement.porter;
        }
        else
        {
            currentCharacter = eInteractionRequirement.journalist;
        }
        SetAnimatorCharacter();
        playerAction = ePlayerAction.none;
    }

    private void SetAnimatorCharacter()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Carson"), currentCharacter == eInteractionRequirement.journalist ? 1.0f : 0.0f);
    }

    public void FanimSwapCharacter()
    {
        FinaliseSwapCharacter();
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

    private bool ActionInGroup(ePlayerAction action, ePlayerAction group)
    {
        return (action & group) != ePlayerAction.none;
    }

    public bool IsJumping
    {
        get
        {
            return groundChecker.PlayerIsJumping;
        }
    }
}
