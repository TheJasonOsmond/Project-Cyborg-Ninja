using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : Enemy
{
    //Movement Fields
    Vector2 _moveDirection;
    float _enemyMoveSpeed = 2f;
    bool _facingRight = false;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!gameManager._gameOver && !disableEnemy)
        {
            MoveEnemy();
            //RotateEnemy();
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
}
