using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody2D rigidbody;

    private float timeToAirborne = 0.1f;
    private Timer airTimer;
    private bool ungrounding = false;
    private Vector2 lastGroundPos = Vector2.zero;
    private float lastGroundMaxDistSq = Mathf.Pow(0.5f, 2.0f);

    private bool grounded = false;

    private bool airborne = false; // True while jump impulse is still in effect
    private float prevYVel = 0.0f;

    private int groundCount = 0;
    private int GroundCount
    {
        get { return groundCount; }
        set
        {
            groundCount = value;
            if (GroundCount == 0)
            {
                StartCoroutine(UngroundedDelay());
            }
            else if (ungrounding)
            {
                ungrounding = false;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        rigidbody = GetComponentInParent<Rigidbody2D>();
        airTimer = TimerManager.Instance.CreateNewTimer(timeToAirborne);
        SetGrounded(false);
    }

    private void Update()
    {
        /*if (PlayerCanJump)
        {
            player.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
        else
        {
            player.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AirborneUpdate();
        if(grounded && !ungrounding)
        {
            lastGroundPos = player.transform.position;
        }
    }

    public void PlayerJumpStart(float yVel)
    {
        airborne = true;
        prevYVel = yVel + float.Epsilon;
    }

    public bool PlayerIsJumping
    {
        get { return !grounded || airborne; }
    }
    public bool PlayerCanJump
    {
        get { return grounded && !airborne; }
    }

    private void AirborneUpdate()
    {
        if (airborne)
        {
            if (!(rigidbody.velocity.y < prevYVel))
            {
                airborne = false;
            }
        }

        prevYVel = rigidbody.velocity.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            SetGrounded(true);
            GroundCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GroundCount--;
        }
    }

    private void SetGrounded(bool state)
    {
        grounded = state;
    }

    private IEnumerator UngroundedDelay()
    {
        if (ungrounding)
        {
            airTimer.Restart();
            yield break;
        }
        ungrounding = true;
        airTimer.Restart();

        while (ungrounding)
        {
            if (airTimer.IsFinished || ((Vector2)player.transform.position - lastGroundPos).sqrMagnitude > lastGroundMaxDistSq)
            {
                SetGrounded(false);
                break;
            }
            yield return null;
        }
        ungrounding = false;
    }
}
