using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_AI : MonoBehaviour
{

    public Transform target;
    public Transform enemyGFX;

    [SerializeField] protected Animator animator;

    public float moveSpeed = 200f; 
    [SerializeField] float nextWaypointDistance = 0.1f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    bool facingRight;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f); //Update path every 0.5 seconds
        
    }
    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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
        if (path == null)
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

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);
        AnimateMovement(direction);

        Debug.Log("X: " + direction.x + " | Y: " + direction.y);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    void AnimateMovement(Vector2 moveDirection)
    {
        if (((Mathf.Abs(moveDirection.x) > 0.4f && (Mathf.Abs(rb.velocity.x) > 0.01f)) ||
            (Mathf.Abs(moveDirection.x) > 0f && (Mathf.Abs(moveDirection.y) < 0.2f) && (Mathf.Abs(rb.velocity.y) > 0.01f)))
            && (Mathf.Abs(rb.velocity.x) > 0.01f))
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
            //
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
