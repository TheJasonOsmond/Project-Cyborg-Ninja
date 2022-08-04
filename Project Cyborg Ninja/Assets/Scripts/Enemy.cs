using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Enemy_AI enemyMovement;
    [SerializeField] Collider2D col2D;
    public Animator animator; //Referencing Enemy GFX

    protected GameManager gameManager;
    protected GameObject player;
    protected Rigidbody2D rb;
    public HealthBar healthbar;
    AnimTimes animTimes;

    bool enemyDisabled = false;

    //Stats
    float _maxHealth = 100;
    float _currentHealth;


    //Effects And Statuses
    bool Stunned = false;
    protected bool isInvulnerable = false;

    public bool disableEnemy
    {
        get
        {
            return enemyDisabled;
        }
        set
        {
            enemyDisabled = value;
            enemyMovement.stopMovement = true;
        }
    }
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
        animTimes = gameObject.GetComponent<AnimTimes>();
        player = GameObject.FindGameObjectWithTag("Player");

        _currentHealth = _maxHealth;
        healthbar.SetMaxHealth(_maxHealth);
    }

    public void takeDamage(float damage)
    {
        if (isInvulnerable)
            return;

        animator.SetTrigger("damaged");
        _currentHealth -= damage;
        healthbar.SetHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        Debug.Log("Start Death");
        //Disable enemy interaction and collision
        disableEnemy = true;
        col2D.enabled = false;

        //Play Death Animation
        animator.SetTrigger("death");


        //Waits for Animation to finish
        animator.speed = 1;
        yield return new WaitForSeconds(animTimes.deathTime);

        //Delete Enemy
        Destroy(gameObject);
        Debug.Log("End Death");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player Bullet")
        {
            takeDamage(player.GetComponent<PlayerActions>().BulletDamage) ;

            Destroy(collision.gameObject);
        }
    }

    //protected abstract IEnumerator Attack();
}
