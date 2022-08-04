using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : Player
{

    //Attack Animation Extras **REMOVE WHEN ANIMATIONS ARE REPLACED
    [SerializeField] Transform animAttackDown;
    [SerializeField] Transform animAttackSide;
    [SerializeField] Transform animAttackUp;

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
        playerController.isAttacking = true;

        //Play attack animation
        playerController.animator.SetTrigger("attack");

        float animationLength = 0.7f;

        //Debug.Log(playerController.animTimes.attackTime);

        StartCoroutine(AnimateAttack(animationLength));
        yield return new WaitForSeconds(animationLength);

        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().takeDamage(attackDamage);

            Debug.Log("We hit " + enemy.name + " for " + attackDamage);
        }

        playerController.isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Enables Additional Animation Sections **REMOVE WHEN ANIMATIONS ARE REPLACED
    IEnumerator AnimateAttack(float duration)
    {
        if (!playerController.isAttacking)
            yield break;

        int facingDirection = playerController.animator.GetInteger("facingDirection");

        GameObject animationObject;

        if (facingDirection == 0)
            animationObject = animAttackDown.gameObject;

        else if (facingDirection == 1)
            animationObject = animAttackSide.gameObject;

        else if (facingDirection == 2)
            animationObject = animAttackUp.gameObject;
        else
            yield break;

        animationObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        animationObject.SetActive(false);

    }
}
