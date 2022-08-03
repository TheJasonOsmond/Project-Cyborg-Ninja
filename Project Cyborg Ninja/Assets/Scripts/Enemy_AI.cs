using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_AI : MonoBehaviour
{
    //Public Fields
    public float moveSpeed = 200f;
    public bool stopMovement = false;
    public Vector2 direction;

    //Private Fields
    [SerializeField] Transform target;
    [SerializeField] Transform enemyGFX;
    [SerializeField] Animator animator;
    [SerializeField] float nextWaypointDistance = 0.1f;

    Seeker seeker;
    Path path;
    Rigidbody2D rb;

    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool facingRight;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.2f); //Update path every 0.5 seconds
        
    }
    void UpdatePath()
    {
        if (stopMovement)
            return;

        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);

        if (direction != null)
            AnimateMovement(direction);
    }
    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; //When current waypoint is zero then end of path
        }
    }

    void FixedUpdate()
    {
        if (path == null || stopMovement)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            AnimateMovement(new Vector2(0f, 0f));
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        Debug.Log("X: " + direction.x + " | Y: " + direction.y);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    void AnimateMovement(Vector2 moveDirection)
    {
        //if (((Mathf.Abs(moveDirection.x) > 0.4f && (Mathf.Abs(rb.velocity.x) > 0.01f)) ||
        //    (Mathf.Abs(moveDirection.x) > 0f && (Mathf.Abs(moveDirection.y) < 0.5f) && (Mathf.Abs(rb.velocity.y) > 0.01f)))
        //    && (Mathf.Abs(rb.velocity.x) > 0.01f))
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            animator.SetBool("isWalking", true);
            animator.SetInteger("facingDirection", 1);

            //Flip if needed
            if (rb.velocity.x >= 0.01f && moveDirection.x > 0f && !facingRight)
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
                facingRight = true;
            }
            else if (rb.velocity.x <= -0.01 && moveDirection.x < 0f && facingRight)
            {
                Debug.Log(animator.GetBool("isWalking"));
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
                facingRight = false;
            }
        }
        else if (Mathf.Abs(moveDirection.y) > 0f && (Mathf.Abs(rb.velocity.y) > 0.01f))
        {
            //Up and Down Animation
            animator.SetBool("isWalking", true);
            if (moveDirection.y > 0f)
                animator.SetInteger("facingDirection", 2);
            else
                animator.SetInteger("facingDirection", 0);

        }
        else if ((moveDirection.x == 0f && moveDirection.y == 0f) && ((Mathf.Abs(rb.velocity.x) < 0.01f) && (Mathf.Abs(rb.velocity.y) < 0.01f)))
        {
                //No Movement
                animator.SetBool("isWalking", false);
                Debug.Log("No Movement");

        }

    }
}
