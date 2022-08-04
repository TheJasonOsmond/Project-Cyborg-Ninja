using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //    [SerializeField] protected GameManager gameManager;
    //    [SerializeField] GameObject aim_indicator;
    //    [SerializeField] TrailRenderer _tr;
    //    Rigidbody2D _rb;
    //    Camera _mainCamera;

    //    //Mouse Fields
    //    Vector2 _mousePos;
    //    Vector2 _mouseOffset;
    //    protected float aimAngle;


    //    //Event Fields
    //    protected bool isAiming = false;
    //    protected bool inAnimation = false;
    //    private bool _isDead = false;
    //    protected bool isPaused;


    //    //Movement Fields
    //    float _inputVertical;
    //    float _inputHorizontal;
    //    protected float baseMoveSpeed = 3f;
    //    protected float _moveSpeed = 3f;
    //    float _moveSpeedLimiter = 0.7f;

    //    Vector2 _dashVelocity;
    //    Vector2 _moveVelocity;
    //    bool _moveDisabled = false;
    //    bool _facingRight = true;

    //    //Dash Fields
    //    bool _canDash = true;
    //    bool _isdashing = false;


    //    //Attack Fields
    //    protected bool isAttacking = false;


    //    //Animations and states
    //    [SerializeField] protected Animator animator;

    //    protected Vector2 MouseOffset
    //    {
    //        get
    //        {
    //            return _mouseOffset;
    //        }
    //    }

    //    public bool MoveDisabled
    //    {
    //        get
    //        {
    //            return _moveDisabled;
    //        }

    //        set
    //        {
    //            _moveDisabled = value;
    //        }

    //    }

    //    protected bool IsDead
    //    {
    //        get
    //        {
    //            return _isDead;
    //        }

    //        set
    //        {
    //            _isDead = true;
    //            isInvulnerable = true;
    //            MoveDisabled = true;
    //            inAnimation = true; //TEMP, Add Disabled Field 
    //        }
    //    }


    //    // Start is called before the first frame update
    //    void Start()
    //    {

    //        _rb = gameObject.GetComponent<Rigidbody2D>();
    //        //animator = gameObject.GetComponent<Animator>();
    //        _mainCamera = Camera.main;

    //        _currentHealth = _maxHealth;
    //        healthbar.SetMaxHealth(_maxHealth);

    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        //Following will only be run when player is not currently in a restrictive animation
    //        if (inAnimation || PauseControl.isPaused)
    //        {
    //            return;
    //        }

    //        if (Input.GetKeyDown(KeyCode.LeftShift))
    //        {
    //            isAiming = true;
    //            MoveDisabled = true;
    //            aim_indicator.SetActive(true);
    //        }
    //        else if (Input.GetKey(KeyCode.LeftShift))
    //        {
    //            //Case where Shift is held during other animation
    //            //Player will start aiming when animation finishes
    //            if (!isAiming) 
    //            {
    //                isAiming = true;
    //                MoveDisabled = true;
    //                aim_indicator.SetActive(true);
    //            }
    //            //Press MOUSE1 to Shoot
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                StartCoroutine(Fire());
    //            }
    //        }
    //        else if (Input.GetKeyUp(KeyCode.LeftShift) && isAiming)
    //        {
    //            isAiming = false;
    //            MoveDisabled = false;
    //            aim_indicator.SetActive(false);
    //            animator.SetBool("isAiming", false);        

    //        }

    //        else if (!_moveDisabled) { 
    //            _inputHorizontal = Input.GetAxisRaw("Horizontal");
    //            _inputVertical = Input.GetAxisRaw("Vertical");

    //            _moveVelocity = new Vector2(_inputHorizontal, _inputVertical) * _moveSpeed;

    //            // Press SPACE to Dash
    //            if (Input.GetKeyDown(KeyCode.Space) && _canDash)
    //            {
    //                StartCoroutine(Dash());
    //            }

    //            // Press MOUSE 1 to Attack
    //            else if (Input.GetMouseButtonDown(0))
    //            {
    //                if (!isAttacking)
    //                StartCoroutine(Attack());
    //            }

    //        }
    //    }


    //    private void FixedUpdate()
    //    {
    //        if (inAnimation)
    //            return;
    //        else if (isAiming)
    //        {
    //            _rb.velocity = new Vector2(0,0); //Stop Movement
    //            //_animator.SetBool("is_walking", false);
    //            animator.SetBool("isAiming", true);
    //            RotateAimIndicator();
    //        }
    //        else
    //        {
    //            MovePlayer();
    //        }

    //        if (!inAnimation && !isAttacking) //Blocks flipping during Animations and Attacks
    //        {
    //            if (_inputHorizontal > 0 && !_facingRight)
    //            {
    //                FlipHorizontal();
    //            }
    //            else if (_inputHorizontal < 0 && _facingRight)
    //            {
    //                FlipHorizontal();
    //            }
    //        }

    //    }

    //    void MovePlayer()
    //    {
    //        // If vertical or horizontal and not disabled
    //        if ((_inputHorizontal != 0 || _inputVertical != 0) && !_moveDisabled)
    //        {
    //            //if diagonal movement, reduce speed
    //            if (_inputHorizontal != 0 && _inputVertical != 0)
    //            {
    //                _moveVelocity *= _moveSpeedLimiter;
    //            }

    //            if (isAttacking)
    //            {
    //                _moveVelocity *= attackMoveSpeedMod;
    //            }
    //            //Physics
    //            _rb.velocity = _moveVelocity;

    //            //Animate Movement
    //            if (_inputHorizontal > 0 || _inputHorizontal < 0)
    //                animator.SetInteger("facingDirection", 1);

    //            else if (_inputVertical < 0)
    //                animator.SetInteger("facingDirection", 0);

    //            else if (_inputVertical > 0)
    //                animator.SetInteger("facingDirection", 2);

    //            animator.SetBool("isWalking", true);
    //            //ChangeAnimationState(PLAYER_WALK_RIGHT);
    //        }
    //        // Don't move
    //        else
    //        {
    //            //Physics
    //            _moveVelocity = new Vector2(0, 0);
    //            _rb.velocity = _moveVelocity;

    //            //Animation
    //            animator.SetBool("isWalking", false);
    //        }
    //    }

    //    void FlipHorizontal()
    //    {
    //        if (animator.GetInteger("facingDirection") == 1)
    //        {
    //            Vector3 currentScale = gameObject.transform.localScale;
    //            currentScale.x *= -1;
    //            gameObject.transform.localScale = currentScale;

    //            _facingRight = !_facingRight;
    //        }

    //    }

    //    // UPDATE: Add multiple dashes
    //    IEnumerator Dash() 
    //    {
    //        _canDash = false;
    //        _isdashing = true;
    //        _moveDisabled = true;
    //        isInvulnerable = true;
    //        inAnimation = true;
    //        _tr.emitting = true;

    //        // If idle, dash forward
    //        if (_inputHorizontal == 0 && _inputVertical == 0)
    //        {
    //            if (_facingRight) _inputHorizontal = 1;

    //            else _inputHorizontal = -1;

    //        }

    //        _dashVelocity = new Vector2(_inputHorizontal, _inputVertical) * _dashSpeed;

    //        //if diagonal movement, reduce speed
    //        if (_inputHorizontal != 0 && _inputVertical != 0)
    //        {
    //            _dashVelocity *= _moveSpeedLimiter;
    //        }

    //        //Physics
    //        _rb.velocity = _dashVelocity;

    //        //Animation
    //        animator.SetTrigger("dash");

    //        yield return new WaitForSeconds(_dashLength);

    //        //_isdashing = false;
    //        _moveDisabled = false;
    //        inAnimation = false;
    //        isInvulnerable = false;
    //        _tr.emitting = false;

    //        yield return new WaitForSeconds(_dashCooldown);
    //        _canDash = true;
    //    }

    //    void RotateAimIndicator()
    //    {
    //        _mousePos = Input.mousePosition;
    //        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);
    //        _mouseOffset = new Vector2(_mousePos.x - screenPoint.x, _mousePos.y - screenPoint.y).normalized;

    //        aimAngle = Mathf.Atan2(_mouseOffset.y, _mouseOffset.x) * Mathf.Rad2Deg; //Get angle between offsets in degrees
    //        //y axis first, otherwise it will mirror its rotation

    //        aim_indicator.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle - 90f); //-90, fixes angle
    //        //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); //-90, fixes angle

    //    }

    public void takeDamage(float damage)
    {

    }
