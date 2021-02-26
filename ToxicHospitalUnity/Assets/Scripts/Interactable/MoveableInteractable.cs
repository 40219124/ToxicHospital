using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableInteractable : BaseInteractable
{
    protected bool touchingPlayer = false;
    protected PlayerController player;
    protected Collider2D playerCollider;
    protected Rigidbody2D rigidbody;
    protected Collider2D thisCollider;
    protected GroundChecker groundChecker;

    protected enum eMoveableState { none, airborne, locked, toPlayer, moveByPlayer }
    protected eMoveableState moveableState = eMoveableState.none;
    protected eMoveableState MoveableState
    {
        get { return moveableState; }
        set
        {
            if (MoveableState != value)
            {
                SetBodyLock(value, MoveableState);
                moveableState = value;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        player = FindObjectOfType<PlayerController>(); // ~~~ could be better

        MoveableState = eMoveableState.airborne;

        GetNonTriggerCollider(player.transform, ref playerCollider);
        GetNonTriggerCollider(transform, ref thisCollider);

        Physics2D.IgnoreCollision(thisCollider, groundChecker.GetComponent<Collider2D>());
    }

    protected void GetNonTriggerCollider(Transform transform, ref Collider2D collider)
    {
        Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();
        foreach(Collider2D c in colliders)
        {
            if (!c.isTrigger)
            {
                collider = c;
                break;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (groundChecker.PlayerIsJumping && MoveableState == eMoveableState.locked)
        {
            MoveableState = eMoveableState.airborne;
        }
        else if (groundChecker.PlayerCanJump && rigidbody.velocity.y == 0.0f && MoveableState == eMoveableState.airborne)
        {
            MoveableState = eMoveableState.locked;
        }
    }

    public override bool CanInteract(eInteractionRequirement triggerType)
    {
        return base.CanInteract(triggerType) && !player.IsJumping;
    }

    protected void SetBodyLock(eMoveableState state, eMoveableState previousState)
    {
        switch (state)
        {
            case eMoveableState.locked:
                rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                break;
            case eMoveableState.airborne:
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                break;
            case eMoveableState.toPlayer:
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
            case eMoveableState.moveByPlayer:
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.SetParent(player.transform);
                thisCollider.transform.SetParent(player.transform);
                rigidbody.isKinematic = true;
                break;
            default:
                rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                break;
        }
        if (previousState == eMoveableState.moveByPlayer && state != eMoveableState.moveByPlayer)
        {
            transform.SetParent(null);
            thisCollider.transform.SetParent(transform);
            thisCollider.transform.localPosition = Vector2.zero;
            rigidbody.isKinematic = false;
            Physics2D.IgnoreCollision(playerCollider, thisCollider, false);
        }
    }

    protected override IEnumerator WaitBeforeInteraction(eActivateInteraction waitType, Timer timer = null)
    {
        if (MoveableState == eMoveableState.toPlayer || MoveableState == eMoveableState.moveByPlayer)
        {
            yield break;
        }
        else
        {
            if (!touchingPlayer)
            {
                float drag = rigidbody.drag;
                rigidbody.drag = 0.0f;
                MoveableState = eMoveableState.toPlayer;
                rigidbody.velocity = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x), 0.0f) * 5.0f;

                while (!touchingPlayer && MoveableState == eMoveableState.toPlayer)
                {
                    yield return null;
                }
                rigidbody.velocity = Vector2.zero;
                rigidbody.drag = drag;
            }
        }
    }

    protected override void DoInteractionAction()
    {
        if (MoveableState == eMoveableState.moveByPlayer)
        {
            MoveableState = eMoveableState.airborne;
        }
        else
        {
            MoveableState = eMoveableState.moveByPlayer;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            touchingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
}
