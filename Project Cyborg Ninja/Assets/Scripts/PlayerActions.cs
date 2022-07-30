using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : PlayerController
{

    //[SerializeField] Animator _animator;

    //Bullet Fields
    [SerializeField] GameObject _playerBullet;
    [SerializeField] GameObject _bulletSpawn;
    float _bulletSpeed = 15f;
    int _bulletDamage = 40;

    //Attack Fields
    public LayerMask enemyLayers; 
    public Transform attackPoint;
    public float attackRange;
    int attackDamage = 100;

    public int BulletDamage
    {
        get
        {
            return _bulletDamage;
        }
        set
        {
            _bulletDamage = value;
        }
    }


    protected override IEnumerator Fire()
    {

        GameObject bullet = Instantiate(_playerBullet, _bulletSpawn.transform.position, Quaternion.identity);
        //bullet.GetComponent<Bullet>().BulletDamage = bulletDamage;

        bullet.GetComponent<Rigidbody2D>().velocity = MouseOffset * _bulletSpeed;

        yield return new WaitForSeconds(2);
        Destroy(bullet);



    }

    protected override IEnumerator Attack()
    {
        isAttacking = true;

        //Play attack animation
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(.3f);

        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().takeDamage(attackDamage);


            Debug.Log("We hit " + enemy.name + " for " + attackDamage);
        }

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
