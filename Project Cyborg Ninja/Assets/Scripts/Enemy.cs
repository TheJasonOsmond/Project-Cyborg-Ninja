using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected GameManager gameManager;
    protected Animator animator;
    protected GameObject player;
    protected Rigidbody2D rb;
    public HealthBar healthbar;

    protected bool disableEnemy = false;

    //Stats
    float _maxHealth = 100;
    float _currentHealth;


    //Effects And Statuses
    bool Stunned = false;
    protected bool isInvulnerable = false;


    protected float currentHealth
    {
        get
        {
            return _currentHealth;

        }

        set
        {
            _currentHealth = value;
            if (_currentHealth < 0)
                _currentHealth = 0;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        _currentHealth = _maxHealth;
        healthbar.SetMaxHealth(_maxHealth);
    }

    public void takeDamage(float damage)
    {
        if (isInvulnerable)
            return;

        animator.SetTrigger("Hurt");
        _currentHealth -= damage;
        healthbar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            //Death();
            StartCoroutine(Deathtest());
        }
    }


    void Death()
    {
        // Disable Enemy
        disableEnemy = true;
        // Die Animation
        animator.SetTrigger("Death");


    }
    IEnumerator Deathtest()
    {
        disableEnemy = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        animator.SetTrigger("Death");

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        float speed = animator.GetCurrentAnimatorStateInfo(0).speed;
        yield return new WaitForSeconds(length * speed);

        Destroy(gameObject);

        //Do something when the animation is complete
    }



    //public void Stunned (float duration)
    //{

    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player Bullet")
        {
            takeDamage(player.GetComponent<PlayerActions>().BulletDamage) ;

            Destroy(collision.gameObject);
        }

    }

    protected abstract IEnumerator Attack();
}
