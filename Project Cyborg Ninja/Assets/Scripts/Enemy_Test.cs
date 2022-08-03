using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : Enemy
{
    public LayerMask playersLayer;

    //Movement Fields
    Vector2 _moveDirection;
    float _enemyMoveSpeed = 2f;
    bool _facingRight = false;

    //Attack Fields
    bool _isAttacking = false;
    bool _canAttack = true;
    float _attackCooldown = 1f;
    public Transform attackPoint;
    public float attackRange;
    float attackDamage = 100f;


    private void FixedUpdate()
    {
        if (!gameManager._gameOver && !disableEnemy)
        {
            Collider2D ObjectInrange = Physics2D.OverlapCircle(attackPoint.position, attackRange, playersLayer);
            bool playerInRange = ObjectInrange != null && ObjectInrange.gameObject.Equals(player);

            if (!_isAttacking && !playerInRange)
            {
                MoveEnemy();
            }

            else if (_canAttack && playerInRange && !disableEnemy)
            {
                //Debug.Log("Player in range!");
                StartCoroutine(Attack());
                //Debug.Log("Got 'em");
            }
            else if (!_canAttack && playerInRange)
            {
                animator.SetInteger("AnimState", 1);
                //rb.velocity = new Vector2(0f, 0f);
            }
        }
    }

    void MoveEnemy()
    {
        animator.SetInteger("AnimState", 2);
        //transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _enemyMoveSpeed * Time.deltaTime);
        //velocity = new Vector2(1.75f, 1.1f);
        _moveDirection = player.transform.position - transform.position;
        _moveDirection.Normalize();
        rb.MovePosition(rb.position + _moveDirection * _enemyMoveSpeed * Time.fixedDeltaTime);


        if ((_moveDirection.x > 0 && !_facingRight) || (_moveDirection.x < 0 && _facingRight)) 
        {
            Vector3 currentScale = gameObject.transform.localScale;
            currentScale.x *= -1;
            gameObject.transform.localScale = currentScale;

            _facingRight = !_facingRight;
        }

    }

    protected IEnumerator Attack()
    {
        if (!_canAttack || disableEnemy)
            yield break;

        _canAttack = false;
        _isAttacking = true;

        //Play attack animation
        animator.SetTrigger("Attack");


        rb.velocity = new Vector2(0f, 0f);

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        float speed = animator.GetCurrentAnimatorStateInfo(0).speed;

        Debug.Log("Enemy Start Attack");
        Debug.Log("Animation Length: " + length + " | Animation Speed: " +  speed);

        
        yield return new WaitForSeconds(length * speed);

        //Damage is only delt if enemy is alive
        if (currentHealth > 0)
        {
            //detect enemies in range of attack
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playersLayer);

            //damage them
            foreach (Collider2D player in hitPlayers)
            {
                Debug.Log("Enemy Damaged: " + player.name + "| Enemy Health: " + currentHealth);
                player.GetComponent<PlayerController>().takeDamage(attackDamage);
            }
        }

        _isAttacking = false;

        //Attack Cooldown
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
