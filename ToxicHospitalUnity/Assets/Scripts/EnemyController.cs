using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public enum eEnemyState
    {
        patrolling,
        chasing
    }

    public eEnemyState EnemyState;
    public LayerMask IgnoreLayer;


    [SerializeField] private float patrolRadius = 2;
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float chaseSpeed = 5;
    [SerializeField] private float visionRange = 15;
    [SerializeField] private float attackCooldown = 1;
    [SerializeField] private Transform castPointContainer;
    public bool FacingRight;

    private Vector3 patrolPlacement;
    private float currentSpeed;
    private float cooldownCurrent;

    private SpriteRenderer enemyGraphic;
    private Transform player;
    private InfectionTracker playerHealthEffects;





    private bool DetectEdges()
    {
        float castLength = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castLength, ~IgnoreLayer);
        if (hit.collider == null)
        {
            //edge found
            Debug.DrawRay(transform.position, Vector2.down * castLength, Color.red);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.down * castLength, Color.green);
            //Debug.Log("solid floor");
            return false;
        }
    }

    private bool DetectWalls()
    {
        bool wallHit = false;
        float castLength = 0.8f;
        int directionMultiplier = FacingRight ? 1 : -1;
        Vector2 castDirection = Vector2.right * directionMultiplier;
        print("walls cast direction: " + castDirection);


        foreach (Transform t in castPointContainer)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, castDirection, castLength, ~IgnoreLayer);
            //if raycast hits AND (hit is not a trigger OR hit has the obstruction tag)
            if (hit.collider != null && (!hit.collider.isTrigger || hit.collider.CompareTag("Obstruction")))
            {
                //wall found
                Debug.DrawRay(t.position, castDirection * castLength, Color.red);
                Debug.Log(hit.collider.name);
                wallHit = true;

            }
            else
            {
                Debug.DrawRay(t.position, castDirection * castLength, Color.blue);
            }
        }


        return wallHit;
    }

    private void Move(float speed, bool direction)
    {
        //if direction is true walk right else walk left
        int directionMultiplier = direction ? 1 : -1;
        Vector3 move = Vector3.right * speed * directionMultiplier;
        gameObject.transform.position += move * Time.deltaTime;

        //scale the sprite to face the right direction
        float xScale = enemyGraphic.transform.localScale.x;
        xScale = directionMultiplier;
        enemyGraphic.transform.localScale = new Vector3(xScale, enemyGraphic.transform.localScale.y, enemyGraphic.transform.localScale.z);

        //scale the casters to the correct side

        //xScale = castPointContainer.localScale.x;
        //xScale = directionMultiplier;
        //enemyGraphic.transform.localScale = new Vector3(xScale, enemyGraphic.transform.localScale.y, enemyGraphic.transform.localScale.z);
    }

    private bool GetTargetDirection(Vector3 target)
    {
        if (target.x > gameObject.transform.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ScanForPlayer()
    {
        bool playerFound = false;
        //if facing in direction of player
        if ((FacingRight && player.position.x > gameObject.transform.position.x) || (!FacingRight && player.position.x < gameObject.transform.position.x))
        {
            foreach (Transform t in castPointContainer)
            {
                Vector2 rayDirection = Vector3.Normalize(player.position - t.position);

                RaycastHit2D hit = Physics2D.Raycast(t.position, rayDirection, visionRange, ~IgnoreLayer);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    //player found
                    Debug.DrawRay(t.position, rayDirection * visionRange, Color.green);
                    playerFound = true;
                }
                else
                {
                    Debug.DrawRay(t.position, rayDirection * visionRange, Color.red);
                }
            }
        }
        return playerFound;
    }

    private bool Attacking()
    {
        bool playerHit = false;
        float castLength = 1.0f;
        int directionMultiplier = FacingRight ? 1 : -1;
        Vector2 castDirection = Vector2.right * directionMultiplier;
        print("Attack cast direction: " + castDirection);


        foreach (Transform t in castPointContainer)
        {
            RaycastHit2D hit = Physics2D.Raycast(t.position, castDirection, castLength, ~IgnoreLayer);
            //if raycast hits AND it hit something tagged player
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                //wall found
                Debug.DrawRay(t.position, castDirection * castLength, Color.red);
                Debug.Log(hit.collider.name);
                playerHit = true;

            }
            else
            {
                //Debug.DrawRay(t.position, castDirection * castLength, Color.blue);
            }
        }

        Debug.Log("Playerhit : " + playerHit);
        return playerHit;
    }



    void Start()
    {
        enemyGraphic = GetComponentInChildren<SpriteRenderer>();
        patrolPlacement = gameObject.transform.position;


        currentSpeed = patrolSpeed;
        cooldownCurrent = attackCooldown;


        //give initial movement
        //Move(patrolSpeed, facingRight);
        player = GameObject.Find("PlayerCentreOfMass").transform;
        playerHealthEffects = GameObject.FindObjectOfType<InfectionTracker>();
    }


    void Update()
    {
        //always check for edges and turn aropund if needed
        //TODO or DetectedWall
        if (DetectEdges() || DetectWalls())
        {
            Debug.Log(DetectEdges() + ", " + DetectWalls());
            FacingRight = !FacingRight;
            Move(currentSpeed, FacingRight);

        }

        //if player in view, chase
        if (ScanForPlayer())
        {
            EnemyState = eEnemyState.chasing;
        }
        else
        {
            EnemyState = eEnemyState.patrolling;
        }

        //patrolling back and forth within radius of spawn, turn around if out or radius or at an edge/wall unless player detected
        if (EnemyState == eEnemyState.patrolling)
        {
            currentSpeed = patrolSpeed;

            Debug.Log("patrolling");
            float distanceFromPlacementPoint = Mathf.Abs((patrolPlacement - gameObject.transform.position).x);
            //if (we face right and placement is left OR we face left and placement position is right) AND distance to placement position greater than patrol area radius
            if (((FacingRight && patrolPlacement.x < gameObject.transform.position.x)
            || (!FacingRight && patrolPlacement.x > gameObject.transform.position.x))
            && distanceFromPlacementPoint > patrolRadius)
            {
                FacingRight = !FacingRight;
            }

            Move(currentSpeed, FacingRight);
        }
        else if (EnemyState == eEnemyState.chasing)
        {
            currentSpeed = chaseSpeed;
            Debug.Log("chasing");
            Move(currentSpeed, FacingRight);
        }
        else
        {
            Debug.LogWarning("Something wrong in state machine.");
        }


        //attack on cooldown
        cooldownCurrent -= Time.deltaTime;
        if (cooldownCurrent <= 0 && Attacking())
        {
            playerHealthEffects.InfectionProgress += 20;
            playerHealthEffects.currentHealth -= 20;
            cooldownCurrent = attackCooldown;
        }


    }

}
