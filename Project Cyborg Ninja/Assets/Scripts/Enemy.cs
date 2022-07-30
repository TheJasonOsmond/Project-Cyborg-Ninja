using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameManager gameManager;
    protected Animator animator;
    protected GameObject player;
    protected Rigidbody2D rb;

    protected bool disableEnemy = false;

    //Stats
    int _maxHealth = 100;
    int _currentHealth;

    //Effects And Statuses
    bool Stunned = false;
    protected bool isInvulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        _currentHealth = _maxHealth;
    }

    public void takeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        animator.SetTrigger("Hurt");
        _currentHealth -= damage;
        Debug.Log(this.name + ": -" + damage + " Health");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            //Death();
            StartCoroutine(Deathtest());
        }
    }
    void Death()
    {

        // Die Animation
        animator.SetTrigger("Death");

        // Disable Enemy
        disableEnemy = true;


    }
    IEnumerator Deathtest()
    {
        string deathAni = "Light Bandit_Death";
        animator.SetTrigger("Death");

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        float speed = animator.GetCurrentAnimatorStateInfo(0).length;
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
}
