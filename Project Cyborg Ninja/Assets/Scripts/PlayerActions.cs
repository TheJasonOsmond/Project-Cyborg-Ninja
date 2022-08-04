using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : Player
{

    //[SerializeField] Animator _animator;

    //Bullet Fields
    [SerializeField] GameObject _playerBullet;
    [SerializeField] GameObject _bulletSpawn;
    public float bulletDamage = 50f;
    float bulletSpeed = 25f;
    

    //Attack Fields
    public LayerMask enemyLayers; 
    public Transform attackPoint;
    public float attackRange;
    float attackDamage = 100f;


    public override IEnumerator Fire()
    {

        GameObject bullet = Instantiate(_playerBullet, _bulletSpawn.transform.position, Quaternion.identity);
        //bullet.GetComponent<Bullet>().BulletDamage = bulletDamage;

        bullet.GetComponent<Rigidbody2D>().velocity = playerController.mouseOffset * bulletSpeed;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, playerController.aimAngle);

        yield return new WaitForSeconds(2);
        Destroy(bullet);

    }

    public override IEnumerator Attack()
    {
        yield break;
    }
    //public override IEnumerator Attack()
    //{
    //    isAttacking = true;

    //    //Play attack animation
    //    animator.SetTrigger("attack");

    //    yield return new WaitForSeconds(.3f);

    //    //detect enemies in range of attack
    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    //    //damage them
    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        enemy.GetComponent<Enemy>().takeDamage(attackDamage);


    //        Debug.Log("We hit " + enemy.name + " for " + attackDamage);
    //    }

    //    isAttacking = false;
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    if (attackPoint == null)
    //        return;

    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    //}

}