//    {
//        if (isInvulnerable)
//            return;

//        animator.SetTrigger("Hurt");
//        _currentHealth -= damage;

//        healthbar.SetHealth(_currentHealth);

//        if (_currentHealth <= 0)
//        {
//            _currentHealth = 0;
//            StartCoroutine(Death());
//        }
        
//        else StartCoroutine(InvulnerabiltyPeriod());

//        Debug.Log("Player Health: " + _currentHealth);
//    }

//    IEnumerator InvulnerabiltyPeriod()
//    {
//        isInvulnerable = true;
//        yield return new WaitForSeconds(_invulnerableLength);
//        isInvulnerable = false;
//    }


//    IEnumerator Death()
//    {
//        IsDead = true;

//        gameManager._gameOver = true;

//        //gameObject.GetComponent<BoxCollider2D>().enabled = false;

//        animator.SetTrigger("Death");

//        float length = animator.GetCurrentAnimatorStateInfo(0).length;
//        float speed = animator.GetCurrentAnimatorStateInfo(0).speed;
//        yield return new WaitForSeconds(length * speed);

//        Debug.Log("GAME OVER!");
//        _rb.velocity = new Vector2(0f, 0f);
//        //Destroy(gameObject);

//        //Do something when the animation is complete
//    }
    
}
