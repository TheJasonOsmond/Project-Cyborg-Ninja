using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject aim_indicator;
    [SerializeField] TrailRenderer _tr;
    Rigidbody2D _rb;
    Camera _mainCamera;

    //Player Fields
    string _playerName = "OZ3N";
    int _currentHealth = 100;
    int _playerMaxHealth = 100;
    int _playerLevel = 0;
    float _invulnerableLength = 1f;

    //Mouse Fields
    Vector2 _mousePos;
    Vector2 _mouseOffset;
    protected float aimAngle;



    //Event Fields
    protected bool isInvulnerable = false;
    protected bool isAiming = false;
    protected bool inAnimation = false;


    //Movement Fields
    float _inputVertical;
    float _inputHorizontal;
    protected float baseMoveSpeed = 3f;
    protected float _moveSpeed = 3f;
    float _moveSpeedLimiter = 0.7f;

    Vector2 _dashVelocity;
    Vector2 _moveVelocity;
    bool _moveDisabled = false;
    bool _facingRight = true;

    //Dash Fields
    bool _canDash = true;
    bool _isdashing = false;
    float _dashSpeed = 14f;
    float _dashLength = 0.2f;
    float _dashCooldown = 0.5f;
    //int _dashCount = 2;

    //Attack Fields
    protected bool isAttacking = false;
    protected float attackMoveSpeedMod = 0.3f;
    
    //Animations and states
    protected Animator animator;
    string _currentState;
    const string PLAYER_IDLE = "Player_idle";
    const string PLAYER_WALK_RIGHT = "Player_walk_right";
    const string PLAYER_DASH = "Player_dash";
    const string PLAYER_AIM = "Player_aim"; //REPLACE ANIMATION

    protected Vector2 MouseOffset
    {
        get
        {
            return _mouseOffset;
        }
    }

    public bool MoveDisabled
    {
        get
        {
            return _moveDisabled;
        }

        set
        {
            _moveDisabled = value;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        _mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        //Following will only be run when player is not currently in a restrictive animation
        if (inAnimation)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isAiming = true;
            MoveDisabled = true;
            aim_indicator.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            //Case where Shift is held during other animation
            //Player will start aiming when animation finishes
            if (!isAiming) 
            {
                isAiming = true;
                MoveDisabled = true;
                aim_indicator.SetActive(true);
            }
            //Press MOUSE1 to Shoot
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Fire());
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isAiming)
        {
            isAiming = false;
            MoveDisabled = false;
            aim_indicator.SetActive(false);
            animator.SetBool("is_aiming", false);        

        }

        else if (!_moveDisabled) { 
            _inputHorizontal = Input.GetAxisRaw("Horizontal");
            _inputVertical = Input.GetAxisRaw("Vertical");

            _moveVelocity = new Vector2(_inputHorizontal, _inputVertical) * _moveSpeed;

            // Press SPACE to Dash
            if (Input.GetKeyDown(KeyCode.Space) && _canDash)
            {
                StartCoroutine(Dash());
            }

            // Press MOUSE 1 to Attack
            else if (Input.GetMouseButtonDown(0))
            {
                if (!isAttacking)
                StartCoroutine(Attack());
            }

        }
    }


    private void FixedUpdate()
    {
        if (inAnimation)
            return;
        else if (isAiming)
        {
            _rb.velocity = new Vector2(0,0); //Stop Movement
            //_animator.SetBool("is_walking", false);
            animator.SetBool("is_aiming", true);
            RotateAimIndicator();
        }
        else
        {
            MovePlayer();
        }

        if (!inAnimation && !isAttacking) //Blocks flipping during Animations and Attacks
        {
            if (_inputHorizontal > 0 && !_facingRight)
            {
                FlipHorizontal();
            }
            else if (_inputHorizontal < 0 && _facingRight)
            {
                FlipHorizontal();
            }
        }
     
    }

    void MovePlayer()
    {
        // If vertical or horizontal and not disabled
        if ((_inputHorizontal != 0 || _inputVertical != 0) && !_moveDisabled)
        {
            //if diagonal movement, reduce speed
            if (_inputHorizontal != 0 && _inputVertical != 0)
            {
                _moveVelocity *= _moveSpeedLimiter;
            }

            if (isAttacking)
            {
                _moveVelocity *= attackMoveSpeedMod;
            }
            //Physics
            _rb.velocity = _moveVelocity;

            //Animation
            /* TO ADD DIRCTIONAL MOVEMENT, ADD ELSE IF STATEMENTS HERE:
             * 
             * if (inputHorizontal > 0) ChangeAnimationState(PLAYER_WALK_RIGHT);
             * else if (inputHorizontal < 0) ChangeAnimationState(PLAYER_WALK_LEFT);
             * else if (inputvertical > 0) ChangeAnimationState(PLAYER_WALK_UP);
             * else if (inputHorizontal < 0) ChangeAnimationState(PLAYER_WALK_DOWN
            */


            animator.SetBool("is_walking", true);
            //ChangeAnimationState(PLAYER_WALK_RIGHT);
        }
        // Don't move
        else
        {
            //Physics
            _moveVelocity = new Vector2(0, 0);
            _rb.velocity = _moveVelocity;

            //Animation
            animator.SetBool("is_walking", false);
        }
    }

    void FlipHorizontal()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }

    // UPDATE: Add multiple dashes
    IEnumerator Dash() 
    {
        _canDash = false;
        _isdashing = true;
        _moveDisabled = true;
        isInvulnerable = true;
        inAnimation = true;
        _tr.emitting = true;

        // If idle, dash forward
        if (_inputHorizontal == 0 && _inputVertical == 0)
        {
            if (_facingRight) _inputHorizontal = 1;
            
            else _inputHorizontal = -1;
            
        }

        _dashVelocity = new Vector2(_inputHorizontal, _inputVertical) * _dashSpeed;

        //if diagonal movement, reduce speed
        if (_inputHorizontal != 0 && _inputVertical != 0)
        {
            _dashVelocity *= _moveSpeedLimiter;
        }

        //Physics
        _rb.velocity = _dashVelocity;

        //Animation
        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(_dashLength);

        _isdashing = false;
        _moveDisabled = false;
        inAnimation = false;
        isInvulnerable = false;
        _tr.emitting = false;

        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
    
    void RotateAimIndicator()
    {
        _mousePos = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);
        _mouseOffset = new Vector2(_mousePos.x - screenPoint.x, _mousePos.y - screenPoint.y).normalized;

        aimAngle = Mathf.Atan2(_mouseOffset.y, _mouseOffset.x) * Mathf.Rad2Deg; //Get angle between offsets in degrees
        //y axis first, otherwise it will mirror its rotation

        aim_indicator.transform.rotation = Quaternion.Euler(0f, 0f, aimAngle - 90f); //-90, fixes angle
        //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); //-90, fixes angle
       
    }

    public void takeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        animator.SetTrigger("Hurt");
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            //Death();
        }
        
        else StartCoroutine(InvulnerabiltyPeriod());

        Debug.Log("Player Health: " + _currentHealth);
    }

    IEnumerator InvulnerabiltyPeriod()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(_invulnerableLength);
        isInvulnerable = false;
    }

    protected abstract IEnumerator Fire();

    protected abstract IEnumerator Attack();

    //Take Damage


}
