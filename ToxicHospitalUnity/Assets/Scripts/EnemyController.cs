using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public enum eEnemyState
    {
        patrolling,
        chasing,
        attacking
    }

    public eEnemyState EnemyState;

    void Start()
    {

    }


    void Update()
    {
        DetectEdges();
    }

    private void DetectEdges()
    {
        float castLength = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castLength);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.down * castLength, Color.green);
            Debug.Log("floor");
        }
        else
        {
            //keep walking 
            Debug.DrawRay(transform.position, Vector2.down * castLength, Color.red);
            Debug.Log("No floor");
        }
    }
}
