using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    Rigidbody2D _rb;
    Camera _mainCamera;

    //Player Fields
    string _playerName = "OZ3N";
    int _playerHealth = 100;
    int _playerMaxHealth = 100;
    int _playerLevel = 0;

    //Mouse Fields
    Vector2 _mousePos;
    Vector2 _mouseOffset;
    [SerializeField] GameObject aim_indicator;


    //Event Fields
    protected bool isInvulnerable = false;
    protected bool isAttacking = false;
    protected bool isAiming = false;
    protected bool isShooting = false;
    protected bool inAnimation = false;
    protected bool isdashing = false;

    //Movement Fields
    float _inputVertical;
    float _inputHorizontal;
    protected float baseMoveSpeed = 3f;
    protected float _moveSpeed = 3f;
    
    float _dashSpeed = 10f;
    float _moveSpeedLimiter = 0.7f;
    Vector2 _dashVelocity;
    Vector2 _moveVelocity;
    bool _moveDisabled = false;
    bool _facingRight = true;
    
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
        if (!inAnimation)
        {
            // Hold LEFTSHIFT to Aim
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isAiming = true;
                MoveDisabled = true;
                aim_indicator.SetActive(true);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                isAiming = true;
                MoveDisabled = true;
                if (Input.GetMouseButtonDown(0))
                {
                    isShooting = true;
                }

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isAiming = false;
                MoveDisabled = false;
                aim_indicator.SetActive(false);
                animator.SetBool("is_aiming", false);          //Ideally keep animations out of update

            }


            else if (!_moveDisabled) { 
                _inputHorizontal = Input.GetAxisRaw("Horizontal");
                _inputVertical = Input.GetAxisRaw("Vertical");

                _moveVelocity = new Vector2(_inputHorizontal, _inputVertical) * _moveSpeed;

                // Press SPACE to Dash
                if (Input.GetKeyDown(KeyCode.Space))
                {
                   isdashing = true;
                   //inAnimation = true;
                }

                // Press MOUSE 1 to Attack
                if (Input.GetMouseButtonDown(0))
                {
                    isAttacking = true;
                    
                }

            }
        }


    }


    private void FixedUpdate()
    {
        
        if (isdashing)
        {
            StartCoroutine(Dash());
        }
        else if (isAiming)
        {
            _rb.velocity = new Vector2(0,0); //Stop Movement
            //_animator.SetBool("is_walking", false);
            animator.SetBool("is_aiming", true);
            //ChangeAnimationState(PLAYER_AIM);
            RotateAimIndicator();
            if (isShooting)
            {
                StartCoroutine(Fire());
            }
        }
        else if (isAttacking)
        {
            Attack();
        }
        else
        {
            MovePlayer();
        }

        if (_inputHorizontal > 0 && !_facingRight)
        {
            FlipHorizontal();
        }
        else if (_inputHorizontal < 0 && _facingRight)
        {
            FlipHorizontal();
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
            //ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void FlipHorizontal()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }

    /*protected void ChangeAnimationState(string newState)
    {
        // Stop Animation from interupting self
        if (_currentState == newState || inAnimation) return;

        //Play new animation
        animator.Play(newState);
        
        //update currentstate
        _currentState = newState;
    
    }
    */

    IEnumerator Dash()
    {
        // UPDATE: Make invulnerable/ I-Frames

        _moveDisabled = true;

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
        //ChangeAnimationState(PLAYER_DASH);
        //_animator.Settrigger("is_dashing", true);
        inAnimation = true;

        float dashlength = 0.1f;
        yield return new WaitForSeconds(dashlength);

        _moveDisabled = false;
        isdashing = false;
        inAnimation = false;
        
    }

    void RotateAimIndicator()
    {
        _mousePos = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);
        _mouseOffset = new Vector2(_mousePos.x - screenPoint.x, _mousePos.y - screenPoint.y).normalized;

        float angle = Mathf.Atan2(_mouseOffset.y, _mouseOffset.x) * Mathf.Rad2Deg; //Get angle between offsets in degrees
        //y axis first, otherwise it will mirror its rotation

        aim_indicator.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); //-90, fixes angle
        //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); //-90, fixes angle
       
    }

    protected abstract IEnumerator Fire();

    protected abstract void Attack();
    //Take Damage


}
