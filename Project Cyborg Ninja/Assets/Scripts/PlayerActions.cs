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
    bool _firstFrame = true;
    float _slowedSpeedModifier = 0.3f;

    //Attack Fields
    const string PLAYER_ATTACK_1 = "Player_attack_1";

    protected override IEnumerator Fire()
    {
        GameObject bullet = Instantiate(_playerBullet, _bulletSpawn.transform.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().velocity = MouseOffset * _bulletSpeed;

        yield return new WaitForSeconds(2);
        Destroy(bullet);
    }

    protected override void Attack()
    {

        //ChangeAnimationState(PLAYER_ATTACK_1);
        //Play attack animation
        //detect enemies in range of attack
        //damage them
        //yield return new WaitForSeconds(2); 


        
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_1") && _firstFrame)
        {
            //This animation is already playing
            inAnimation = true;
            animator.SetTrigger("Attack");
            //_moveSpeed = 3f;
            //MoveDisabled = false;
            _firstFrame = false;


        }
        
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_1") && !_firstFrame)
        { 
            Debug.Log("End of Animation");
            _firstFrame = true;
            isAttacking = false;
            inAnimation = false;
        }
            
    }



}
