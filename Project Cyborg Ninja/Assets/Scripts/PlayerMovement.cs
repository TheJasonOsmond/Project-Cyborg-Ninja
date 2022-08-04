using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerController
{
    [SerializeField] PlayerController _controller;

    //public void MovePlayer()
    //{
    //    // If vertical or horizontal and not disabled
    //    if ((super.inputHorizontal != 0 || inputVertical != 0) && !canMove)
    //    {
    //        //if diagonal movement, reduce speed
    //        if (inputHorizontal != 0 && inputVertical != 0)
    //        {
    //            _moveVelocity *= _moveSpeedLimiter;
    //        }

    //        if (isAttacking)
    //        {
    //            _moveVelocity *= attackMoveSpeedMod;
    //        }
    //        //Physics
    //        _rb.velocity = _moveVelocity;

    //        //Animate Movement
    //        if (_inputHorizontal > 0 || _inputHorizontal < 0)
    //            animator.SetInteger("facingDirection", 1);

    //        else if (_inputVertical < 0)
    //            animator.SetInteger("facingDirection", 0);

    //        else if (_inputVertical > 0)
    //            animator.SetInteger("facingDirection", 2);

    //        animator.SetBool("isWalking", true);
    //        //ChangeAnimationState(PLAYER_WALK_RIGHT);
    //    }
    //    // Don't move
    //    else
    //    {
    //        //Physics
    //        _moveVelocity = new Vector2(0, 0);
    //        _rb.velocity = _moveVelocity;

    //        //Animation
    //        animator.SetBool("isWalking", false);
    //    }
    //}
}
