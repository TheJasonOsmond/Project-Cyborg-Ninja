using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles Player Input
public class PlayerControllerNew : MonoBehaviour
{
    public Animator animator;
    
    Player player;
    Rigidbody2D rb;
    Camera mainCamera;

    [SerializeField] GameObject aimIndicator;
    [SerializeField] Transform enemyGFX;

    //Public State Fields
    public bool allowInput = true;
    public bool canMove = true;
    public bool canDash = true;
    public bool canAttack = true;
    public bool isAttacking = false;
    public bool isAiming = false;

    //Private State Fields
    bool facingRight;
    Vector2 moveVelocity;
    float moveSpeedLimiter = 0.7f;

    //Keyboard Input Fields
    float inputVertical;
    float inputHorizontal;

    //Mouse Input Fields
    Vector2 _mousePos;
    Vector2 _mouseOffset;
    protected float aimAngle;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (!allowInput || PauseControl.isPaused)
            return;

        #region Aiming Controls
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isAiming = true;
            canMove = false;
            aimIndicator.SetActive(true);
        }

        else if (Input.GetKey(KeyCode.LeftShift))
        {
            //Case where Shift is held during other animation
            //Player will start aiming when animation finishes
            if (!isAiming)
            {
                isAiming = true;
                canMove = false;
                aimIndicator.SetActive(true);
            }

            //Press MOUSE1 to Shoot
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Pew!");
                //StartCoroutine(player.Fire());
            }
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift) && isAiming)
        {
            isAiming = false;
            canMove = true;
            aimIndicator.SetActive(false);
            animator.SetBool("isAiming", false);
        }
        #endregion

        if (canMove)
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");

            moveVelocity = new Vector2(inputHorizontal, inputVertical) * player.moveSpeed;
        }



    }

    void FixedUpdate()
    {
        if (isAiming)
        {
            moveVelocity = new Vector2(0f, 0f);

            AnimateMovement(moveVelocity);
            rb.velocity = moveVelocity;

            Aim();
        }
        
        else if (canMove)
            MovePlayer();
    }


    void MovePlayer()
    {
        // If vertical or horizontal and not disabled
        if ((inputHorizontal != 0 || inputVertical != 0))
        {
            //if diagonal movement, reduce speed
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                moveVelocity *= moveSpeedLimiter;
            }

            if (isAttacking)
            {
                moveVelocity *= player.attackMoveSpeedMod;
            }

            //Physics
            rb.velocity = moveVelocity;

            //Animate Movement
            //if (inputHorizontal > 0 || inputHorizontal < 0)
            //    animator.SetInteger("facingDirection", 1);

            //else if (inputVertical < 0)
            //    animator.SetInteger("facingDirection", 0);

            //else if (inputVertical > 0)
            //    animator.SetInteger("facingDirection", 2);


            //ChangeAnimationState(PLAYER_WALK_RIGHT);
        }

        // Don't move
        else
        {
            moveVelocity = new Vector2(0, 0);
            rb.velocity = moveVelocity;
        }

        AnimateMovement(rb.velocity);
    }

    void Aim()
    {
        _mousePos = Input.mousePosition;
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);
        _mouseOffset = new Vector2(_mousePos.x - screenPoint.x, _mousePos.y - screenPoint.y).normalized;

        //Get angle between offsets in degrees. y axis first, otherwise it will mirror its rotation
        aimAngle = Mathf.Atan2(_mouseOffset.y, _mouseOffset.x) * Mathf.Rad2Deg; 

        aimIndicator.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle - 90f); //-90, fixes angle
        //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); //-90, fixes angle

    }


    void AnimateMovement(Vector2 moveDirection)
    {
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